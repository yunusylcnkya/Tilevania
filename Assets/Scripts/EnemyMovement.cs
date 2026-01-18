using UnityEngine;

/*
Bu script düşmanın hareketini kontrol eder.
Düşman sürekli sağa veya sola yürür.
Bir yerden çıkınca yön değiştirir ve ters tarafa döner.
*/

public class EnemyMovement : MonoBehaviour
{
    /*
    moveSpeed:
    Düşmanın yürüme hızı.
    Pozitifse sağa, negatifse sola gider.
    */
    [SerializeField] float moveSpeed = 1f;

    /*
    myRigidbody:
    Düşmanın fizik kurallarına uymasını sağlar.
    */
    Rigidbody2D myRigidbody;

    /*
    Start:
    Oyun başlarken 1 kere çalışır.
    Düşmanın Rigidbody2D parçasını alır.
    */
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    /*
    Update:
    Oyun çalıştığı sürece sürekli çalışır.
    Düşmanı yatay olarak yürütür.
    */
    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    /*
    OnTriggerExit2D:
    Düşman bir alanın dışına çıktığında çalışır.
    Mesela platformun ucuna gelince burası tetiklenir.
    */
    void OnTriggerExit2D(Collider2D collision)
    {
        /*
        Yönü tersine çevirir.
        Sağa gidiyorsa sola, sola gidiyorsa sağa gider.
        */
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    /*
    FlipEnemyFacing:
    Düşmanın baktığı yönü değiştirir.
    Böylece yüzü gittiği tarafa döner.
    */
    void FlipEnemyFacing()
    {
        transform.localScale =
            new Vector2(-(Mathf.Sign(myRigidbody.linearVelocity.x)), 1f);
    }
}
