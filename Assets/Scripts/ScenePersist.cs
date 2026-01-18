using UnityEngine;

/*
Bu script bir objenin sahneler arasında kaybolmamasını sağlar.
Mesela müzik, skor veya oyun ayarları gibi şeyler
sahne değişse bile aynı kalır.
Ama aynı objeden sadece 1 tane olmasına dikkat eder.
*/

public class ScenePersist : MonoBehaviour
{
    /*
    Awake:
    Oyun açılır açılmaz çalışan ilk fonksiyonlardan biridir.
    Sahne yüklenirken hemen kontrol yapar.
    */
    void Awake()
    {
        /*
        Sahnedeki kaç tane ScenePersist olduğunu sayar.
        */
        int numberScenePersists =
            FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;

        /*
        Eğer 1’den fazla varsa:
        - Bu obje yok edilir
        (Çünkü fazladan kopyaya gerek yok)
        */
        if (numberScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            /*
            Eğer bu tek ise:
            - Sahne değişse bile silinmez
            */
            DontDestroyOnLoad(gameObject);
        }
    }

    /*
    ResetScenePersist:
    Bu obje artık gerekmediğinde çağrılır.
    Mesela oyun tamamen sıfırlanacaksa.
    */
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
