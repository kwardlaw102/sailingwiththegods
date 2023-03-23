using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDeckManager : MonoBehaviour
{
	private Ship.CrewRosterList crewRoster;
	private List<GameObject> crewObjects = new List<GameObject>();
	[SerializeField]
	private GameObject crewPrefab;
	public List<Transform> spawnLocations;

	void Start()
    {
		crewRoster = GetCrewRoster();
		SpawnCrew();

		//CrewRosterTest(crewRoster);
		//SacrificeCrewMember(crewRoster, crewRoster[0]);
	}

	private void CrewRosterTest(Ship.CrewRosterList crewList) {
		Debug.Log("Crew List:");
		foreach (CrewMember c in crewList) {
			Debug.Log(c.name);
		}
		Debug.Log("End of list");
	}

	private void SacrificeCrewMember(Ship.CrewRosterList crewList, CrewMember c) {
		crewList.Remove(c);
		Debug.Log(c.name + " has died.");
	}

	private Ship.CrewRosterList GetCrewRoster() {
		Ship.CrewRosterList cr = Globals.Game?.Session?.playerShipVariables?.ship?.crewRoster;
		if (cr == null) {
			Debug.LogWarning("Failed to find existing crew roster. Generating dummy roster.");
			cr = GenerateDummyCrew();
		}
		return cr;
	}

	private Ship.CrewRosterList GenerateDummyCrew() {
		string[] names = { "Seercules", "Harold", "Paul", "Michael", "John" };
		CrewType[] types = { CrewType.Seer, CrewType.Warrior, CrewType.Sailor, CrewType.Navigator, CrewType.Lawyer };

		Ship.CrewRosterList list = new Ship.CrewRosterList();
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
			GameObject crewObj = Instantiate(crewPrefab);
			crewObj.transform.position = spawnLocations[ndx].position;
			crewObj.GetComponent<DummyCrewmate>().SetNameAndType(c.name, c.typeOfCrew);
			crewObjects.Add(crewObj);
			ndx++;
		}
	}
}
