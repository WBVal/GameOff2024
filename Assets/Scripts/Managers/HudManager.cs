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

		public void CrossHairInteract(bool canInteract)
        {
            crosshair.color = canInteract ? Color.red : Color.white;
        }

        public void Pause(bool value)
        {
            pauseMenu.gameObject.SetActive(value);
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
    }
}
