using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vendingScript : MonoBehaviour
{
    private gameManager managerScript;
    public GameObject snackSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    public void Dispense()
    {
        print(snackSpawnPoint.transform.position);
        Instantiate(
            managerScript.snacks[Random.Range(0, managerScript.snacks.Count)], 
            snackSpawnPoint.transform.position, 
            snackSpawnPoint.transform.rotation);
    }
}
