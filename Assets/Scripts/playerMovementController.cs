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
    private Rigidbody rb;
    private float rotationX;
    private float rotationY;
    private float yDis;
	private RaycastHit hit;
    private Vector3 moveVector;
    private Vector3 forwardVector;
    private Vector3 horizontalVector;
    private Vector3 verticalVector;
    private touchInputController inputScript;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputScript = GameObject.FindGameObjectWithTag("touchInputController").GetComponent<touchInputController>();
    }

    // Update is called once per frame
    void Update()
    {
        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * lookSpeed;

        rotationY += Mathf.Clamp(Input.GetAxis("Mouse Y") * lookSpeed, minY, maxY);

        transform.localEulerAngles = new Vector3(0, inputScript.GetRotationY(), 0);

        if (Physics.Raycast(transform.position, -transform.up, out hit, 100.0f)) {
            yDis = Vector3.Distance(new Vector3(0, transform.position.y, 0), new Vector3(0, hit.point.y,0));
            if (yDis > onGroundThreshold) {
                Vector3 tmp = new Vector3(0, yDis - 1, 0);
                transform.position -= tmp;
            }
        }
    }

    void FixedUpdate() {
    	float horizontalMovement = Mathf.Clamp(inputScript.GetHorizontalMovement(), -speedLimit, speedLimit);
        float verticalMovement = Mathf.Clamp(inputScript.GetVerticalMovement(), -speedLimit, speedLimit);

		forwardVector = transform.forward * movementSpeed * verticalMovement;
        horizontalVector = transform.right * movementSpeed * horizontalMovement;

        Move();
    }

    void Move() {
        rb.velocity = forwardVector + horizontalVector;
    }
}
