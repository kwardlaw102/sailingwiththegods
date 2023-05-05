using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncenseBurner : MonoBehaviour, InteractionPromptProvider
{
	[TextArea(1, 10)]
	public string interactionResultText = "You light the thymiaterion. The skies begin to clear.";
	public string interactionPrompt = "light the incense burner"; // "Press E to..."

	public string GetInteractionPrompt() {
		return interactionPrompt;
	}

	public void Interact() {
		NotificationManager.instance.DisplayNotification(interactionResultText);
	}
}
