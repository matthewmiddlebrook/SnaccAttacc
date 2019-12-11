using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vendingScript : MonoBehaviour
{
    private gameManager managerScript;
    private GameObject snackSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dispense()
    {
        Instantiate(
            managerScript.snacks[Random.Range(0, managerScript.snacks.Count)], 
            snackSpawnPoint.transform.position, 
            Quaternion.identity);
    }
}
