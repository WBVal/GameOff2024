using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Upgrade
{
	[CreateAssetMenu(fileName = "NpcForgivenessUpgrade", menuName = "ScriptableObjects/Upgrade/NpcForgivenessUpgrade")]
	public class NpcForgivenessUpgrade : ScriptableUpgrade
	{
		public override void Apply()
		{
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.NpcForgiveness = true;
			}
		}

		public override void Disable()
		{
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.NpcForgiveness = false;
			}
		}
	}
}
