using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FPSInteractable : MonoBehaviour
{	
	public UnityEvent OnHover;
	public UnityEvent OnInteract;
	public UnityEvent OnStopHover;
	[SerializeField]
	public IInteractionPromptProvider interactionPromptProvider;

	public void StartHover() {
		Debug.Log("Hovered over " + name);
	}

	public void Interact() {
		Debug.Log("Interacted with " + name);
	}

	public void StopHover() {
		Debug.Log("Stopped hovering over " + name);
	}

	public string GetInteractionPrompt() {
		if (interactionPromptProvider != null) {
			return interactionPromptProvider.GetInteractionPrompt();
		}
		return "interact";
	}
}
