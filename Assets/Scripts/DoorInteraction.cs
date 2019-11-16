using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float openSpeed = 4;
    public float openAngle = 105;

     public bool isInteractable = true;
     public bool openOnce = false;
    
    [SerializeField]
    private bool isOpen = false;
    private Quaternion startingRotation;
    private Quaternion endingRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        if (isOpen)
        {
            endingRotation = transform.localRotation;
            Vector3 rot = endingRotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y - openAngle, rot.z);
            startingRotation = Quaternion.Euler(rot);
        }
        else
        {
            startingRotation = transform.localRotation;
            Vector3 rot = startingRotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y - openAngle, rot.z);
            endingRotation = Quaternion.Euler(rot);    
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            if (!openOnce && !isInteractable && V3Equal(transform.localRotation.eulerAngles, endingRotation.eulerAngles)) {
                isInteractable = true;
            } else {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, endingRotation, Time.deltaTime * openSpeed);
            }
        } else {
            if (!isInteractable && V3Equal(transform.localRotation.eulerAngles, startingRotation.eulerAngles)) {
                isInteractable = true;
            } else {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, startingRotation, Time.deltaTime * openSpeed);
            }
        }
    }

    public void Interact()
    {
        isInteractable = false;
        isOpen = !isOpen;
    }

    // Rewrites Unity's default Vector3 comparision operator to make it less sensitive
    public bool V3Equal(Vector3 a, Vector3 b) {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }
}
