using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DummyCrewmate : MonoBehaviour, IInteractionPromptProvider
{
	public string crewmateName = "Crewmate";
	public string crewmateType = "Sailor";
	[TextArea(1,10)]
	public string nameplateFormat = "{0}\n-{1}-";
	public TextMeshProUGUI nameplate;

	public bool speakThroughDebug = false;

	private void Start() {
		UpdateNameplate();
	}

	public void Say(string message) {
		if (speakThroughDebug) {
			Debug.Log(FormatDialogue(message));
		}
		else {
			DialogueManager.DisplayText(FormatDialogue(message));
		}
	}

	private string FormatDialogue(string message) {
		return crewmateName + " says, \"" + message + "\"";
	}

	private void UpdateNameplate() {
		nameplate.text = string.Format(nameplateFormat, crewmateName, crewmateType);
	}

	public string GetInteractionPrompt() {
		return "speak with " + crewmateName;
	}

}
