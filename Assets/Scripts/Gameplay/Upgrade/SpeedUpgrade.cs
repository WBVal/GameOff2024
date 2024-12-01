using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Upgrade
{
    [CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "ScriptableObjects/Upgrade/SpeedUpgrade")]
    public class SpeedUpgrade : ScriptableUpgrade
	{
		[SerializeField]
		float speedFactor;

		public override bool CheckCompatibility()
		{
			return PlayerStatsManager.Instance.PlayerStats.SpeedFactor == 1f;
		}

		public override void Apply()
		{
			base.Apply();
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.SpeedFactor = speedFactor;
			}
		}

		public override void Disable()
		{
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.SpeedFactor = 1f;
			}
		}
	}
}
