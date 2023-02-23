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

	void Update()
    {
		cursorLocation = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight) / 2;
		FPSInteractable curHoveredInteractable = GetHoveredInteractable(cursorLocation);
		if (hoveredInteractable != curHoveredInteractable) {
			if (curHoveredInteractable != null) {
				curHoveredInteractable.OnStartHover();
			}
			else {
				hoveredInteractable.OnStopHover();
			}
			hoveredInteractable = curHoveredInteractable;
		}
		if (hoveredInteractable != null && Input.GetKeyDown(interactKey)) {
			hoveredInteractable.OnInteract();
		}
    }

	FPSInteractable GetHoveredInteractable(Vector3 cursorPosition) {
		Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool foundObject = Physics.Raycast(interactionRay, out RaycastHit hitInfo, interactionDistance, interactableLayers, QueryTriggerInteraction.Collide);
		if (!foundObject) return null;
		return hitInfo.collider.transform.GetComponent<FPSInteractable>();
	}
}
