using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiceGameToggle : MonoBehaviour
{
	private bool isEnabled;
	public GameObject diceMinigame;
	private static DiceGameToggle instance;
	public UnityEvent onRitualEnd;
    // Start is called before the first frame update

	private void Awake() 
	{
		instance = this;
	}
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
	public static DiceGameToggle StartRitual() {
		instance.ToggleDiceGame();
		return instance;
	}

	public void ToggleDiceGame() {
		isEnabled = !isEnabled;
		diceMinigame.GetComponent<DiceMinigame>().Reset();
	}
}
