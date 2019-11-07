using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockage : MonoBehaviour
{
    [Header("Settings")]
    public int blockagePrice;
    public float delay;
	public GameObject[] roomSpawns;

    [Header("Runtime Variables")]
    public bool isInteractable = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearBlockage() {
    	transform.GetChild(0).GetComponent<Animator>().Play("clear");
    	isInteractable = false;

    	gameManager tmp = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    	tmp.SubtractPoints(blockagePrice);
    	tmp.AddSpawns(roomSpawns);

    	Destroy(gameObject, delay);
    }
}
