using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimBalloonSpawn : MonoBehaviour
{
    public LayerMask layer;

    private RaycastHit hit;
    private pointAtPlayerLookLocation lookScript;

    // Start is called before the first frame update
    void Start()
    {
        lookScript = GameObject.FindGameObjectWithTag("balloonSpawn").GetComponent<pointAtPlayerLookLocation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 100.0f, layer)) {
            lookScript.target = hit.point;
        }
    }
}
