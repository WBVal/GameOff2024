using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Managers
{
	public class HudManager : Singleton<HudManager>
	{
		[SerializeField]
		PauseMenu pauseMenu;

		[SerializeField]
		Image crosshair;

		[SerializeField]
		TextMeshProUGUI timerText;

		[SerializeField]
		Message messageContainer;

		[SerializeField]
		HudDetectionUI hudDetection;

		[SerializeField]
		EndScreen endScreen;

		[SerializeField]
		InteractionHelper interactionHelper;

		private GameObject currentWindow;
		public GameObject CurrentWindow 
		{ 
			get { return currentWindow; } 
			set 
			{ 
				currentWindow = value;
				Cursor.visible = value != null;
				Cursor.lockState = value != null ? CursorLockMode.Confined : CursorLockMode.Locked;
			} 
		}

		private void Awake()
		{
			endScreen.gameObject.SetActive(false);
			interactionHelper.gameObject.SetActive(false);
		}

		public void CrossHairInteract(bool canInteract)
		{
			crosshair.color = canInteract ? Color.red : Color.white;
		}

		public void Pause(bool value)
		{
			pauseMenu.gameObject.SetActive(value);
		}

		public void QuitCurrentWindow()
		{
			currentWindow.SetActive(false);
			currentWindow = null;
			Time.timeScale = GameManager.Instance.CurrentTimeScale;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		public void SetTime(string value)
		{
			timerText.text = value;
		}

		public void ShowTimer()
		{
			timerText.gameObject.SetActive(true);
		}

		public void HideTimer()
		{
			timerText.gameObject.SetActive(false);
		}

		public void Message(string content)
		{
			messageContainer.DisplayMessage(content, 3f);
		}

		public void SetSightDetection(float value)
		{
			hudDetection.SetNormalizedSightDetection(value);
		}

		public void SetNoiseDetection(float value)
		{
			hudDetection.SetHearingDetectionLevel(value);
		}

		public void OnDeath()
		{
			endScreen.gameObject.SetActive(true);
			endScreen.OnDeath();
		}

		public void OnVictory()
		{
			endScreen.gameObject.SetActive(true);
			endScreen.OnVictory();
		}

		public void OnMissExecute()
		{
			endScreen.gameObject.SetActive(true);
			endScreen.OnMissExecute();
		}

		public void ShowHelper(string helperText)
		{
			if (string.IsNullOrEmpty(helperText))
			{
				interactionHelper.gameObject.SetActive(false);
				return;
			}

			interactionHelper.gameObject.SetActive(true);
			interactionHelper.Helper = helperText;
		}
	}
}
