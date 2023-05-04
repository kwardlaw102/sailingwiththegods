using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public abstract class YarnFlagResponse
{
	private static Dictionary<string, YarnFlagResponse> responseDictionary = new Dictionary<string, YarnFlagResponse>();

	public static void RegisterResponse(string flagVariableName, YarnFlagResponse response) {
		if (responseDictionary.ContainsKey(flagVariableName)) {
			Debug.LogError("Tried to create a duplicate response for an existing Yarn flag; this is currently not supported"); //TODO: support multiple responses for same variable; but when should control be restored?
			return;
		}
		responseDictionary.Add(flagVariableName, response);
	}

	public static void ProcessResponses(InMemoryVariableStorage storage) {
		foreach (string variableName in responseDictionary.Keys) {
			if (storage.GetValue(variableName).AsBool) {
				responseDictionary[variableName].Invoke();
			}
		}
	}

	protected void EnableControls() {
		StormDeckManager.instance.EnableControls();
	}

	public abstract void Invoke();
}

public class RunRitualResponse : YarnFlagResponse
{
	private System.Type ritualType;

	public RunRitualResponse(System.Type ritualType) {
		this.ritualType = ritualType;
	}


	public override void Invoke() {
		StormDeckRitual ritual = StormDeckRitual.Run(ritualType);
		ritual.onRitualEnd.AddListener(StormDeckManager.instance.EnableControls);
	}
}

public class FuncResponse : YarnFlagResponse
{
	private UnityAction functionCall;

	public FuncResponse(UnityAction functionCall) {
		this.functionCall = functionCall;
	}

	public override void Invoke() {
		functionCall.Invoke();
		StormDeckManager.instance.EnableControls();
	}
}