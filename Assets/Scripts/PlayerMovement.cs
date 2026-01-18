using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*
Bu script oyuncunun hareketlerini kontrol eder.
Oyuncu:
- Sağa sola koşar
- Zıplar
- Merdivene tırmanır
- Ateş eder
- Düşmana veya tuzağa değerse ölür
*/

public class PlayerMovement : MonoBehaviour
{
    /*
    moveSpeed:
    Oyuncunun sağa sola ne kadar hızlı gideceği.
    */
    [SerializeField] float moveSpeed = 10f;

    /*
    jumpSpeed:
    Oyuncunun ne kadar yükseğe zıplayacağı.
    */
    [SerializeField] float jumpSpeed = 5f;

    /*
    climbSpeed:
    Merdivende yukarı aşağı çıkma hızı.
    */
    [SerializeField] float climbSpeed = 5f;

    /*
    deathKick:
    Oyuncu ölürken geriye ve yukarı savrulması.
    */
    [SerializeField] Vector2 deathKick = new Vector2(10f, 20f);

    /*
    bullet:
    Ateş edildiğinde çıkan mermi.
    */
    [SerializeField] GameObject bullet;

    /*
    gun:
    Merminin çıkacağı nokta (silah ucu).
    */
    [SerializeField] Transform gun;

    /*
    moveInput:
    Oyuncunun klavyeden verdiği yön bilgisi.
    */
    Vector2 moveInput;

    /*
    Fizik ve animasyonla ilgili parçalar.
    */
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    /*
    Oyunun başındaki yerçekimi değeri.
    Merdivene girince değiştirip sonra geri almak için.
    */
    float gravityScaleAtStart;

    /*
    Oyuncu hayatta mı?
    Ölürse hareket edemez.
    */
    bool isAlive = true;

    /*
    Start:
    Oyun başlarken 1 kere çalışır.
    Gerekli parçaları bulur.
    */
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        myFeetCollider = GetComponent<BoxCollider2D>();
    }

    /*
    Update:
    Oyun çalıştığı sürece sürekli çalışır.
    Oyuncu hayattaysa hareketler kontrol edilir.
    */
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    /*
    OnMove:
    Oyuncu sağa veya sola bastığında çalışır.
    */
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    /*
    OnJump:
    Oyuncu zıplama tuşuna bastığında çalışır.
    Sadece yerdeyken zıplanabilir.
    */
    void OnJump(InputValue value)
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    /*
    Run:
    Oyuncuyu sağa sola hareket ettirir.
    Aynı zamanda koşma animasyonunu kontrol eder.
    */
    void Run()
    {
        Vector2 playerVelocity =
            new Vector2(moveInput.x * moveSpeed, myRigidbody.linearVelocity.y);

        myRigidbody.linearVelocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }

    /*
    FlipSprite:
    Oyuncunun baktığı yönü değiştirir.
    Sağa gidiyorsa sağa, sola gidiyorsa sola bakar.
    */
    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale =
                new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);
        }
    }

    /*
    ClimbLadder:
    Oyuncu merdivene değerse tırmanabilir.
    Yerçekimi kapatılır ve yukarı aşağı hareket eder.
    */
    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }

        myRigidbody.gravityScale = 0f;

        Vector2 climbVelocity =
            new Vector2(myRigidbody.linearVelocity.x, moveInput.y * climbSpeed);

        myRigidbody.linearVelocity = climbVelocity;

        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", hasVerticalSpeed);
    }

    /*
    OnAttack:
    Oyuncu saldırı tuşuna bastığında çalışır.
    Silahın ucundan mermi çıkar.
    */
    void OnAttack(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    /*
    Die:
    Oyuncu düşmanlara veya tuzaklara değerse ölür.
    Ölme animasyonu oynar ve oyun durumu değiştirilir.
    */
    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.linearVelocity = deathKick;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
}
