//David Herrod
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UrGameController : MonoBehaviour
{
	public Vector2Int rewardAmts;

	[Header("Player")]
	public string playerTag;
	public List<UrPiece> playerPieces;
	public List<UrGameTile> playerBoardPositions;
	public GameObject playerPathLine;

	[Header("Enemy")]
	public string enemyTag;
	public List<UrGameTile> enemyBoardPositions;
	public GameObject enemyPathLine;
	
	[Header("UI")]
	public MiniGameInfoScreen mgScreen;
	public Sprite gameIcon;
	public TavernaMiniGameDialog playerBarks;
	public TavernaEnemyDialog enemyBarks;
	public float barkChance = 0.25f;
	public Button rollDiceButton;
	public Text alertText;
	public float alertShowTime;
	public float alertFadeSpeed;

	[Header("Text")]
	[TextArea(2, 30)]
	public string introText;
	[TextArea(2, 30)]
	public string instructions;
	[TextArea(2, 30)]
	public string history;
	[TextArea(2, 30)]
	public string winText;
	[TextArea(2, 30)]
	public string loseText;

	private bool isGameOver = false;
	private int currentRoll;
	private bool isPlayerTurn = true;
	private bool allowPlayerMove = false;

	private UrDiceRoller dice;
	[HideInInspector] public UrAIController enemyAI;

	private Color baseAlertColor;
	private Outline alertOutline;
	private Color baseOutlineColor;
	private Coroutine fadeCoroutine;

	private List<string> flavor;
	private List<string> winFlavor;
	private List<string> loseFlavor;

	public void Awake() 
	{
		//Assign variables
		enemyAI = GetComponent<UrAIController>();
		dice = GetComponent<UrDiceRoller>();

		//Show the UI
		mgScreen.gameObject.SetActive(true);

		if (Globals.GameVars != null) 
		{
			//Load the lists in from GameVars
		}
		else 
		{
			flavor = new List<string> { "Ur intro flavor 1", "Ur intro flavor 2", "Ur intro flavor 3" };
			winFlavor = new List<string> { "Ur win flavor 1", "Ur win flavor 2", "Ur win flavor 3" };
			loseFlavor = new List<string> { "Ur lose flavor 1", "Ur lose flavor 2", "Ur lose flavor 3" };
		}

		string text = introText + "\n\n" + instructions + "\n\n" + flavor.RandomElement();
		mgScreen.DisplayText("The Game of Ur: An Introduction", "Taverna game", text, gameIcon, MiniGameInfoScreen.MiniGame.TavernaStart);

		//Set up the baseline for the alert colors
		baseAlertColor = alertText.color;
		alertOutline = alertText.GetComponent<Outline>();
		baseOutlineColor = alertOutline.effectColor;
		alertText.text = "";
	}

	public void PauseMinigame() {
		mgScreen.gameObject.SetActive(true);
		Time.timeScale = 0;
		mgScreen.DisplayText("The Game of Ur: Instructions and History", "Taverna game", instructions + "\n\n" + history, gameIcon, MiniGameInfoScreen.MiniGame.TavernaPause);
	}

	public void UnpauseMinigame() {
		mgScreen.gameObject.SetActive(false);
		Time.timeScale = 1;
	}

	public void ExitMinigame() {
		TavernaController.BackToTavernaMenu();
	}

	public void RestartMinigame() {
		TavernaController.ReloadTavernaGame("Ur");
	}

	/// <summary>
	/// Turns off all board tile highlights
	/// </summary>
	public void UnhighlightBoard() 
	{
		foreach (UrGameTile tile in playerBoardPositions) 
		{
			tile.ShowHighlight(false);
		}

		foreach (UrGameTile tile in enemyBoardPositions) 
		{
			tile.ShowHighlight(false);
		}
	}

	public void UnhighlightPieces() 
	{
		foreach (UrPlayerPiece piece in playerPieces) 
		{
			if (piece != null) {
				piece.ShowHighlight(false);
			}

		}
		foreach (UrPiece piece in enemyAI.enemyPieces) 
		{
			if (piece != null) {
				piece.ShowHighlight(false);
			}

		}
	}

	/// <summary>
	/// Rolls the dice and returns the result
	/// </summary>
	/// <returns></returns>
	public int GetDiceRoll() 
	{
		rollDiceButton.interactable = false;
		currentRoll = dice.RollDice(isPlayerTurn);

		return currentRoll;
	}

	/// <summary>
	/// Rolls the dice
	/// </summary>
	public void RollDice() 
	{
		rollDiceButton.interactable = false;
		currentRoll = dice.RollDice(isPlayerTurn);
		if (isPlayerTurn) {
			allowPlayerMove = true;
		}
	}

	public void SwitchTurn(bool playerTurn) 
	{
		isPlayerTurn = playerTurn;
		allowPlayerMove = false;
		rollDiceButton.interactable = isPlayerTurn;

		playerPathLine.SetActive(isPlayerTurn);
		enemyPathLine.SetActive(!isPlayerTurn);

		UnhighlightBoard();
		UnhighlightPieces();

		if (!isPlayerTurn) 
		{
			enemyAI.EnemyTurn();
		}
	}

	public IEnumerator WaitToSwitchTurn(bool playerTurn, float waitTime) 
	{
		yield return new WaitForSeconds(waitTime);
		SwitchTurn(playerTurn);
	}

	/// <summary>
	/// Checks if the specified player can move any of their pieces
	/// </summary>
	/// <param name="isPlayer">Whether to check the player or not</param>
	/// <param name="highlightPieces">Whether to highlight any mobile pieces</param>
	/// <returns></returns>
	public bool CanPlayerMove(bool isPlayer, bool highlightPieces = true) 
	{
		int movable = 0;
		List<UrGameTile> checkPath = new List<UrGameTile>();
		List<UrPiece> checkPieces = new List<UrPiece>();

		if (isPlayer) 
		{
			checkPath = playerBoardPositions;
			checkPieces = playerPieces;
		}
		else 
		{
			checkPath = enemyBoardPositions;
			checkPieces = enemyAI.enemyPieces;
		}
		
		foreach(UrPiece p in checkPieces) 
		{
			if (p.PopulateValidMovesList(checkPath, isPlayer).Count > 0) 
			{
				if (highlightPieces) 
				{
					p.ShowHighlight(true);
				}

				movable++;
			}
		}

		return movable > 0;
	}

	public void ShowAlertText(string alert) 
	{
		StartCoroutine(DoShowAlertText(alertText, alertOutline, alert));
	}

	private IEnumerator DoShowAlertText(Text t, Outline o, string alert) 
	{
		//For some reason, just calling StopCoroutine(FadeText(t, o)) doesn't work, so we have to do it this way
		if (fadeCoroutine != null) 
		{
			StopCoroutine(fadeCoroutine);
			fadeCoroutine = null;
		}
		yield return null;
		t.color = baseAlertColor;
		o.effectColor = baseOutlineColor;
		alertText.text = alert;
		yield return null;
		fadeCoroutine = StartCoroutine(FadeText(t, o));
	}

	private IEnumerator FadeText(Text t, Outline o) 
	{
		yield return new WaitForSeconds(alertShowTime);
		Color clearColor = new Color(baseAlertColor.r, baseAlertColor.g, baseAlertColor.b, 0f);
		Color clearOutline = new Color(baseOutlineColor.r, baseOutlineColor.g, baseOutlineColor.b, 0f);

		//I had a lot of trouble with this for some reason - Lerp didn't want to cooperate
		//I've seen people do something like Color.Lerp(t.color, endColor, t) with t as the for loop iterater,
		//but for some reason that wasn't giving the right results here
		for (float i = 0; i <= 1; i += Time.deltaTime * alertFadeSpeed) 
		{
			t.color = Color.Lerp(baseAlertColor, clearColor, i);
			o.effectColor = Color.Lerp(baseOutlineColor, clearOutline, i);
			yield return null;
		}

		alertText.text = "";
		alertText.color = baseAlertColor;
		o.effectColor = baseOutlineColor;
	}

	public void PointScored(bool isPlayer, UrPiece c) 
	{
		if (isPlayer) 
		{
			playerPieces.Remove(c);
			c.GetComponent<MeshRenderer>().enabled = false;
			Destroy(c.gameObject, 1f);
			if (playerPieces.Count == 0) 
			{
				WinGame();
			}
		}
		else 
		{
			enemyAI.enemyPieces.Remove(c);
			c.GetComponent<MeshRenderer>().enabled = false;
			Destroy(c.gameObject, 1f);
			if (enemyAI.enemyPieces.Count == 0) 
			{
				LoseGame();
			}
		}
	}

	private void WinGame() 
	{
		isGameOver = true;
		rollDiceButton.interactable = false;
		allowPlayerMove = false;

		mgScreen.gameObject.SetActive(true);

		//Calculate how much of a reward you get based on how many pieces your opponent still has
		int reward = Random.Range(rewardAmts.x, rewardAmts.y + 1);
		if (Globals.GameVars != null) {
			Globals.GameVars.playerShipVariables.ship.AddToFoodAndWater(reward);
		}

		string text = winText + "\n\n" + $"For your victory, you win {reward} food and water!" + "\n\n" + winFlavor.RandomElement();
		mgScreen.DisplayText("The Game of Ur: Victory!", "Taverna Game", text, gameIcon, MiniGameInfoScreen.MiniGame.TavernaEnd);
	}

	private void LoseGame() 
	{
		isGameOver = false;
		rollDiceButton.interactable = false;
		allowPlayerMove = false;

		string text = loseText + "\n\n" + "Although you have lost this round, you can always find a willing opponent to try again!" + "\n\n" + loseText.RandomElement();
		mgScreen.DisplayText("The Game of Ur: Defeat!", "Taverna Game", text, gameIcon, MiniGameInfoScreen.MiniGame.TavernaEnd);
	}

	public int CurrentRoll {
		get {
			return currentRoll;
		}
	}

	public bool IsPlayerTurn {
		get {
			return isPlayerTurn;
		}
	}

	public bool AllowPlayerMove {
		get {
			return allowPlayerMove;
		}
	}

	public bool IsGameOver {
		get {
			return isGameOver;
		}
	}
}
