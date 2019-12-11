using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementController : MonoBehaviour
{
    public float movementSpeed;
    public float speedLimit;
    public float lookSpeed;
    public float minY;
    public float maxY;
    public float onGroundThreshold;

    private Vector3 moveDirection;
    private CharacterController cc;
    private float rotationX;
    private float yDis;
	private RaycastHit hit;
    private Vector3 moveVector;
    private Vector3 forwardVector;
    private Vector3 horizontalVector;
    private Vector3 verticalVector;
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

        }
    }

    void FixedUpdate() {
        if (!managerScript.isPaused) {
            float horizontalMovement = Mathf.Clamp(inputScript.GetHorizontalMovement(), -speedLimit, speedLimit);
            float verticalMovement = Mathf.Clamp(inputScript.GetVerticalMovement(), -speedLimit, speedLimit);

            if (!Application.isMobilePlatform) {
                horizontalMovement = Input.GetAxis("Horizontal") * 50;
                verticalMovement = Input.GetAxis("Vertical") * 50;
            }

            forwardVector = transform.forward * movementSpeed * verticalMovement;
            horizontalVector = transform.right * movementSpeed * horizontalMovement;

            Move();
        }
    }

    void Move() {
        if (!attackScript.isAttacking && healthScript.isAlive) {
            // rb.velocity = forwardVector + horizontalVector;
            cc.SimpleMove(forwardVector+horizontalVector);
        }
        else {
            // rb.velocity = new Vector3(0, 0, 0);
        }
    }
}
