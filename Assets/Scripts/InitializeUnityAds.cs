using EasyButtons;
using UnityEngine;
using UnityEngine.Advertisements;

public class InitializeUnityAds : MonoBehaviour { 

    string gameId = "3455491";
    
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
