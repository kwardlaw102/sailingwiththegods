using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour 
{
	private int value;
	private bool hold;
	public int GetValue() 
	{
		return value;
	}
	public void ToggleHold() 
	{
		hold = !hold;
	}

	public void Roll() 
	{
		if(hold == false)
		{
			int roll = Random.Range(0, 4);
			switch (roll) { //Randomly chooses a number 0-3 and assigns a value based on real astragaloi numbers
				case 0:
					value = 1;
					break;
				case 1:
					value = 3;
					break;
				case 2:
					value = 4;
					break;
				case 3:
					value = 6;
					break;
			}
		}
		this.gameObject.GetComponent<Text>().text = value.ToString();
	}
}
