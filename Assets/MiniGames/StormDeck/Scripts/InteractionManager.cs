using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
	public float interactionDistance = 5f;
	public LayerMask interactableLayers;
	private Vector3 cursorLocation;
	public KeyCode interactKey = KeyCode.E;
	private FPSInteractable hoveredInteractable;
	public TMPro.TextMeshProUGUI interactionPrompt;

	private bool interactionPromptIsShowing = false;

	void Update() {
		cursorLocation = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight) / 2;
		FPSInteractable curHoveredInteractable = GetHoveredInteractable(cursorLocation);

		if (hoveredInteractable != curHoveredInteractable && curHoveredInteractable != null) {
			ShowInteractionPrompt(curHoveredInteractable.GetInteractionPrompt());
		}
		if (curHoveredInteractable == null && interactionPromptIsShowing) {
			HideInteractionPrompt();
		}
		if (curHoveredInteractable != null && Input.GetKeyDown(interactKey)) {
			curHoveredInteractable.OnInteract.Invoke();
		}
		hoveredInteractable = curHoveredInteractable;
    }

	FPSInteractable GetHoveredInteractable(Vector3 cursorPosition) {
		Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool foundObject = Physics.Raycast(interactionRay, out RaycastHit hitInfo, interactionDistance, interactableLayers, QueryTriggerInteraction.Collide);
		if (!foundObject) return null;
		return hitInfo.collider.transform.GetComponent<FPSInteractable>();
	}

	private void ShowInteractionPrompt(string prompt) {
		interactionPrompt.text = "Press " + interactKey.ToString() + " to " + prompt;
		interactionPrompt.enabled = true;
		interactionPromptIsShowing = true;
	}

	private void HideInteractionPrompt() {
		interactionPrompt.enabled = false;
		interactionPromptIsShowing = false;
	}
}
