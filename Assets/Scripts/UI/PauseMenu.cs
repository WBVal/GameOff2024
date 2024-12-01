using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        Slider sensSlider;

		private void Awake()
		{
            sensSlider.value = SceneFlowManager.Instance.Sensitivity / 10f;
            sensSlider.onValueChanged.AddListener(SetSens);
		}
		public void Resume()
        {
            GameManager.Instance.Pause();
        }

        public void Quit()
        {
            SceneFlowManager.Instance.LoadScene("LobbyScene");
		}

        public void SetSens(float sens)
        {
            SceneFlowManager.Instance.Sensitivity = sens * 10f;
        }
    }
}
