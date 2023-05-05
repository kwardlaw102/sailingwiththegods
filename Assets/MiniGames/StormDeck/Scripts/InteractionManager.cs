using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	[Header("Configuration")]
	public float interactionDistance = 5f;
	public KeyCode interactKey = KeyCode.E;
	public LayerMask interactableLayers;

	[Header("Object Refererences")]
	[SerializeField] private TMPro.TextMeshProUGUI interactionPrompt = null;

	private FPSInteractable hoveredInteractable;
	private bool canInteract = false;
	private bool isInteractionPromptVisible => interactionPrompt.enabled;

	public static InteractionManager instance { get; private set; }

	private void Awake() {
		if (interactionPrompt == null) {
			Debug.LogError("The interactionPrompt field must be set.");
			enabled = false;
			return;
		}

		// to make debugging easier, setting the instance should be the final check so we don't set an invalid InteractionManager as the singleton instance
		if (instance != null) {
			Debug.LogError("Only one instance of " + GetType() + " should exist at a time.");
			return;
		}
		instance = this;
	}

	void Update() {
		Vector3 cursorPosition = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight) / 2; // assuming cursor is at center of the screen
		FPSInteractable curHoveredInteractable = GetHoveredInteractable(cursorPosition);

		if (!CanInteract() || (curHoveredInteractable == null && isInteractionPromptVisible)) {
			HideInteractionPrompt();
		}
		else if (hoveredInteractable != curHoveredInteractable && curHoveredInteractable != null) {
			ShowInteractionPrompt(curHoveredInteractable.GetInteractionPrompt());
		}
		else if (curHoveredInteractable != null && Input.GetKeyDown(interactKey)) {
			curHoveredInteractable.OnInteract.Invoke();
		}
		hoveredInteractable = curHoveredInteractable;
    }

	public void EnableInteraction() {
		canInteract = true;
	}

	public void DisableInteraction() {
		canInteract = false;
	}

	private FPSInteractable GetHoveredInteractable(Vector3 cursorPosition) {
		Ray interactionRay = Camera.main.ScreenPointToRay(cursorPosition);
		bool didFindObject = Physics.Raycast(interactionRay, out RaycastHit hitInfo, interactionDistance, interactableLayers, QueryTriggerInteraction.Collide);
		if (!didFindObject) return null;
		return hitInfo.collider.GetComponentInParent<FPSInteractable>(); // return the interactable object this collider is part of
	}

	private void ShowInteractionPrompt(string prompt) {
		interactionPrompt.text = "Press " + interactKey.ToString() + " to " + prompt;
		interactionPrompt.enabled = true;
	}

	private void HideInteractionPrompt() {
		interactionPrompt.enabled = false;
	}

	private bool CanInteract() {
		return canInteract;
	}
}
