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
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            if (healthScript.isAlive) {
                if (!Application.isMobilePlatform) {
                    transform.localEulerAngles = new Vector3(0, rotationX, 0);
                } else {
                    transform.localEulerAngles = new Vector3(0, inputScript.GetRotationY(), 0);
                }
            }
            if (cc.isGrounded) {
                // float horizontalMovement = Mathf.Clamp(inputScript.GetHorizontalMovement(), -speedLimit, speedLimit);
                // float verticalMovement = Mathf.Clamp(inputScript.GetVerticalMovement(), -speedLimit, speedLimit);

                // if (!Application.isMobilePlatform) {
                //     horizontalMovement = Input.GetAxis("Horizontal") * 50;
                //     verticalMovement = Input.GetAxis("Vertical") * 50;
                // }

                // forwardVector = transform.forward * movementSpeed * verticalMovement;
                // horizontalVector = transform.right * movementSpeed * horizontalMovement;

                // if (!attackScript.isAttacking && healthScript.isAlive) {
                //     cc.SimpleMove(forwardVector + horizontalVector);
                // }

                // Move forward / backward
                Vector3 forwardVector = transform.TransformDirection(Vector3.forward) * movementSpeed * Input.GetAxis("Vertical") * 50;
                Vector3 horizontalVector = transform.TransformDirection(Vector3.right) * movementSpeed * Input.GetAxis("Horizontal") * 50;
                cc.SimpleMove(forwardVector + horizontalVector);
            }
            else {
                cc.SimpleMove(Vector3.forward * 0);
            }
        }
    }
}
