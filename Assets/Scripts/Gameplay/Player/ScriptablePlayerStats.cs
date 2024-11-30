using Gameplay.Cheat;
using Gameplay.Upgrade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
	[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
	public class ScriptablePlayerStats : ScriptableObject
	{
		public ScriptableUpgrade[] Upgrades;

		public ScriptableCheatCode[] Cheats;

		public int EyesCount = 0;

		#region Variable Stats
		public float SpeedFactor = 1.0f;
		public float DetectionSpeed = 1.0f;
		public bool NpcForgiveness;
		public bool FreeClue;
		#endregion

		#region Time Score

		public float BestStandardRunTime = float.MaxValue;
		public float BestGhostRunTime = float.MaxValue;
		public float BestLuckyRunTime = float.MaxValue;
		public float BestNoPowerRunTime = float.MaxValue;
		public float BestCheaterRunTime = float.MaxValue;
		public void AddToStandardRun(float newTime)
		{
			if (newTime < BestStandardRunTime)
			{
				BestStandardRunTime = newTime;
			}
		}
		public void AddToGhostRun(float newTime)
		{
			if (newTime < BestGhostRunTime)
			{
				BestGhostRunTime = newTime;
			}
		}
		public void AddToLuckyRun(float newTime)
		{
			if (newTime < BestLuckyRunTime)
			{
				BestLuckyRunTime = newTime;
			}
		}
		public void AddNoPowerRun(float newTime)
		{
			if (newTime < BestNoPowerRunTime)
			{
				BestNoPowerRunTime = newTime;
			}
		}

		public void AddCheaterRun(float newTime)
		{
			if (newTime < BestCheaterRunTime)
			{
				BestCheaterRunTime = newTime;
			}
		}
		#endregion

		public void Init()
		{
			foreach(ScriptableUpgrade upgrade in Upgrades)
			{
				upgrade.IsUsed = false;
			}

			EyesCount = 0;

			SpeedFactor = 1.0f;
			DetectionSpeed = 1.0f;
			NpcForgiveness = false;
			FreeClue = false;

			BestStandardRunTime = float.MaxValue;
			BestGhostRunTime = float.MaxValue;
			BestLuckyRunTime = float.MaxValue;
			BestNoPowerRunTime = float.MaxValue;
			BestCheaterRunTime = float.MaxValue;
		}
	}
}
