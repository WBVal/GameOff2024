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

		[Header("Cheat")]
		[SerializeField]
		bool cheatEnabled;
		[SerializeField]
		bool seeTarget;
		[SerializeField]
		bool canBeDetected;
		[SerializeField]
		bool noPolice;


		private void Awake()
		{
			playerStats.Init();

			DontDestroyOnLoad(this);
		}

		private void Start()
		{
			if (cheatEnabled)
			{
				GameManager.Instance.Target.ShowIndicator = seeTarget;
				GameManager.Instance.Player.CanBeDetected = canBeDetected;
			}
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

		public void InitCheats()
		{
			seeTarget = false;
			noPolice = false;
			canBeDetected = true;
			GameManager.Instance.Target.ShowIndicator = seeTarget;
			GameManager.Instance.Player.CanBeDetected = canBeDetected;
		}

		public void CheatDetection()
		{
			isCheating = true;
			GameManager.Instance.Player.CanBeDetected = false;
			HudManager.Instance.Message("Cheat Code: you cannot be detected");
		}
		public void CheatSeeTarget()
		{
			isCheating = true;
			GameManager.Instance.Target.ShowIndicator = true;
			HudManager.Instance.Message("Cheat Code: Eye bearer is visible");
		}

		[ContextMenu("NoPolice")]
		public void CheatNoPolice()
		{
			if (noPolice) return;
			isCheating = true;
			noPolice = true;
			Destroy(GameManager.Instance.Policeman.gameObject);
			HudManager.Instance.Message("Cheat Code: the policeman is gone");
		}
	}
}
