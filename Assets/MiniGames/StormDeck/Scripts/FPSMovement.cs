using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSMovement : MonoBehaviour
{
	public float moveSpeed = 3f;
	public KeyCode FORWARD_KEY = KeyCode.W;
	public KeyCode BACK_KEY = KeyCode.S;
	public KeyCode LEFT_KEY = KeyCode.A;
	public KeyCode RIGHT_KEY = KeyCode.D;

	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		Vector3 inputDirection = GetInputDirection();
		rb.velocity = GetMovementDirection(inputDirection) * moveSpeed + Vector3.up * rb.velocity.y;
    }

	public void Stop() {
		rb.velocity = Vector3.zero;
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
