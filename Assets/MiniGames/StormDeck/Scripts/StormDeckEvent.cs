using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDeckEvent : RandomEvents.NegativeEvent
{
	public override float Weight() {
		return 2f;
	}

	public override void Execute() {
		MiniGames.EnterScene("StormDeckGame");
	}
}
