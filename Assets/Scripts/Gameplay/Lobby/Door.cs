using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay.Lobby
{
	public class Door : MonoBehaviour, IFpsInteractable
	{
		public void OnInteraction()
		{
			LobbyManager.Instance.ExitLobby();
		}
	}
}
