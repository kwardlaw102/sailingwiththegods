using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceGameToggle : StormDeckRitual
{
	public GameObject diceMinigameUI;
	public DiceMinigame diceMinigame;

    void Update()
    {
		if (Input.GetKeyUp(KeyCode.G))
			ToggleDiceGame();
    }

	public override void StartRitual() {
		ToggleDiceGame();
	}

	public void ToggleDiceGame() {
		diceMinigameUI.SetActive(!diceMinigameUI.activeSelf);
		diceMinigame.Reset();
	}
}
