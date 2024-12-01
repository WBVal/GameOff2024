using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cheat
{
	[CreateAssetMenu(fileName = "SeeTargetCheat", menuName = "ScriptableObjects/Cheats/SeeTargetCheat")]
	public class SeeTargetCheatCode : ScriptableCheatCode
	{
		public override void Apply()
		{
			base.Apply();
			PlayerStatsManager.Instance.CheatSeeTarget();
		}
	}
}