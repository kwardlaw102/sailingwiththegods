using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewDialogueManager : MonoBehaviour
{
	public static CrewDialogueManager instance;
	public static CrewMember crewMember = null;
	public static GameObject crewObject = null;

	private void Awake() {
		instance = this;
	}

	public static void SpeakTo(GameObject _crewObject) {
		crewObject = _crewObject;
		crewMember = StormDeckManager.instance.CrewObjectToCrewMember(_crewObject);
	}

	public static void SpeakTo(CrewMember _crewMember) {
		crewMember = _crewMember;
		crewObject = StormDeckManager.instance.CrewMemberToCrewObject(_crewMember);
	}

	public static void OnDialogueEnd() {
		crewMember = null;
		crewObject = null;
	}
}
