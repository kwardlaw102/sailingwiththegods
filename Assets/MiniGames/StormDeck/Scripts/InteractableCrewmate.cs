using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableCrewmate : MonoBehaviour, InteractionPromptProvider
{
	[Header("Configuration")]
	public string crewmateName = "Crewmate";
	public CrewType crewmateType = CrewType.Sailor;
	[TextArea(1,10)]
	public string nameplateFormat = "{0}\n-{1}-";

	[Header("Object References")]
	public TextMeshProUGUI nameplate;

	private UISystem UI => Globals.UI;
	private DialogScreen dialogScreen;

	private void Start() {
		UpdateNameplate();
	}

	public void SetNameAndType(string name, CrewType type) {
		crewmateName = name;
		crewmateType = type;
		UpdateNameplate();
	}
	private void UpdateNameplate() {
		nameplate.text = string.Format(nameplateFormat, crewmateName, crewmateType);
	}

	public string GetInteractionPrompt() {
		return "speak with " + crewmateName;
	}

	public void StartDialogue() {
		if (UI == null) {
			Debug.LogWarning("The global UISystem is null. Please load this minigame through the main game using the playtesting hotkeys.");
			return;
		}
		StormDeckManager.instance.DisableControls();
		dialogScreen = UI.Show<DialogScreen>();
		SetDialogVariables();
		dialogScreen.StartDialog("StartStormDeck", "darken"); // WARNING: if this errors, the player's controls will not re-enable
		StartCoroutine(WaitForDialogEnd());
	}

	private void OnDialogEnd() {
		YarnFlagResponse.ProcessFlags(dialogScreen.Storage);
	}

	private void SetDialogVariables() {
		YarnVariableList variableList = new YarnVariableList();
		variableList.Add("crewName", crewmateName);
		variableList.Add("crewType", crewmateType.ToString());
		dialogScreen.Storage.defaultVariables = variableList.ToArray();
		dialogScreen.Storage.ResetToDefaults();
	}

	IEnumerator WaitForDialogEnd() {
		while (dialogScreen.gameObject.activeInHierarchy) {
			yield return new WaitForEndOfFrame();
		}
		OnDialogEnd();
	}
}