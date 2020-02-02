using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages;
    public GameObject startPage;
    GameObject currentPage;

    // Start is called before the first frame update
    void Start()
    {
        currentPage = startPage;
        foreach (GameObject p in pages) 
        {
            if (p == startPage)
                p.SetActive(true);
            else
                p.SetActive(false);
        }
    }

    public void SwitchPage(GameObject newPage)
    {
        currentPage.SetActive(false);
        newPage.SetActive(true);
        currentPage = newPage;
    }
}
