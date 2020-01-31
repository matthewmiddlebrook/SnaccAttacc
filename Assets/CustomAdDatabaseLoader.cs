using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomAdDatabaseLoader : MonoBehaviour
{
    public string url;
    List<CachedAdData> ads;

    void Start()
    {
        ads = new List<CachedAdData>();
        StartCoroutine(DownloadJson(url));
    }

    IEnumerator DownloadJson(string MediaUrl)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(MediaUrl))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = MediaUrl.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError || webRequest.isHttpError) {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            } else {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                ProcessJson(webRequest.downloadHandler.text);
            }
        }
    }

    void ProcessJson(string json)
    {
        // FromJson expects a key starting the array, so we add
        // "ads": to the beginning and surround with curly braces
        AdData[] uncachedAds = JsonUtility.FromJson<AdDataArray>("{\"ads\":" + json + "}").ads;

        // We should cache the ads to the device in order to save data
        foreach (AdData a in uncachedAds) {
            StartCoroutine(CacheAd(a));
        }

    }

    IEnumerator CacheAd(AdData a)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(a.imageLink);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError) {
            Debug.Log(request.error);
        } else {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            CachedAdData cachedAd = new CachedAdData();
            cachedAd.adLink = a.adLink;
            cachedAd.expirationDate = a.expirationDate;
            cachedAd.image = texture;
            ads.Add(cachedAd);
        }

    }

    public CachedAdData GetAd() {
        return ads[Random.Range(0, ads.Count)];
    }
}
