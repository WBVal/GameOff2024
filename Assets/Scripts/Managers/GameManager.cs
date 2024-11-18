using Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        Player player;
        public Player Player { get => player;}
    }
}
