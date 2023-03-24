using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDeckManager : MonoBehaviour
{
	private List<GameObject> crewObjects = new List<GameObject>();
	[SerializeField]
	private GameObject crewPrefab = null;
	public List<Transform> spawnLocations;

	private IList<CrewMember> crewRoster;
	int malefactorNdx;

	void Start()
    {
		crewRoster = GetCrewRoster();
		malefactorNdx = ChooseMalefactor();
		Debug.Log(crewRoster[malefactorNdx].name + " is the malefactor");

		SpawnCrew();
		crewObjects[malefactorNdx].GetComponentInChildren<FPSInteractable>().OnInteract.RemoveAllListeners();
		crewObjects[malefactorNdx].GetComponentInChildren<FPSInteractable>().OnInteract.AddListener(() => SacrificeCrewMember(crewRoster, crewRoster[malefactorNdx]));
	}

	private void CrewRosterTest(IList<CrewMember> crewList) {
		Debug.Log("Crew List:");
		foreach (CrewMember c in crewList) {
			Debug.Log(c.name);
		}
		Debug.Log("End of list");
	}

	private void SacrificeCrewMember(IList<CrewMember> crewList, CrewMember c) {
		Destroy(crewObjects[crewList.IndexOf(c)]);
		crewList.Remove(c);
		//Debug.Log(c.name + " has been thrown overboard.");
		DialogueManager.DisplayText(c.name + " has been thrown overboard.");
	}

	private IList<CrewMember> GetCrewRoster() {
		IList<CrewMember> cr = Globals.Game?.Session?.playerShipVariables?.ship?.crewRoster;
		if (cr == null) {
			Debug.LogWarning("Failed to find existing crew roster. Generating dummy roster.");
			cr = GenerateDebugCrew();
		}
		return cr;
	}

	private IList<CrewMember> GenerateDebugCrew() {
		string[] names = { "Seercules", "Harold", "Paul", "Michael", "John" };
		CrewType[] types = { CrewType.Seer, CrewType.Warrior, CrewType.Sailor, CrewType.Navigator, CrewType.Lawyer };

		IList<CrewMember> list = new Ship.CrewRosterList();
		for (int i = 0; i < names.Length; i++) {
			list.Add(new CrewMember(i, names[i], 0, 0, types[i], "", true, false, false, null));
		}
		return list;
	}

	private void SpawnCrew() {
		int ndx = 0;
		foreach (CrewMember c in crewRoster) {
			if (ndx >= spawnLocations.Count) {
				Debug.LogError("Attempted to spawn more crew than the number of spawn locations.");
				return;
			}
			GameObject crewObj = Instantiate(crewPrefab, transform); // instantiating new game objects in scene root causes bugs when minigame exits
			crewObj.transform.position = spawnLocations[ndx].position;
			crewObj.GetComponent<DummyCrewmate>().SetNameAndType(c.name, c.typeOfCrew);
			crewObjects.Add(crewObj);
			ndx++;
		}
	}

	private int ChooseMalefactor() {
		// TODO: make sure chosen malefactor is killable; return -1 if no crew members are killable
		return Random.Range(0, crewRoster.Count);
	}
}
