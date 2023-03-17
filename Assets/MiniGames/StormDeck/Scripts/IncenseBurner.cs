using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncenseBurner : MonoBehaviour
{
	[TextArea(1, 10)]
	public string interactionText;

	public void Interact() {
		DialogueManager.DisplayText(interactionText);
	}
}
