using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameUISystem : UISystem
{
	[SerializeField] Dashboard Dashboard = null;
	[SerializeField] TitleScreen TitleScreen = null;
	[SerializeField] PortScreen PortScreen = null;

	void AddViews() {
		Add(Dashboard);
		Add(TitleScreen);
		Add(PortScreen);
	}

	void Start() {
		AddViews();
		Globals.Register(this);
	}
}