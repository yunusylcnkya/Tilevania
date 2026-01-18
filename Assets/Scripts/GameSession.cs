using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Bu script oyunun genel durumunu yönetir.
Oyuncunun:
- Kaç canı kaldığını
- Kaç puanı olduğunu
takip eder.
Sahne değişse bile bu bilgiler kaybolmaz.
*/

public class GameSession : MonoBehaviour
{
    /*
    playerLives:
    Oyuncunun toplam can sayısı.
    */
    [SerializeField] int playerLives = 3;

    /*
    score:
    Oyuncunun topladığı puan.
    */
    [SerializeField] int score = 0;

    /*
    livesText:
    Ekranda görünen can sayısı yazısı.
    */
    [SerializeField] TextMeshProUGUI livesText;

    /*
    scoreText:
    Ekranda görünen puan yazısı.
    */
    [SerializeField] TextMeshProUGUI scoreText;

    /*
    Awake:
    Oyun açılır açılmaz çalışır.
    Bu scriptin sahnede sadece 1 tane olmasını sağlar.
    */
    void Awake()
    {
        /*
        Sahnedeki GameSession sayısını kontrol eder.
        */
        int numberGameSessions =
            FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        /*
        Eğer 1’den fazla varsa:
        - Fazla olanı siler
        */
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            /*
            Eğer tek ise:
            - Sahne değişse bile silinmez
            */
            DontDestroyOnLoad(gameObject);
        }
    }

    /*
    Start:
    Oyun başladığında 1 kere çalışır.
    Can ve puan yazılarını ekranda gösterir.
    */
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    /*
    ProcessPlayerDeath:
    Oyuncu öldüğünde çağrılır.
    Can varsa azaltır, yoksa oyunu sıfırlar.
    */
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    /*
    AddToScore:
    Oyuncu puan kazandığında çağrılır.
    Puanı artırır ve ekrana yazar.
    */
    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    /*
    TakeLife:
    Oyuncunun canını 1 azaltır.
    Aynı bölümü yeniden başlatır.
    */
    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    /*
    ResetGameSession:
    Oyuncunun canı biterse çalışır.
    - Sahne bilgilerini sıfırlar
    - Oyunu en baştan başlatır
    - GameSession objesini siler
    */
    void ResetGameSession()
    {
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
