using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
	private const float ROTATION_SCALE = 200f;
	private const float MAX_CAMERA_PITCH_DEG = 90f;

	[Header("Configuration")]
	public float vertLookSensitivity = 1f;
	public float horizLookSensitivity = 1f;
	public bool invertYAxis = false;

	[Header("Object References")]
	[SerializeField] private Transform character = null;
	
	private float cameraPitch = 0;

	private void Update() {
		UpdatePitch();
		if (character != null) {
			UpdateCharacterRotation();
		}		
	}

	private void UpdateCharacterRotation() {
		float mouseDeltaX = Input.GetAxis("Mouse X");
		float deltaYaw = mouseDeltaX * Time.deltaTime * ROTATION_SCALE * horizLookSensitivity;
		character.rotation *= Quaternion.AngleAxis(deltaYaw, Vector3.up);
	}

	private void UpdatePitch() {
		float mouseDeltaY = Input.GetAxis("Mouse Y");
		int invertY = invertYAxis ? -1 : 1;
		cameraPitch += mouseDeltaY * Time.deltaTime * ROTATION_SCALE * vertLookSensitivity * invertY;
		cameraPitch = Mathf.Clamp(cameraPitch, -MAX_CAMERA_PITCH_DEG, MAX_CAMERA_PITCH_DEG);
		transform.localRotation = Quaternion.AngleAxis(cameraPitch, Vector3.left);
	}

	private void OnEnable() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	private void OnDisable() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, transform.forward);
	}
}
