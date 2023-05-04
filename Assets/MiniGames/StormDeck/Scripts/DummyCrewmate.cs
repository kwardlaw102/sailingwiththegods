using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;
public class DummyCrewmate : MonoBehaviour, InteractionPromptProvider
{
	private UISystem UI => Globals.UI;
	private DialogScreen dialogScreen;

	public string crewmateName = "Crewmate";
	public CrewType crewmateType = CrewType.Sailor;
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
			DialogueManager.instance.DisplayNotification(FormatDialogue(message));
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

	public void SetNameAndType(string name, CrewType type) {
		crewmateName = name;
		crewmateType = type;
		UpdateNameplate();
	}

	public void StartDialogue() {
		Debug.Log(crewmateName + " talks to you.");
		if (UI == null) {
			Debug.LogWarning("The global UISystem is null. Please load this minigame through the main game using the playtesting hotkeys.");
			return;
		}
		StormDeckManager.instance.DisableControls();
		dialogScreen = UI.Show<DialogScreen>();
		SetDialogVariables();
		dialogScreen.StartDialog("StartStormDeck", "darken"); // if this errors, the player's controls will not re-enable
		StartCoroutine(WaitForDialogEnd());
	}

	private void SetDialogVariables() {
		YarnVariableList variableList = new YarnVariableList();
		variableList.Add("crewName", crewmateName);
		variableList.Add("crewType", crewmateType.ToString());
		dialogScreen.Storage.defaultVariables = variableList.ToArray();
		dialogScreen.Storage.ResetToDefaults();
	}

	IEnumerator WaitForDialogEnd() {
		while (dialogScreen.gameObject.activeInHierarchy)
		{
			yield return new WaitForEndOfFrame();
		}
		OnDialogEnd();
	}

	private void OnDialogEnd() {
		YarnFlagResponse.ProcessResponses(dialogScreen.Storage);
		//ProcessYarnFlags();
	}

	private void ProcessYarnFlags() {
		List<YarnFlagResponse> responses = new List<YarnFlagResponse>();

		return;
		bool restoreControls = true;

		if (CheckDialogFlag("$startDiceRitual")) {
			RunRitual(typeof(DiceMinigame));
			restoreControls = false;
		}
		else if (CheckDialogFlag("$startSacrificeAnimalRitual")) {
			Debug.Log("Start animal sacrifice");
			RunRitual(typeof(TestForQuests));
			restoreControls = false;
		}

		if (restoreControls) {
			StormDeckManager.instance.EnableControls();
		}
	}

	

	private void RunRitual(System.Type ritualType) {
		StormDeckRitual ritual = StormDeckRitual.Run(ritualType);
		ritual.onRitualEnd.AddListener(StormDeckManager.instance.EnableControls);
	}

	private StormDeckRitual TriggerRitual() {
		if (CheckDialogFlag("$startDiceRitual")) {
			return StormDeckRitual.Run(typeof(DiceMinigame));
		}
		else if (CheckDialogFlag("$startSacrificeAnimalRitual")) {
			Debug.Log("Start animal sacrifice");
			return StormDeckRitual.Run(typeof(TestForQuests));
		}
		return null;
	}

	private bool CheckDialogFlag(string variableName) {
		return dialogScreen.Storage.GetValue(variableName).AsBool;
	}
}
