using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDeckManager : MonoBehaviour
{
	private List<GameObject> crewObjects = new List<GameObject>();

	[Header("Minigame Info")]
	public MiniGameInfoScreen minigameInfoScreen;
	public string eventTitle;
	public string eventSubtitle;
	public Sprite eventIcon;

	[Header("Event Components")]
	public GameObject crewPrefab = null;
	public List<Transform> spawnLocations;
	public FPSCamera fpsCamera;
	public FPSMovement fpsMovement;
	public Canvas eventUI;

	private IList<CrewMember> crewRoster;
	int malefactorNdx;

	[Header("Event Info Dialog")]
	[TextArea(1, 3)]
	public string eventStartMessage1;
	[TextArea(1,3)]
	public string eventStartMessage2;

	public List<string> malefactorFlavorText;
	public List<string> historicalQuotes;

	public static StormDeckManager instance;

	private void Awake() {
		instance = this;
	}

	void Start()
    {
		crewRoster = GetCrewRoster();
		malefactorNdx = ChooseMalefactor();
		Debug.Log(crewRoster[malefactorNdx].name + " is the malefactor");

		SpawnCrew();
		crewObjects[malefactorNdx].GetComponentInChildren<FPSInteractable>().OnInteract.RemoveAllListeners();
		crewObjects[malefactorNdx].GetComponentInChildren<FPSInteractable>().OnInteract.AddListener(() => SacrificeCrewMember(crewRoster, crewRoster[malefactorNdx]));

		StartCoroutine(UnlockCursorForMenuCoroutine());
		ShowEventInfoDialog();
	}

	private IEnumerator UnlockCursorForMenuCoroutine() {
		DisableControls();
		while (minigameInfoScreen.gameObject.activeInHierarchy) {
			yield return new WaitForEndOfFrame();
		}
		EnableControls();
	}

	private string GenerateEventStartDialog() {
		string result = "";
		result += eventStartMessage1;
		foreach (string text in GetMalefactorFlavorTexts(2)) {
			result += " " + text;
		}
		result += eventStartMessage2;
		result += GetHistoricalQuote();
		return result;
	}

	private void ShowEventInfoDialog() {
		string eventStartDialog = GenerateEventStartDialog();
		minigameInfoScreen.DisplayText(eventTitle, eventSubtitle, eventStartDialog, eventIcon, MiniGameInfoScreen.MiniGame.StormDeckStart);
	}

	private List<string> GetMalefactorFlavorTexts(int count = 1) {
		if (count > malefactorFlavorText.Count || count < 0) {
			throw new System.ArgumentOutOfRangeException("Input 'count' must be nonnegative and cannot exceed number of available malefactor flavor text entries.");
		}

		List<string> copy = new List<string>(malefactorFlavorText);
		List<string> output = new List<string>();
		for (int i = 0; i < count; i++) {
			int ndx = Random.Range(0, copy.Count);
			output.Add(copy[ndx]);
			copy.RemoveAt(ndx);
		}
		return output;
	}

	private string GetHistoricalQuote() {
		if (historicalQuotes.Count == 0) return "";
		int ndx = Random.Range(0, historicalQuotes.Count);
		return historicalQuotes[ndx];
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

	public void DisableControls() {
		fpsCamera.enabled = false;
		fpsMovement.enabled = false;

	}

	public void EnableControls() {
		fpsCamera.enabled = true;
		fpsMovement.enabled = true;
	}

	public void HideUI() {
		eventUI.enabled = false;
	}

	public void ShowUI() {
		eventUI.enabled = true;
	}

	// Player attempts to sail away after they have performed all of their rituals
	public void EndMinigame() {
		if (Random.Range(0, 100) > 50) {
			Victory();
		}
		else {
			Failure();
		}
		if (Globals.MiniGames == null) {
			Debug.LogWarning("Player attempted to end the game, but the minigame manager (Globals.MiniGames) is null.");
			return;
		}
		Globals.MiniGames.Exit();
	}

	private void Victory() {
		Debug.Log("Victory!");
	}

	private void Failure() {
		Debug.Log("Failure...");
	}
}
