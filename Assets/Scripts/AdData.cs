using System;
using UnityEngine;

[Serializable]
public class AdData
{
    public string adLink;
    public string imageLink;
    public ExpirationDate expirationDate;
}

public class CachedAdData
{
    public string adLink;
    public Texture2D image;
    public ExpirationDate expirationDate;
}

[Serializable]
public class AdDataArray
{
    public AdData[] ads;
}

[Serializable]
public class ExpirationDate
{
    public int _seconds;
    public int _nanoseconds;

    public ExpirationDate(int s) {
        _seconds = s;
    }
}