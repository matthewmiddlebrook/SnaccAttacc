using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchInputController : MonoBehaviour
{
    public float xSensitivity;
    public float ySensitivity;
    public float deadZone;
    public float tapThreshold;
    public int holdDelayDuration;
    public bool controlsInverted = false;

    private GameObject leftJoystick;
    private GameObject rightJoystick;
    private Vector2 leftJoystickOrigin;
    private Vector2 rightJoystickOrigin;
    private Vector2 leftJoystickLastPos;
    private Vector2 rightJoystickLastPos;
    private float rotationY = 0;
    private float rotationX = 0;
    private float verticalMovement = 0;
    private float horizontalMovement = 0;
    private float deltaTwoX;
	private float deltaTwoY;
	private float deltaOneX;
	private float deltaOneY;
    private Vector2 rightTapStart;
    private Vector2 rightTapEnd;
    private Vector2 leftTapStart;
    private Vector2 leftTapEnd;
    private int rightHoldDelay;
    private int leftHoldDelay;

    private GameObject player;

    void Start() {
    	leftJoystick = GameObject.FindGameObjectWithTag("leftJoystick");
    	rightJoystick = GameObject.FindGameObjectWithTag("rightJoystick");

    	leftJoystickOrigin = new Vector2(leftJoystick.transform.position.x, leftJoystick.transform.position.y);
    	rightJoystickOrigin = new Vector2(rightJoystick.transform.position.x, rightJoystick.transform.position.y);
    	leftJoystickLastPos = leftJoystickOrigin;
        rightJoystickLastPos = rightJoystickOrigin;

        
        player = GameObject.FindGameObjectWithTag("player");
    }

    void Update()
    {
        if (Input.touchCount == 1) {
        	Touch touch = Input.GetTouch(0);

        	if (touch.position.x < Screen.width/2) {
        		rightJoystick.transform.position = new Vector3 (rightJoystickOrigin.x, rightJoystickOrigin.y, rightJoystick.transform.position.z);

        		MoveLeftStick(touch);
        	}
        	else {
        		leftJoystick.transform.position = new Vector3 (leftJoystickOrigin.x, leftJoystickOrigin.y, leftJoystick.transform.position.z);

        		MoveRightStick(touch);
        	}
        }

        if (Input.touchCount == 2) {
        	Touch touch = Input.GetTouch(0);

        	if (touch.position.x < Screen.width/2) {
        		MoveLeftStick(touch);
        	}
        	else {
        		MoveRightStick(touch);
        	}

        	Touch touchTwo = Input.GetTouch(1);

        	if (touchTwo.position.x > Screen.width/2) {
        		MoveRightStick(touchTwo);
        	}
        	else {
        		MoveLeftStick(touchTwo);
        	}
        }

        if (!controlsInverted) {
            if (deltaOneX < -deadZone) {
            	horizontalMovement = deltaOneX;
            }
            else if (deltaOneX > deadZone) {
            	horizontalMovement = deltaOneX;
            }
            else {
                horizontalMovement = 0;
            }

            if (deltaOneY < -deadZone) {
            	verticalMovement = deltaOneY;
            }
            else if (deltaOneY > deadZone) {
            	verticalMovement = deltaOneY;
            }
            else {
                verticalMovement = 0;
            }

            rotationY -= -deltaTwoX * (xSensitivity / 10);
            rotationX -= deltaTwoY * (ySensitivity / 10);

            RefreshRightStick();
        }
        else {
            if (deltaTwoX < -deadZone) {
                horizontalMovement = deltaTwoX;
            }
            else if (deltaTwoX > deadZone) {
                horizontalMovement = deltaTwoX;
            }
            else {
                horizontalMovement = 0;
            }

            if (deltaTwoY < -deadZone) {
                verticalMovement = deltaTwoY;
            }
            else if (deltaTwoY > deadZone) {
                verticalMovement = deltaTwoY;
            }
            else {
                verticalMovement = 0;
            }

            rotationY -= -deltaOneX * (xSensitivity / 10);
            rotationX -= deltaOneY * (ySensitivity / 10);

            RefreshLeftStick();
        }

        if (rightHoldDelay > 0) {
            rightHoldDelay--;
        }
        if (rightHoldDelay == 0) {
            RegisterRightHold();
        }

        if (leftHoldDelay > 0) {
            leftHoldDelay--;
        }
        if (leftHoldDelay == 0) {
            RegisterLeftHold();
        }

        rightJoystickLastPos = new Vector2(rightJoystick.transform.position.x, rightJoystick.transform.position.y);
        leftJoystickLastPos = new Vector2(leftJoystick.transform.position.x, leftJoystick.transform.position.y);
    }

    void StartRightTouch(Touch touch) {
        rightTapStart = touch.position;
        rightHoldDelay = holdDelayDuration;
    }

    void EndRightTouch(Touch touch) {
        rightTapEnd = touch.position;
        ReleaseRightHold();
    }

    void StartLeftTouch(Touch touch) {
        leftTapStart = touch.position;
        leftHoldDelay = holdDelayDuration;
    }

    void EndLeftTouch(Touch touch) {
        leftTapEnd = touch.position;
        ReleaseLeftHold();
    }

    void RegisterRightTap() {
        if (Vector2.Distance(rightTapStart, rightTapEnd) < tapThreshold) {
            player.SendMessage("Tap");
        }
    }

    void RegisterLeftTap() {
        if (Vector2.Distance(leftTapStart, leftTapEnd) < tapThreshold) {
            player.SendMessage("Tap");
        }
    }

    void RegisterRightHold() {
        player.SendMessage("Hold");
    }

    void RegisterLeftHold() {
        player.SendMessage("Hold");
    }

    void ReleaseRightHold() {
        rightHoldDelay = -1;
        player.SendMessage("ReleaseHold");
    }

    void ReleaseLeftHold() {
        leftHoldDelay = -1;
        player.SendMessage("ReleaseHold");
    }

    void UpdateRightStickDeltaValues() {
        if (!controlsInverted) {
            deltaTwoX = rightJoystick.transform.position.x - rightJoystickLastPos.x;
            deltaTwoY = rightJoystick.transform.position.y - rightJoystickLastPos.y;
        }
        else {
            deltaTwoX = rightJoystick.transform.position.x - rightJoystickOrigin.x;
            deltaTwoY = rightJoystick.transform.position.y - rightJoystickOrigin.y;
        }
    }

    void UpdateLeftStickDeltaValues() {
    	if (!controlsInverted) {
            deltaOneX = leftJoystick.transform.position.x - leftJoystickOrigin.x;
            deltaOneY = leftJoystick.transform.position.y - leftJoystickOrigin.y;
        }
        else {
            deltaOneX = leftJoystick.transform.position.x - leftJoystickLastPos.x;
            deltaOneY = leftJoystick.transform.position.y - leftJoystickLastPos.y;
        }
    }

    void RefreshRightStick() {
    	deltaTwoX = 0;
        deltaTwoY = 0;
    }

    void RefreshLeftStick() {
    	deltaOneX = 0;
        deltaOneY = 0;
    }

    void MoveRightStick(Touch touch) {
    	switch (touch.phase) {
    		case TouchPhase.Began:
                StartRightTouch(touch);
                rightJoystick.transform.position = new Vector3 (touch.position.x, touch.position.y, rightJoystick.transform.position.z);
                break;
    		
    		case TouchPhase.Moved:
    			rightJoystick.transform.position = new Vector3 (touch.position.x, touch.position.y, rightJoystick.transform.position.z);
    			UpdateRightStickDeltaValues();
                break;
    		
    		case TouchPhase.Ended:
                EndRightTouch(touch);
                RegisterRightTap();
                rightJoystick.transform.position = new Vector3 (rightJoystickOrigin.x, rightJoystickOrigin.y, rightJoystick.transform.position.z);
                RefreshRightStick();
    			break;
    	}
    }

    void MoveLeftStick(Touch touch) {
    	switch (touch.phase) {
    		case TouchPhase.Began:
    			StartLeftTouch(touch);
                leftJoystick.transform.position = new Vector3 (touch.position.x, touch.position.y, leftJoystick.transform.position.z);
                break;

            case TouchPhase.Moved:
    			leftJoystick.transform.position = new Vector3 (touch.position.x, touch.position.y, leftJoystick.transform.position.z);
    			UpdateLeftStickDeltaValues();
    			break;
    		
    		case TouchPhase.Ended:
    			EndLeftTouch(touch);
                RegisterLeftTap();
                leftJoystick.transform.position = new Vector3 (leftJoystickOrigin.x, leftJoystickOrigin.y, leftJoystick.transform.position.z);
                RefreshLeftStick();
    			break;
    	}
    }

    public float GetRotationX() {
        return rotationX;
    }

    public float GetRotationY() {
        return rotationY;
    }

    public float GetVerticalMovement() {
        return verticalMovement;
    }

    public float GetHorizontalMovement() {
        return horizontalMovement;
    }
}
