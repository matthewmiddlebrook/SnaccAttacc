using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomAdSaver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // AdData data = new AdData();
        // data.adlink = "matthewmiddlebrook.com";
        // data.imageLink = "https://i.pinimg.com/originals/33/04/8a/33048aa1286f6ea64f687174ba639271.jpg";
        // data.expirationDate = new ExpirationDate(1579932000);
        // string json = JsonUtility.ToJson(data);
        // print(json);

        
    }

    IEnumerator DownloadJson(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else {
            Texture2D texture = new DownloadHandlerFile(request);
        }

    }
}
