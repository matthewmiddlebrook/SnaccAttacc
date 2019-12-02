using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialOnSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] materials;

    void Start()
    {
        Renderer r = gameObject.GetComponent<Renderer>();
        r.material = materials[Random.Range(0, materials.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
