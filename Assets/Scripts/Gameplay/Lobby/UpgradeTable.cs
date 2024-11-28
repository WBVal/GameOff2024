using Gameplay.Upgrade;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay.Lobby
{
	public class UpgradeTable : MonoBehaviour, IFpsInteractable
    {
		[SerializeField]
		UpgradeController upgradeController;

		public void OnInteraction()
		{
			upgradeController.gameObject.SetActive(true);
			HudManager.Instance.CurrentWindow = upgradeController.gameObject;
			GameManager.Instance.CurrentTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}

		public void ShowHelper()
		{
			HudManager.Instance.ShowHelper("Manage eye powers");
		}
	}
}
