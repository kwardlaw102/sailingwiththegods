using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
	public KeyCode FORWARD_KEY = KeyCode.W;
	public KeyCode BACK_KEY = KeyCode.S;
	public KeyCode LEFT_KEY = KeyCode.A;
	public KeyCode RIGHT_KEY = KeyCode.D;
	public float moveSpeed = 3f;

    private void Update() {
		Vector3 inputDirection = GetInputDirection();
		transform.position += GetMovementDirection(inputDirection) * moveSpeed * Time.deltaTime;
    }

	private Vector3 GetMovementDirection(Vector3 inputDirection) {
		return transform.forward * inputDirection.z + transform.right * inputDirection.x;
	}

	private Vector3 GetInputDirection() {
		return new Vector3(GetSideAxisInput(), 0, GetForwardAxisInput()).normalized;
	}

	private int GetForwardAxisInput() {
		int axisValue = 0;
		if (Input.GetKey(FORWARD_KEY)) 
			axisValue++;
		if (Input.GetKey(BACK_KEY)) 
			axisValue--;
		return axisValue;
	}

	private int GetSideAxisInput() {
		int axisValue = 0;
		if (Input.GetKey(RIGHT_KEY))
			axisValue++;
		if (Input.GetKey(LEFT_KEY))
			axisValue--;
		return axisValue;
	}
}
