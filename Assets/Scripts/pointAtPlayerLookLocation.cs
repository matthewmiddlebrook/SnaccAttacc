using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointAtPlayerLookLocation : MonoBehaviour
{
    [HideInInspector] public Vector3 target;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        Quaternion rotation = Quaternion.LookRotation(target - transform.position);
    	transform.rotation = rotation;
    }
}
