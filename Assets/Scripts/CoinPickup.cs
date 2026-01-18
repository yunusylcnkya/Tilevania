using UnityEngine;

/*
Bu script para (coin) toplama işini yönetir.
Oyuncu paraya dokunduğunda:
- Puan kazanır
- Para alma sesi çalar
- Para sahneden kaybolur
*/

public class CoinPickup : MonoBehaviour
{
    /*
    coinPickupSFX:
    Para alındığında çalacak ses.
    */
    [SerializeField] AudioClip coinPickupSFX;

    /*
    pointsForCoinPickup:
    Bir para alındığında kazanılacak puan.
    */
    [SerializeField] int pointsForCoinPickup = 100;

    /*
    wasCollected:
    Bu para daha önce alındı mı?
    Aynı paranın iki kere alınmasını engeller.
    */
    bool wasCollected = false;

    /*
    OnTriggerEnter2D:
    Oyuncu paraya değdiği anda çalışır.
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        Eğer değen obje oyuncuysa
        VE bu para daha önce alınmadıysa:
        */
        if (other.CompareTag("Player") && !wasCollected)
        {
            /*
            Paranın alındığını işaretler.
            */
            wasCollected = true;

            /*
            Oyuncunun puanını artırır.
            */
            FindFirstObjectByType<GameSession>()
                .AddToScore(pointsForCoinPickup);

            /*
            Para alma sesini çalar.
            */
            AudioSource.PlayClipAtPoint(
                coinPickupSFX,
                transform.position
            );

            /*
            Parayı görünmez yapar
            ve tamamen yok eder.
            */
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
