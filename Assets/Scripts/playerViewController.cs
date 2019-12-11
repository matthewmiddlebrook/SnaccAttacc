using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerViewController : MonoBehaviour
{
    public float lookSpeed;
    public float minY;
    public float maxY;

    private float rotationX;
    private float rotationY;
    private GameObject player;
    private Vector3 offset;
    private playerHealth healthScript;
    private touchInputController inputScript;

    private gameManager managerScript;

    // Start is called before the first frame update
    void Start()
    {
        inputScript = GameObject.FindGameObjectWithTag("touchInputController").GetComponent<touchInputController>();
        player = GameObject.FindGameObjectWithTag("player");
        offset = player.transform.position - transform.position;
        healthScript = transform.parent.GetChild(0).GetComponent<playerHealth>();

        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!managerScript.isPaused) {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY -= Input.GetAxis("Mouse Y") * lookSpeed, minY, maxY);


            if (healthScript && healthScript.isAlive) {
                if (!Application.isMobilePlatform) {
                    transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
                } else {
                    transform.localEulerAngles = new Vector3(inputScript.GetRotationX(), inputScript.GetRotationY(), 0);
                }
            }
        }
    }
}
