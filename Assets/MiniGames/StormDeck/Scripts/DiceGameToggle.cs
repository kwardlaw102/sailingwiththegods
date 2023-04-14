using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGameToggle : MonoBehaviour
{
	private bool isEnabled;
	public GameObject diceMinigame;
    // Start is called before the first frame update
    void Start()
    {
		isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.G))
			ToggleDiceGame();

		if(isEnabled)
			diceMinigame.SetActive(true);
		else
			diceMinigame.SetActive(false);
    }

	public void ToggleDiceGame() {
		isEnabled = !isEnabled;
		diceMinigame.GetComponent<DiceMinigame>().Reset();
	}
}
