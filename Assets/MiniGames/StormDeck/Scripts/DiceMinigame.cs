using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceMinigame : StormDeckRitual
{
	int targetValue, rolls;
	public GameObject d1, d2, d3, d4, d5;
	public Text totalText;
	public Text infoText;
	public GameObject ritualUI;

    // Start is called before the first frame update
    void Start()
    {
		Reset();
    }

	public void Reset() 
	{
		rolls = 0;
		targetValue = Random.Range(5, 30);
		while (targetValue == 6 || targetValue == 29) {
			targetValue = Random.Range(5, 30);
		}
		infoText.text = "Roll numbers that add up to " + targetValue;
		UpdateTotalText(0);
	}

	public void DicesRolled() 
	{
		rolls++;
		int total = d1.GetComponent<Dice>().GetValue() + d2.GetComponent<Dice>().GetValue() + d3.GetComponent<Dice>().GetValue() + d4.GetComponent<Dice>().GetValue() + d5.GetComponent<Dice>().GetValue();
		Debug.Log("total: " + total + ", target: " + targetValue + ", rolls: " + rolls);
		if(total == targetValue) {
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
