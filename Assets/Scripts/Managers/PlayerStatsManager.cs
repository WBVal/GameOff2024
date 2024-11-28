using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Player;
using Utils;

namespace Managers
{
    public class PlayerStatsManager : Singleton<PlayerStatsManager>
    {
        [SerializeField]
        ScriptablePlayerStats playerStats;
		public ScriptablePlayerStats PlayerStats { get { return playerStats; } }

		[Header("Speedrun Categories")]
		[SerializeField]
		bool isGhost = true;
		public bool IsGhost { get { return isGhost; } set { isGhost = value; } }
		[SerializeField]
		bool isLucky = true;
		public bool IsLucky { get {  return isLucky; } set {  isLucky = value; } }
		[SerializeField]
		bool isNoPower = true;
		public bool IsNoPower { get { return isNoPower; } set { isNoPower = value; } }
		[SerializeField]
		bool isCheating = false;
		public bool IsCheating { get { return isCheating; } set { isCheating = value; } }



		private void Awake()
		{
			playerStats.Init();

			DontDestroyOnLoad(this);
		}

		[ContextMenu("AddEye")]
		public void AddEye()
		{
			playerStats.EyesCount++;
		}

		public void PublishTime(float time)
		{
			if (isCheating)
			{
				playerStats.AddToLuckyRun(time);
				isLucky = true;
				isGhost = true;
				isNoPower = true;
				isCheating = false;
				return;
			}

			playerStats.AddToStandardRun(time);

			if (isLucky)
			{
				playerStats.AddToLuckyRun(time);
				isLucky = true;
				isGhost = true;
				isNoPower = true;
				isCheating = false;
				return;
			}

			if (isGhost) playerStats.AddToGhostRun(time);

			if (isNoPower) playerStats.AddNoPowerRun(time);

			isLucky = true;
			isGhost = true;
			isNoPower = true;
			isCheating = false;
		}
	}
}
