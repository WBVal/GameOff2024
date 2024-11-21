using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public void Resume()
        {
            GameManager.Instance.Pause();
        }

        public void Quit()
        {
            SceneFlowManager.Instance.LoadScene("LobbyScene");
		}
    }
}
