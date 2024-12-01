using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Upgrade
{
	[CreateAssetMenu(fileName = "FreeClueUpgrade", menuName = "ScriptableObjects/Upgrade/FreeClueUpgrade")]
	public class FreeClueUpgrade : ScriptableUpgrade
	{
		public override void Apply()
		{
			base.Apply();
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.FreeClue = true;
			}
		}

		public override void Disable()
		{
			if (PlayerStatsManager.Instance != null)
			{
				PlayerStatsManager.Instance.PlayerStats.FreeClue = false;
			}
		}
	}
}
