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

	void Update()
    {
		cursorLocation = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight) / 2;
		FPSInteractable curHoveredInteractable = GetHoveredInteractable(cursorLocation);
		if (hoveredInteractable != curHoveredInteractable) {
			if (curHoveredInteractable != null) {
				ShowInteractionPrompt(curHoveredInteractable.GetInteractionPrompt());
				curHoveredInteractable.OnHover.Invoke();
			}
			else {
				HideInteractionPrompt();
				hoveredInteractable.OnStopHover.Invoke();
			}
			hoveredInteractable = curHoveredInteractable;
		}
		if (hoveredInteractable != null && Input.GetKeyDown(interactKey)) {
			hoveredInteractable.OnInteract.Invoke();
		}
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
		//Debug.Log("Press " + interactKey.ToString() + " to " + prompt);
	}

	private void HideInteractionPrompt() {
		//Debug.Log("Hide interaction prompt");
		interactionPrompt.enabled = false;
	}
}
