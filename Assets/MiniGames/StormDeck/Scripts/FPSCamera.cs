using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
	public Transform character;
	private const float ROTATION_SCALE = 200f;
	private const float MAX_CAMERA_PITCH = 90f;
	private float cameraPitch = 0;

	public float vertLookSensitivity = 1f;
	public float horizLookSensitivity = 1f;
	public bool invertYAxis = false;

	// Start is called before the first frame update
	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
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
		cameraPitch = Mathf.Clamp(cameraPitch, -MAX_CAMERA_PITCH, MAX_CAMERA_PITCH);
		transform.localRotation = Quaternion.AngleAxis(cameraPitch, Vector3.left);
	}
}
