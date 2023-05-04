using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceMinigame : StormDeckRitual
{
	private int targetValue, rolls;
	public GameObject d1, d2, d3, d4, d5;
	public Text totalText;
	public Text infoText;
	public GameObject ritualUI;

    // Start is called before the first frame update
    void Start()
    {
		Reset();
    }

	public void Reset() //Reset the amount of rolls the player has used, and sets a new randomly generated target value
	{
		rolls = 0;
		targetValue = Random.Range(5, 30);
		while (targetValue == 6 || targetValue == 29) { //It is impossible for 5 astragaloi values to add up to 6 or 29
			targetValue = Random.Range(5, 30);
		}
		infoText.text = "Roll numbers that add up to " + targetValue;
		UpdateTotalText(0);
	}

	public void DicesRolled() //After the dice have been rolled, add all new and old values and determine if the new value matches the target
	{
		rolls++;
		int total = d1.GetComponent<Dice>().GetValue() + d2.GetComponent<Dice>().GetValue() + d3.GetComponent<Dice>().GetValue() + d4.GetComponent<Dice>().GetValue() + d5.GetComponent<Dice>().GetValue();
		Debug.Log("total: " + total + ", target: " + targetValue + ", rolls: " + rolls);
		if(total == targetValue) { //If the player meets the target sum the ritual is complete and the UI box closes
			Debug.Log("You win!");
			ritualUI.SetActive(false);
			onRitualEnd.Invoke();
		}
		UpdateTotalText(total);
	}

	public override void StartRitual() {
		Reset();
		ritualUI.SetActive(true);
	}

	private void UpdateTotalText(int total) {
		totalText.text = "Your current total is " + total + " and you have used " + rolls + " rolls.";
	}
}
