using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cheat
{
	[CreateAssetMenu(fileName = "NoPoliceCheat", menuName = "ScriptableObjects/Cheats/NoPoliceCheat")]
	public class NoPoliceCheatCode : ScriptableCheatCode
	{
		public override void Apply()
		{
			PlayerStatsManager.Instance.CheatNoPolice();
		}
	}
}