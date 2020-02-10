using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;

using EasyButtons;

public class CustomAd : MonoBehaviour
{
    public Image adImage;
    public Text countdownText;
    
    public float waitingTime;
    public UnityEvent AfterLoading;
    public UnityEvent AfterWaiting;

    CustomAdDatabaseLoader adDatabaseLoader;
    AdManager adManager;
    CachedAdData ad;
    bool hasWaited;
    float waitTimer;

    void OnEnable()
    {
        hasWaited = false;
        waitTimer = waitingTime;

        GameObject adDB = GameObject.FindGameObjectWithTag("adDatabase");
        
        adManager = adDB.GetComponent<AdManager>();
        adDatabaseLoader = adDB.GetComponent<CustomAdDatabaseLoader>();

        if (adDatabaseLoader.isLoaded)
            LoadAd();
    }

    void Update() {
        if (!hasWaited) {
            if (waitTimer <= 0) {
                hasWaited = true;
                AfterWaiting.Invoke();
            } else {
                waitTimer -= Time.deltaTime;
                countdownText.text = ((int)(waitTimer+1)).ToString();
            }
        }
    }

    [Button]
    public void LoadAd()
    {
        ad = adDatabaseLoader.GetAd();
        adImage.sprite = Sprite.Create(ad.image, new Rect(0, 0, ad.image.width, ad.image.height), new Vector2(0.5f, 0.5f));
        AfterLoading.Invoke();
    }

    public void LoadLink()
    {
        Application.OpenURL("https://"+ad.adLink);
    }

    public void CloseAd()
    {
        adManager.OnCustomAdDidFinish();
        Destroy(gameObject);
    }
}
