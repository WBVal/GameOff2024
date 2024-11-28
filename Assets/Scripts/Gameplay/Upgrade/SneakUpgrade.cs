using Gameplay.Upgrade;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Upgrade
{
	[CreateAssetMenu(fileName = "SneakUpgrade", menuName = "ScriptableObjects/Upgrade/SneakUpgrade")]
	public class SneakUpgrade : ScriptableUpgrade
	{
		[SerializeField]
		float detectionFactor;

		public override void Apply()
		{
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.DetectionSpeed = detectionFactor;
			}
		}

		public override void Disable()
		{
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.DetectionSpeed = 1f;
			}
		}
	}
}
