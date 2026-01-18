using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Bu script bölüm çıkışını kontrol eder.
Oyuncu çıkış noktasına gelince:
- Biraz beklenir
- Sonraki bölüme geçilir
- Eğer son bölümse, oyun en başa döner
*/

public class LevelExit : MonoBehaviour
{
    /*
    levelLoadDelay:
    Çıkışa geldikten sonra
    kaç saniye beklenileceğini belirler.
    */
    [SerializeField] float levelLoadDelay = 1f;

    /*
    OnTriggerEnter2D:
    Bir obje çıkış alanına değdiğinde çalışır.
    Burada oyuncu gelince sonraki bölüm başlatılır.
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());
    }

    /*
    LoadNextLevel:
    Bu bir Coroutine’dir.
    Yani oyunu durdurmadan bekleyebilir.
    */
    IEnumerator LoadNextLevel()
    {
        /*
        Gerçek zamanlı olarak biraz bekler.
        (Oyun durdurulmuş olsa bile çalışır)
        */
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        /*
        Şu anki sahnenin numarasını alır.
        */
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        /*
        Bir sonraki sahnenin numarasını hesaplar.
        */
        int nextSceneIndex = currentSceneIndex + 1;

        /*
        Eğer son sahnedeysek:
        - Baştaki sahneye geri döner
        */
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        /*
        Sahne geçmeden önce
        sahneler arası taşınan objeyi sıfırlar.
        */
        FindFirstObjectByType<ScenePersist>().ResetScenePersist();

        /*
        Yeni sahneyi yükler.
        */
        SceneManager.LoadScene(nextSceneIndex);
    }
}
