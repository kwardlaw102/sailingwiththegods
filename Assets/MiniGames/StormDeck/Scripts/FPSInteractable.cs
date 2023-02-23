using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInteractable : MonoBehaviour
{
	public void OnStartHover() {
		Debug.Log("Hovered over " + name);
	}

	public void OnInteract() {
		Debug.Log("Interacted with " + name);
	}

	public void OnStopHover() {
		Debug.Log("Stopped hovering over " + name);
	}
}
