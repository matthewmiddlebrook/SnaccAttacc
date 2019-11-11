using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float openSpeed = 4;
    public float openAngle = 105;
    
    [SerializeField]
    private bool isOpen = false;
    private Quaternion startingRotation;
    private Quaternion endingRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        startingRotation = transform.localRotation;
        Vector3 rot = startingRotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y - openAngle, rot.z);
        endingRotation = Quaternion.Euler(rot);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
            transform.localRotation = Quaternion.Slerp(transform.localRotation, endingRotation, Time.deltaTime * openSpeed);
        else
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startingRotation, Time.deltaTime * openSpeed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}
