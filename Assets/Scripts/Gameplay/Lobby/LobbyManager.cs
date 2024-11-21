using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay.Lobby
{
    public class LobbyManager : Singleton<LobbyManager>
    {
        public void ExitLobby()
        {
            SceneFlowManager.Instance.LoadScene("SampleScene");
        }
    }
}
