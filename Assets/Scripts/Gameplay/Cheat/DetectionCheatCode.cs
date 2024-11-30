using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cheat
{
	[CreateAssetMenu(fileName = "DetectionCheat", menuName = "ScriptableObjects/Cheats/DetectionCheat")]
	public class DetectionCheatCode : ScriptableCheatCode
	{
		public override void Apply()
		{
			PlayerStatsManager.Instance.CheatDetection();
		}
	}
}
