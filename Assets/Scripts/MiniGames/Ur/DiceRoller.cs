//David Herrod	
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
	public Text diceRoll;
	public GameObject[] diceModels;
	public Vector3[] markUpPositions;
	public Vector3[] blankUpPositions;

	public int RollDice()
	{
		//1 is a blank, 2 is a mark
		int[] diceRolls = new int[diceModels.Length];

		for (int i = 0; i < diceRolls.Length; i++) {
			diceRolls[i] = Random.Range(1, 3);
		}

		//Rotate the dice to show the appropriate mark/blank
		for (int i = 0; i < diceRolls.Length; i++) {
			if (diceRolls[i] % 2 == 0) {
				diceModels[i].transform.eulerAngles = markUpPositions.RandomElement();
			}
			else {
				diceModels[i].transform.eulerAngles = blankUpPositions.RandomElement();
			}
			diceModels[i].transform.eulerAngles += Vector3.up * Random.Range(0f, 360f);
		}

		//Calculate - we're hard-coding it to 3 because there's not really a nice formula for the roll
		int marks = (diceRolls[0] % 2 == 0 ? 1 : 0) + (diceRolls[1] % 2 == 0 ? 1 : 0) + (diceRolls[2] % 2 == 0 ? 1 : 0);
		int roll = marks == 3 ? 5 : marks;
		diceRoll.text = roll.ToString();

		return roll;
	}

	private void DiceRotate(GameObject dice) 
	{

	}
	//	public Rigidbody[] dice;
	//	public Transform[] diceToRotate;
	//#pragma warning disable 0649
	//	private Animator diceAnimator;
	//#pragma warning restore 0649
	//	public Animator playerAnimator;
	//	public UrGameController urCont;
	//	public GameObject diceParent;

	//	private int drv = 0;
	//	private bool rolledDice = true;
	//	public Vector3 d1Init;
	//	public Vector3 d2Init;
	//	public Vector3 d3Init;

	//private void Start() {
	//	SetDicePosition();
	//}
	//void Update() {
	//	//if (Input.GetKeyDown("b")) {
	//	//	foreach (Rigidbody d in dice) {
	//	//		d.AddForce(Vector3.up * (Random.Range(76, 101)));
	//	//		d.AddTorque(-transform.forward * (Random.Range(7, 13)));
	//	//	}
	//	//	StartCoroutine(WaitToCount());

	//	//}

	//	//if (Input.GetKeyDown("i")) {
	//	//	playerAnimator.SetTrigger("RollDiceAction");
	//	//}

	//}

	//   IEnumerator WaitToCount() {
	//	drv = 0;
	//	yield return new WaitForSeconds(1.5f);
	//	foreach (Rigidbody d in dice) {
	//		//Debug.Log(d.transform.GetChild(0).transform.up.y);
	//		//Debug.Log(d.transform.GetChild(1).transform.up.y);
	//		if (d.transform.GetChild(0).transform.up.y >=0.8f || d.transform.GetChild(1).transform.up.y >= 0.8f) {
	//			drv++;
	//		}
	//	}
	//	urCont.SetDiceValue(drv);
	//	//Debug.Log(drv);
	//}

	//IEnumerator AnimDelay() {
	//	ThrowDice();
	//	yield return new WaitForSeconds(0.04f);
	//	playerAnimator.SetTrigger("RollDiceAction");
	//}

	//IEnumerator AnimDelayZwei() {
	//	playerAnimator.SetTrigger("StartDROver");
	//	yield return new WaitForSeconds(0.8f);
	//	 PickUpDice();
	//	foreach (Rigidbody d in dice) {
	//		d.Sleep();
	//	}
	//	dice[0].transform.position = d1Init;
	//	dice[1].transform.position = d2Init;
	//	dice[2].transform.position = d3Init;
	//	foreach (Rigidbody d in dice) {
	//		d.WakeUp();
	//	}
	//	rolledDice = false;
	//}
	////private void Update() {
	////if(Input.GetKeyDown("i")) {
	////	playerAnimator.SetTrigger("RollDiceAction");
	////}
	////}
	//public int DiceResult(int value) {
	//	if(value == 0) {
	//		return 0;
	//	}
	//	else if(value == 1) {
	//		return 1;
	//	}
	//	else if(value == 2) {
	//		return 4;
	//	}
	//	else if(value == 3) {
	//		return 5;
	//	}
	//	else {
	//		return 0;
	//	}

	//}

	//public void DiceAnimation(int dr) {
	//	diceAnimator.SetTrigger(dr);
	//}

	//public int GetDiceRollValue() {
	//	return drv;
	//}
	//public void StartDiceRoll() {
	//	if (!rolledDice) { 
	//	StartCoroutine(AnimDelay());
	//		foreach (Transform t in diceToRotate) {
	//			t.rotation = Random.rotation;
	//		}
	//		foreach (Rigidbody d in dice) {
	//		d.AddForce(Vector3.forward * (Random.Range(36, 44)));
	//		d.AddTorque(-transform.forward * (Random.Range(2, 15)));
	//	}
	//		rolledDice = true;
	//	StartCoroutine(WaitToCount());
	//}
	//	else if(rolledDice){
	//		StartCoroutine(AnimDelayZwei());
	//		//foreach (Rigidbody d in dice) {
	//		//	d.Sleep();
	//		//}
	//		//dice[0].transform.position = d1Init;
	//		//dice[1].transform.position = d2Init;
	//		//dice[2].transform.position = d3Init;
	//		//foreach (Rigidbody d in dice) {
	//		//	d.WakeUp();
	//		//}
	//		//rolledDice = false;
	//	}
	//}

	//public void PickUpDice() {
	//	diceParent.SetActive(false);
	//}
	//public void ThrowDice() {
	//	diceParent.SetActive(true);
	//}
	//public void SetDicePosition() {
	//	d1Init = dice[0].transform.position;
	//	d2Init = dice[1].transform.position;
	//	d3Init = dice[2].transform.position;
	//}






}


