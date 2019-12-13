using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class blockage : MonoBehaviour
{
    [Header("Settings")]
    public int blockagePrice;
    public float delay;
	public GameObject[] roomSpawns;

    public GameObject[] doors;

    [Header("Runtime Variables")]
    public bool isInteractable = true;

    private gameManager managerScript;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void ClearBlockage() {
        audioSource.clip = managerScript.doorOpenSound;
        audioSource.Play();

    	foreach (GameObject d in doors) {
            d.GetComponent<DoorInteraction>().Interact();
        }
        // transform.GetChild(0).GetComponent<Animator>().Play("clear");
    	isInteractable = false;

    	gameManager tmp = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    	tmp.SubtractPoints(blockagePrice);
    	tmp.AddSpawns(roomSpawns);

        foreach (GameObject d in doors) {
            d.GetComponent<Outline>().enabled = false;
        }

    	Destroy(gameObject, delay);
    }
}
