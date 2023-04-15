using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StormDeckRitual : MonoBehaviour
{
	private static Dictionary<Type, StormDeckRitual> ritualDictionary = new Dictionary<Type, StormDeckRitual>();
	
	public UnityEvent onRitualEnd;
	
	public abstract void StartRitual();

	protected void Awake() {
		Register(this);
	}

	private static void Register(StormDeckRitual ritual) {
		ritualDictionary.Add(ritual.GetType(), ritual);
	}

	public static StormDeckRitual Run(Type ritualType) {
		if (!ritualDictionary.ContainsKey(ritualType)) {
			throw new ArgumentException("A ritual of type " + ritualType.ToString() + " does not exist in the scene.");
		}
		ritualDictionary[ritualType].StartRitual();
		return ritualDictionary[ritualType];
	}
}
