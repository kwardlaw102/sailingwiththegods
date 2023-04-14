using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceMinigame : MonoBehaviour
{
	int targetValue, rolls;
	public GameObject d1, d2, d3, d4, d5;
	public Text totalText;

    // Start is called before the first frame update
    void Start()
    {
		rolls = 0;
		targetValue = Random.Range(5, 30);
		while(targetValue == 6 || targetValue == 29) {
			targetValue = Random.Range(5, 30);
		}
		this.gameObject.GetComponent<Text>().text = "Roll numbers that add up to " + targetValue;
    }

	public void Reset() 
	{
		rolls = 0;
		targetValue = Random.Range(5, 30);
		while (targetValue == 6 || targetValue == 29) {
			targetValue = Random.Range(5, 30);
		}
		this.gameObject.GetComponent<Text>().text = "Roll numbers that add up to " + targetValue;
	}

	public void DicesRolled() 
	{
		rolls++;
		int total = d1.GetComponent<Dice>().GetValue() + d2.GetComponent<Dice>().GetValue() + d3.GetComponent<Dice>().GetValue() + d4.GetComponent<Dice>().GetValue() + d5.GetComponent<Dice>().GetValue();
		if(total == targetValue) 
			Debug.Log("You win!");
		totalText.text = "Your current total is " + total + " and you have used " + rolls + " rolls.";
	}
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
