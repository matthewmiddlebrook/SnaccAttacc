using EasyButtons;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeUnityAds : MonoBehaviour
{

#if UNITY_IOS
    private string gameId = "3474294";
#elif UNITY_ANDROID
    private string gameId = "3474295";
#endif

    void Start()
    {
        Advertisement.Initialize(gameId);
    }

    [Button]
    public void ShowAd()
    {
        Advertisement.Show();
    }
}
