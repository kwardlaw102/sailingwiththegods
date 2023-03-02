using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DummyCrewmate : MonoBehaviour
{
	public string crewmateName = "Crewmate";
	public string crewmateType = "Sailor";
	[TextArea(1,10)]
	public string nameplateFormat = "{0}\n-{1}-";
	public TextMeshProUGUI nameplate;

	private void Start() {
		UpdateNameplate();
	}

	public void Say(string message) {
		Debug.Log(crewmateName + " says, \"" + message + "\"");
	}

	private void UpdateNameplate() {
		nameplate.text = string.Format(nameplateFormat, crewmateName, crewmateType);
	}

}
