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
    public UnityEvent AfterLoading;

    CustomAdDatabaseLoader adDatabaseLoader;
    CachedAdData ad;

    void OnEnable()
    {
        adDatabaseLoader = GameObject.FindGameObjectWithTag("adDatabase").GetComponent<CustomAdDatabaseLoader>();
        if (adDatabaseLoader.isLoaded)
            LoadAd();
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
        Destroy(gameObject);
    }
}
