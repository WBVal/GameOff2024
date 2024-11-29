using Gameplay.Player;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Gameplay.Upgrade
{
	public class UpgradeController : MonoBehaviour
	{
		[SerializeField]
		TextMeshProUGUI availableEyes;
		[SerializeField]
		Transform upgradeParent;
		[SerializeField]
		UpgradeUI upgradeUIPrefab;
		[SerializeField]
		GameObject[] usedEyesPlaceholders;

		ScriptableUpgrade[] upgrades;
		List<UpgradeUI> upgradeUIList;
		int currentUsedEyes;
		int maxUsedEyes;

		private ScriptablePlayerStats playerStats;
		private void Awake()
		{
			currentUsedEyes = 0;
			maxUsedEyes = usedEyesPlaceholders.Length;

			upgradeUIList = new List<UpgradeUI>();
			upgrades = PlayerStatsManager.Instance.PlayerStats.Upgrades;
			playerStats = PlayerStatsManager.Instance.PlayerStats;

			foreach(ScriptableUpgrade upgrade in upgrades)
			{
				UpgradeUI upgradeUI = Instantiate(upgradeUIPrefab, upgradeParent);

				upgradeUI.Description = upgrade.Description;
				upgradeUI.Cost = upgrade.Cost.ToString();
				upgradeUI.IsSelected = upgrade.IsUsed;
				upgradeUI.IsAvailable = playerStats.EyesCount >= upgrade.Cost;

				if (upgradeUI.IsSelected)
				{
					upgradeUI.Title = upgrade.Title + " (equipped)";
					upgradeUI.IsAvailable = false;
					currentUsedEyes += upgrade.Cost;
				}
				else
				{
					upgradeUI.Title = upgrade.Title;
					upgradeUI.IsAvailable = true;
				}

				UpgradeUI tempUI = upgradeUI;
				ScriptableUpgrade tempUpgrade = upgrade;

				upgradeUI.OnClick += (() =>
				{
					tempUI.IsSelected = !tempUI.IsSelected;
					if (tempUI.IsSelected)
					{
						playerStats.EyesCount -= tempUpgrade.Cost;
						currentUsedEyes += tempUpgrade.Cost;
						upgradeUI.Title = upgrade.Title + " (equipped)";
						tempUpgrade.IsUsed = true;
						tempUpgrade.Apply();
					}
					else
					{
						playerStats.EyesCount += tempUpgrade.Cost;
						currentUsedEyes -= tempUpgrade.Cost;
						upgradeUI.Title = upgrade.Title;
						tempUpgrade.IsUsed = false;
						tempUpgrade.Disable();
					}
					UpdateUpgradeListDisplay();
					UpdateUsedEyes();
				});
				tempUI.CompatibilityCheck += (() =>
				{
					return tempUpgrade.CheckCompatibility();
				});
				upgradeUIList.Add(upgradeUI);
			}
			UpdateUpgradeListDisplay();
			UpdateUsedEyes();
		}

		public void UpdateUpgradeListDisplay()
		{
			availableEyes.text = playerStats.EyesCount.ToString();
			foreach(UpgradeUI upgradeUI in upgradeUIList)
			{
				upgradeUI.IsAvailable = (Int32.Parse(upgradeUI.Cost) <= Math.Min(playerStats.EyesCount, maxUsedEyes - currentUsedEyes)) && (upgradeUI.CompatibilityCheck());
				upgradeUI.UpdateDisplay();
			}
		}

		private void UpdateUsedEyes()
		{
			foreach(GameObject eyes in usedEyesPlaceholders)
			{
				eyes.SetActive(false);
			}
			for(int i=0; i<currentUsedEyes; i++)
			{
				usedEyesPlaceholders[i].SetActive(true);
			}
		}
	}
}
