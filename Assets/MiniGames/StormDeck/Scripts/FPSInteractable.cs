using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FPSInteractable : MonoBehaviour
{	
	public UnityEvent OnHover;
	public UnityEvent OnInteract;
	public UnityEvent OnStopHover;
	

	public void StartHover() {
		Debug.Log("Hovered over " + name);
	}

	public void Interact() {
		Debug.Log("Interacted with " + name);
	}

	public void StopHover() {
		Debug.Log("Stopped hovering over " + name);
	}
}
