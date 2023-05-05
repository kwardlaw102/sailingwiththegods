using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StormDeckManager : MonoBehaviour
{
	[SerializeField] private MinigameInfoSettings minigameInfo = null;
	[SerializeField] private MinigameInfoSettings outcomeInfo = null;

	[Header("Object References")]
	[SerializeField] private GameObject crewPrefab = null;
	[SerializeField] private List<Transform> spawnLocations = null;
	[SerializeField] private FPSCamera fpsCamera = null;
	[SerializeField] private FPSMovement fpsMovement = null;
	[SerializeField] private GameObject minigameHUD = null;
	[SerializeField] private MiniGameInfoScreen minigameInfoScreen = null;

	[Header("Event Info Text")]
	[TextArea(1, 3)]
	[SerializeField] private string eventStartMessage1 = "";
	[TextArea(1,3)]
	[SerializeField] private string eventStartMessage2 = "";

	[Header("Flavor Text")]
	[SerializeField] private List<string> malefactorFlavorText = null;
	[SerializeField] private List<string> historicalQuotes = null;

	private float successChance = 0.05f;
	private readonly List<GameObject> crewObjects = new List<GameObject>();
	private IList<CrewMember> crewRoster = null;

	// ============== DIVINATIONS, RITUALS, AND OTHER DIALOGUE VARIABLES =================

	// Malefactor variables
	private int malefactorNdx;
	private CrewMember malefactor => crewRoster[malefactorNdx];
	private int astragaliHolderNdx;

	// ===================================================================================

	public static StormDeckManager instance { get; private set; }

	private void Awake() {
		if (instance != null) {
			Debug.LogError("Only one instance of " + GetType() + " should exist at a time.");
			return;
		}
		instance = this;
	}

	void Start()
    {
		//TODO: handle edge cases where player's crew is empty or only contains unkillable crew members
		crewRoster = SpawnCrew(PullCrewRoster());

		// Malefactor and ritual setup
		ChooseMalefactor(crewRoster);
		Debug.Log(crewRoster[malefactorNdx].name + " is the malefactor");
		crewObjects[malefactorNdx].GetComponentInChildren<FPSInteractable>().OnInteract = new UnityEvent(); // This is a workaround since RemoveAllListeners does not work for listeners assigned through inspector; see this thread: https://forum.unity.com/threads/documentation-unityevent-removealllisteners-only-removes-non-persistent-listeners.341796/ 
		crewObjects[malefactorNdx].GetComponentInChildren<FPSInteractable>().OnInteract.AddListener(() => SacrificeCrewMember(PullCrewRoster(), crewRoster[malefactorNdx]));
		SpawnAstragali(crewRoster);
		try {
			InitYarnFlagResponses(); // FIXME: since the flag responses are registered in a static variable, entering the minigame multiple times creates duplicates
		}
		catch (System.NotImplementedException e) {
			Debug.LogError(e.StackTrace);
		}
		InitAstragaliConsequence();

		StartCoroutine(ShowInfoMenuCoroutine(EnableControls));
		ShowEventInfoDialog();
	}

	// Update this function when new mechanics need to be triggered by the Yarn dialogue system
	private void InitYarnFlagResponses() {
		YarnFlagResponse.RegisterResponse("$startDiceRitual", new RunRitualResponse(typeof(DiceMinigame)));
		YarnFlagResponse.RegisterResponse("$startSacrificeAnimalRitual", new FuncResponse(() => Debug.Log("(Run sacrifice animal divination)")));
	}

	public void DisableControls() {
		fpsCamera.enabled = false;
		fpsMovement.enabled = false;
		InteractionManager.instance.DisableInteraction();
		fpsMovement.Stop();
	}

	public void EnableControls() {
		fpsCamera.enabled = true;
		fpsMovement.enabled = true;
		InteractionManager.instance.EnableInteraction();
	}

	public void HideUI() {
		minigameHUD.SetActive(false);
	}

	public void ShowUI() {
		minigameHUD.SetActive(true);
	}

	#region Crew setup

	public CrewMember CrewObjectToCrewMember(GameObject crewObject) {
		return crewRoster[crewObjects.IndexOf(crewObject)];
	}

	public GameObject CrewMemberToCrewObject(CrewMember crewMember) {
		return crewObjects[crewRoster.IndexOf(crewMember)];
	}

	private IList<CrewMember> PullCrewRoster() {
		IList<CrewMember> crewRoster = Globals.Game?.Session?.playerShipVariables?.ship?.crewRoster;
		if (crewRoster == null) {
			Debug.LogWarning("Failed to find existing crew roster. Generating dummy roster.");
			crewRoster = GenerateDebugCrew();
		}
		return crewRoster;
	}

	private IList<CrewMember> GenerateDebugCrew() {
		string[] names = { "Timothy", "Harold", "Paul", "Michael", "John" };
		CrewType[] types = { CrewType.Passenger, CrewType.Warrior, CrewType.Sailor, CrewType.Navigator, CrewType.Lawyer };
		IList<CrewMember> list = new Ship.CrewRosterList();
		for (int i = 0; i < names.Length; i++) {
			list.Add(new CrewMember(i, names[i], 0, 0, types[i], "", true, false, false, null));
		}
		return list;
	}

	private IList<CrewMember> SpawnCrew(IList<CrewMember> crewRoster) {
		IList<CrewMember> spawnedCrew = new List<CrewMember>();
		int ndx = 0;
		foreach (CrewMember crewMember in crewRoster) {
			if (ndx >= spawnLocations.Count) { // TODO: prioritize spawning certain types of crew members
				return spawnedCrew;
			}
			GameObject crewObj = Instantiate(crewPrefab, transform); // WARNING: instantiating new game objects in scene root causes bugs when minigame exits
			crewObj.transform.position = spawnLocations[ndx].position;
			crewObj.GetComponent<InteractableCrewmate>().SetNameAndType(crewMember.name, crewMember.typeOfCrew);
			crewObjects.Add(crewObj);
			spawnedCrew.Add(crewMember);
			ndx++;
		}
		return spawnedCrew;
	}

	#endregion

	#region Ritual functions

	private void ChooseMalefactor(IList<CrewMember> crewRoster) {
		int numAttempts = 0;
		do {
			malefactorNdx = Random.Range(0, crewRoster.Count);
			if (++numAttempts > 100) {
				throw new System.NotImplementedException("Failed to choose a malefactor " + (numAttempts - 1) + " times; crew likely consists only of Seers or unkillable crew. This case is not yet handled.");
			}
			numAttempts++;
		} while (!IsValidMalefactor(crewRoster[malefactorNdx])); //FIXME: infinite loop if crew only consists of Seers or unkillable crew
	}

	private bool IsValidMalefactor(CrewMember crewMember) {
		return crewMember.typeOfCrew != CrewType.Seer && crewMember.isKillable;
	}

	private void SpawnAstragali(IList<CrewMember> crewRoster) {
		int numAttempts = 0;
		do {
			astragaliHolderNdx = Random.Range(0, crewRoster.Count);
			if (++numAttempts > 100) {
				throw new System.NotImplementedException("Failed to choose a holder for the astragali " + (numAttempts - 1) + " times; crew likely consists only of Seers. This case is not yet handled.");
			}
		} while (crewRoster[astragaliHolderNdx].typeOfCrew == CrewType.Seer);
	}

	private void SacrificeCrewMember(IList<CrewMember> crewList, CrewMember crewMember) {
		Destroy(crewObjects[crewList.IndexOf(crewMember)]);
		crewList.Remove(crewMember);
		NotificationManager.instance.DisplayNotification(crewMember.name + " has been thrown overboard.");
	}

	#endregion

	#region Game flow

	// Player attempts to sail away after they have performed all of their rituals
	public void EndMinigame() {
		if (Random.Range(0f, 1f) < successChance) // TODO: success chance should increase when rituals are successfully performed
			Victory();
		else
			Failure();
	}

	private void Victory() {
		Debug.Log("Victory!");
		ShowOutcomeInfoScreen();
	}

	private void Failure() {
		Debug.Log("Failure...");
		ShowOutcomeInfoScreen();
	}

	private void ShowOutcomeInfoScreen() {
		minigameInfoScreen.gameObject.SetActive(true);
		minigameInfoScreen.DisplayText(outcomeInfo.title, outcomeInfo.subtitle, outcomeInfo.content, outcomeInfo.icon, outcomeInfo.type);
		if (Globals.MiniGames == null) {
			Debug.LogWarning("Player attempted to end the game, but the minigame manager (Globals.MiniGames) is null.");
			return;
		}
		StartCoroutine(ShowInfoMenuCoroutine(Globals.MiniGames.Exit));
	}

	private IEnumerator ShowInfoMenuCoroutine(params UnityAction[] callbacks) {
		DisableControls();
		HideUI();
		minigameInfoScreen.gameObject.SetActive(true);
		while (minigameInfoScreen.gameObject.activeInHierarchy) {
			yield return new WaitForEndOfFrame();
		}
		ShowUI();
		foreach (UnityAction callback in callbacks) {
			callback.Invoke();
		}
	}

	#endregion

	#region Event intro setup

	private void ShowEventInfoDialog() {
		string eventStartDialog = GenerateEventStartDialog();
		minigameInfoScreen.DisplayText(minigameInfo.title, minigameInfo.subtitle, eventStartDialog, minigameInfo.icon, minigameInfo.type);
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

	private List<string> GetMalefactorFlavorTexts(int count = 1) {
		if (count > malefactorFlavorText.Count || count < 0) {
			throw new System.ArgumentOutOfRangeException("Input 'count' must be nonnegative and cannot exceed number of available malefactor flavor text entries.");
		}

		List<string> output = new List<string>();
		List<string> copy = new List<string>(malefactorFlavorText);
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

	#endregion

	private void InitAstragaliConsequence() {
		if (StormDeckRitual.TryGetRitual(out DiceMinigame diceMinigame)) {
			diceMinigame.onRitualEnd.AddListener(() => {
				// TODO: succeeding at the astragali minigame should give the player hint about what ritual to perform, not directly boost their chances of escape
				NotificationManager.instance.DisplayNotification("You have performed a successful divination with the astragali,"
					+ " and your odds of successfully navigating out of the storm have increased.");
				successChance = 0.6f;
			});
		}
	}

	#region Private inner classes for collapsible inspector sections

	[System.Serializable]
	private class MinigameInfoSettings
	{
		public string title = "";
		public string subtitle = "";
		public string content = "";
		public Sprite icon = null;
		public MiniGameInfoScreen.MiniGame type = MiniGameInfoScreen.MiniGame.StormDeckStart;
	}

	#endregion
}