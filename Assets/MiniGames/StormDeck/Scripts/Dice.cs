using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour 
{
	private static readonly int[] FACE_VALUES = { 1, 3, 4, 6 }; // based on real astragali face values

	public int value { get; private set; }

	private bool hold = false;

	public void ToggleHold() {
		hold = !hold;
	}

	public void Roll() {
		if(!hold)
		{
			RandomizeValue();
		}
		GetComponent<Text>().text = value.ToString();
	}

	private void RandomizeValue() {
		value = FACE_VALUES[Random.Range(0, FACE_VALUES.Length)];
	}
}
