using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FPSInteractable : MonoBehaviour
{	
	public UnityEvent OnHover;
	public UnityEvent OnInteract;
	public UnityEvent OnStopHover;

	private void Awake() {
		InteractionPromptProvider[] promptProviders = GetComponents<InteractionPromptProvider>();
		if (promptProviders.Length > 1) {
			Debug.LogError("Interactable objects should not have multiple components that implement the InteractionPromptProvider interface.", this);
		}
	}

	public string GetInteractionPrompt() {
		if (TryGetComponent(out InteractionPromptProvider promptProvider)) {
			return promptProvider.GetInteractionPrompt();
		}
		return "interact";
	}

	// TESTING METHODS: These methods can be assigned to the UnityEvents in the Inspector to easily test the FPSInteractable script 
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
