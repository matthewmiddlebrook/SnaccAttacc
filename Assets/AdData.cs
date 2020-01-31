using System;

[Serializable]
public class AdData
{
    public string adlink;
    public string imageLink;
    public ExpirationDate expirationDate;
}

[Serializable]
public class ExpirationDate {
    public int _seconds;
    public int _nanoseconds;

    public ExpirationDate(int s) {
        _seconds = s;
    }
}