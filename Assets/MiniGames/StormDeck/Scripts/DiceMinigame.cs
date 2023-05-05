using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceMinigame : StormDeckRitual
{
	public List<Dice> dice;
	public Text totalText;
	public Text infoText;
	public GameObject ritualUI;

	private int targetValue, rolls;

	void Start()
    {
		Reset();
    }

	public override void StartRitual() {
		Reset();
		ritualUI.SetActive(true);
	}

	public void Reset() // Reset the amount of rolls the player has used, and sets a new randomly generated target value
	{
		rolls = 0;
		do {
			targetValue = Random.Range(5, 30);
		}
		while (targetValue == 6 || targetValue == 29); // It is impossible for 5 astragali values to add up to 6 or 29
		infoText.text = "Roll numbers that add up to " + targetValue;
		UpdateTotalText(0);

		foreach (Dice die in dice) {
			die.Roll(); // Randomize the dice at the start of the minigame
		}
	}

	public void RollDice() // Roll the dice, calculate the total, and check whether it matches the target value
	{
		rolls++;
		foreach (Dice die in dice) {
			die.Roll();
		}
		int total = GetDiceTotal();
		UpdateTotalText(total);
		//Debug.Log("total: " + total + ", target: " + targetValue + ", rolls: " + rolls);

		if (total == targetValue) { // If the player meets the target sum the ritual is complete and the UI box closes
			Debug.Log("You win!");
			ritualUI.SetActive(false);
			onRitualEnd.Invoke();
			return;
		}
	}

	private int GetDiceTotal() {
		int total = 0;
		foreach (Dice die in dice) {
			total += die.value;
		}
		return total;
	}

	private void UpdateTotalText(int total) {
		totalText.text = "Your current total is " + total + " and you have used " + rolls + " rolls.";
	}
}
