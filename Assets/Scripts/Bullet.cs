using Unity.VisualScripting;
using UnityEngine;

/*
Bu script mermi (bullet) davranışını kontrol eder.
Mermi:
- Oyuncunun baktığı yöne doğru gider
- Düşmana çarparsa onu yok eder
- Bir şeye çarpınca kendisi de yok olur
*/

public class Bullet : MonoBehaviour
{
    /*
    bulletSpeed:
    Merminin ne kadar hızlı gideceğini belirler.
    */
    [SerializeField] float bulletSpeed = 20f;

    // Merminin fizik hareketleri için
    Rigidbody2D myRigidbody;

    // Oyuncuya ulaşmak için
    PlayerMovement player;

    /*
    xSpeed:
    Merminin sağa mı sola mı gideceğini belirler.
    */
    float xSpeed;

    /*
    Start:
    Oyun başladığında (mermi oluşturulduğunda) çalışır.
    */
    void Start()
    {
        // Merminin Rigidbody bileşenini alır
        myRigidbody = GetComponent<Rigidbody2D>();

        // Sahnedeki oyuncuyu bulur
        player = FindFirstObjectByType<PlayerMovement>();

        /*
        Oyuncunun baktığı yöne göre merminin yönünü ayarlar.
        Oyuncu sağa bakıyorsa mermi sağa gider,
        sola bakıyorsa sola gider.
        */
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    /*
    Update:
    Her karede çalışır.
    */
    void Update()
    {
        /*
        Mermiyi yatay yönde sürekli hareket ettirir.
        */
        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    /*
    OnTriggerEnter2D:
    Mermi bir trigger objeye değdiğinde çalışır.
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        Eğer mermi bir düşmana çarparsa:
        */
        if (other.CompareTag("Enemy"))
        {
            // Düşmanı yok et
            Destroy(other.gameObject);
        }

        /*
        Mermi her durumda yok edilir
        (düşmana çarpsa da çarpmasa da).
        */
        Destroy(gameObject);
    }

    /*
    OnCollisionEnter2D:
    Mermi normal bir objeye çarptığında çalışır.
    */
    void OnCollisionEnter2D(Collision2D other)
    {
        /*
        Mermiyi 1 saniye sonra yok eder.
        */
        Destroy(gameObject, 1f);
    }
}
