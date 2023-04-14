using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StormDeckRitual : MonoBehaviour
{
	public UnityEvent onRitualEnd;
	private static StormDeckRitual instance;

	public static StormDeckRitual StartRitual() {
		return instance;
	}

	public abstract void OnStartRitual();
}
