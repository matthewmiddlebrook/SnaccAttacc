using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementController : MonoBehaviour
{
    public float movementSpeed;
    public float speedLimit;
    public float lookSpeed;

    private CharacterController cc;
    private float rotationX;
    private Vector3 forwardVector;
    private Vector3 horizontalVector;
    private Vector3 moveDirection = Vector3.zero;
    private playerAttackController attackScript;
    private playerHealth healthScript;
    private touchInputController inputScript;
    private gameManager managerScript;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        attackScript = GetComponent<playerAttackController>();
        healthScript = GetComponent<playerHealth>();
        inputScript = GameObject.FindGameObjectWithTag("touchInputController").GetComponent<touchInputController>();

        managerScript = GameObject.FindGameObjectWithTag("gameManager").GetComponent<gameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!managerScript.isPaused) {
            if (attackScript.isAttacking) {
                moveDirection = Vector3.zero;
            }
            if (healthScript.isAlive) {
                if (Application.isMobilePlatform) {
                    transform.localEulerAngles = new Vector3(0, inputScript.GetRotationY(), 0);
                } else {
                    rotationX += Input.GetAxis("Mouse X") * lookSpeed;
                    transform.localEulerAngles = new Vector3(0, rotationX, 0);
                }
            }
            if (cc.isGrounded && !attackScript.isAttacking && healthScript.isAlive)
            {
                // We are grounded, so recalculate
                // move direction directly from axes
                float horizontalMovement;
                float verticalMovement;
                // Use touch controls on mobile
                if (Application.isMobilePlatform) {
                    horizontalMovement = Mathf.Clamp(inputScript.GetHorizontalMovement(), -speedLimit, speedLimit);
                    verticalMovement = Mathf.Clamp(inputScript.GetVerticalMovement(), -speedLimit, speedLimit);
                } else {
                    horizontalMovement = Input.GetAxis("Horizontal") * speedLimit;
                    verticalMovement = Input.GetAxis("Vertical") * speedLimit;
                }

                forwardVector = transform.forward * movementSpeed * Input.GetAxis("Vertical") * 50;
                horizontalVector = transform.right * movementSpeed * Input.GetAxis("Horizontal") * 50;

                moveDirection = forwardVector + horizontalVector;

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
