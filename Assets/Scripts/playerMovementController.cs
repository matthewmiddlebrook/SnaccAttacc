using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class playerMovementController : MonoBehaviour
{
    public Camera cam;
    public float movementSpeed;
    public float speedLimit;
    public float lookSpeed;

    private CharacterController cc;
    private Vector3 forwardVector;
    private Vector3 horizontalVector;
    private float verticalMovement;
    private float horizontalMovement;
    private Vector3 moveDirection = Vector3.zero;
    private playerAttackController attackScript;
    private playerHealth healthScript;
    private touchInputController inputScript;
    private gameManager managerScript;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        cc = GetComponent<CharacterController>();
        attackScript = GetComponent<playerAttackController>();
        healthScript = GetComponent<playerHealth>();
        inputScript = GameObject.FindGameObjectWithTag("touchInputController").GetComponent<touchInputController>();

        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();

        CinemachineCore.GetInputAxis = AxisOverride;
    }
    
    public float AxisOverride(string axisName)
    {
        if (!managerScript.isPaused) {
            if (Application.isMobilePlatform || managerScript.mobileTesting) {
                if (axisName == "Mouse X") {
                    return inputScript.GetRotationY();
                } else if (axisName == "Mouse Y") {
                    return inputScript.GetRotationX();
                }
                
            } else {
                if (axisName == "Mouse X") {
                    return Input.GetAxis("Mouse X");
                } else if (axisName == "Mouse Y") {
                    return Input.GetAxis("Mouse Y");
                }
            }
        }
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!managerScript.isPaused) {
            // If you want to pause the player when attacking
            // if (attackScript.isAttacking) {
            //     moveDirection = Vector3.zero;
            // }
            if (cc.isGrounded && !attackScript.isAttacking && healthScript.isAlive)
            {
                var camera = Camera.main;
                var forward = cam.transform.forward;
                var right = cam.transform.right;

                forward.y = 0f;
                right.y = 0f;

                forward.Normalize();
                right.Normalize();

                if (Application.isMobilePlatform || managerScript.mobileTesting) {
                    verticalMovement = Mathf.Clamp(inputScript.GetVerticalMovement(), -speedLimit, speedLimit);
                    horizontalMovement = Mathf.Clamp(inputScript.GetHorizontalMovement(), -speedLimit, speedLimit);
                } else {
                    verticalMovement = Input.GetAxis("Vertical") * speedLimit;
                    horizontalMovement = Input.GetAxis("Horizontal") * speedLimit;
                }

                forwardVector = forward * movementSpeed * verticalMovement;
                horizontalVector = right * movementSpeed * horizontalMovement;

                moveDirection = forwardVector + horizontalVector;

                transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (forward), lookSpeed);
                // Just in case you wanna use jump  
                // if (Input.GetButton("Jump"))
                // {
                //     moveDirection.y = 5;
                // }
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y += Physics.gravity.y * Time.deltaTime;

            // Move the controller
            cc.Move(moveDirection * Time.deltaTime);
        }
    }
}
