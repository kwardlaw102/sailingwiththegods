using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCrewmate : MonoBehaviour
{
	public string crewmateName = "Crewmate";

	public void Say(string message) {
		Debug.Log(crewmateName + " says, \"" + message + "\"");
	}
}
