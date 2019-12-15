using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMusicController : MonoBehaviour
{
    public AudioSource audioSource;
    private gameManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = managerScript.backgroundMusic;
        audioSource.Play();
    }
}
