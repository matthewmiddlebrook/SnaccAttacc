using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    private string afterAdAction;
    public InitializeUnityAds unityAds;
    public CustomAdDatabaseLoader customAds;

    void Start() {
        Advertisement.AddListener(this);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        print("Ad completed");
        if (afterAdAction == "restart") {
            SceneManager.LoadScene("Game");
        } else if (afterAdAction == "quit") {
            SceneManager.LoadScene("Menu");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        // if (placementId == myPlacementId) {
        // Advertisement.Show (myPlacementId);
        // }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    void ShowUnityAd(string action) {
        afterAdAction = action;
        unityAds.ShowAd();
    }

    void ShowCustomAd(string action) {
        afterAdAction = action;
        customAds.ShowAd();
    }

    public void ShowAd(string action) {
        int randomness = Random.Range(0, 1);
        if (randomness >= 0.7) {
            ShowUnityAd(action);
        } else {
            ShowCustomAd(action);
        }
    }
}
