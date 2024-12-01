using Audio;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Gameplay.Cheat
{
	[CreateAssetMenu(fileName = "DetectionCheat", menuName = "ScriptableObjects/Cheats/DetectionCheat")]
	public class DetectionCheatCode : ScriptableCheatCode
	{
		public override void Apply()
		{
			base.Apply();
			PlayerStatsManager.Instance.CheatDetection();
		}
	}
}
