using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay.Lobby {
    public class TutorialEntrance : MonoBehaviour, IFpsInteractable
    {
		[SerializeField]
		string tutoSceneName;

		public void OnInteraction()
		{
			SceneFlowManager.Instance.LoadScene(tutoSceneName);
		}

		public void ShowHelper()
		{
			HudManager.Instance.ShowHelper("Go to tutorial");
		}
    }
}
