using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StormDeckRitual : MonoBehaviour
{
	private static readonly Dictionary<System.Type, StormDeckRitual> ritualDictionary = new Dictionary<System.Type, StormDeckRitual>();

	public UnityEvent onRitualEnd;
	
	public abstract void StartRitual();

	protected void Awake() {
		if (!HasRegisteredRitual(this))
			Register(this);
	}

	protected static void Register(StormDeckRitual ritual) {
		ritualDictionary.Add(ritual.GetType(), ritual);
	}

	private static bool HasRegisteredRitual(StormDeckRitual ritual) {
		return ritualDictionary.ContainsKey(ritual.GetType());
	}

	public static StormDeckRitual Run(System.Type ritualType) {
		if (!ritualDictionary.ContainsKey(ritualType)) {
			throw new System.ArgumentException("A ritual of type " + ritualType.ToString() + " does not exist in the scene.");
		}
		ritualDictionary[ritualType].StartRitual();
		return ritualDictionary[ritualType];
	}

	public static bool TryGetRitual<T>(out T ritualInstance) where T : StormDeckRitual {
		if (!ritualDictionary.ContainsKey(typeof(T))) {
			ritualInstance = null;
			return false;
		}
		ritualInstance = (T) ritualDictionary[typeof(T)];
		return true;
	}
}
