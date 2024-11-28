using Gameplay.Player;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
    public class GatheringArea : SpawnArea
	{
		[SerializeField]
		ClueDisplay clueDisplay;
		[SerializeField]
		float clueLoadingTime;

		public enum ClueState
		{
			HIDDEN,
			LOCKED,
			LOADING,
			REVEALED
		}
        private Clue clue;
        public Clue Clue { get => clue; set => clue = value; }

		private Coroutine loadCoroutine;
		private float loadProgress;
		private ClueState currentState;

		ScriptablePlayerStats playerStats;

		private void Awake()
		{
			playerStats = PlayerStatsManager.Instance.PlayerStats;
			currentState = ClueState.HIDDEN;
			clueDisplay.HideClue();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (currentState != ClueState.LOCKED || playerStats.NpcForgiveness)
			{
				currentState = ClueState.LOADING;

				if (loadCoroutine != null)
				{
					StopCoroutine(loadCoroutine);
				}
				loadCoroutine = StartCoroutine(LoadClueCoroutine());
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (currentState == ClueState.LOCKED && !playerStats.NpcForgiveness) return;

			if (loadCoroutine != null)
			{
				StopCoroutine(loadCoroutine);
			}

			currentState = ClueState.HIDDEN;
			clueDisplay.HideClue();
		}

		private void OnClueLoaded()
		{
			PlayerStatsManager.Instance.IsLucky = false;
			currentState = ClueState.REVEALED;
			clueDisplay.DisplayClue(clue);
		}

		private IEnumerator LoadClueCoroutine()
		{
			loadProgress = 0.0f;
			float elapsedTime = 0f;
			while(elapsedTime < clueLoadingTime)
			{
				elapsedTime += Time.deltaTime;
				loadProgress = elapsedTime / clueLoadingTime;
				clueDisplay.LoadClue(loadProgress);
				yield return null;
			}
			OnClueLoaded();
			loadProgress = 1f;
		}

		public override void OnNpcDetection()
		{
			if(currentState == ClueState.LOCKED) return; // avoid multiple calls

			currentState = ClueState.LOCKED;
			if (loadCoroutine != null)
			{
				StopCoroutine(loadCoroutine);
			}
			clueDisplay.LockClue();
		}
	}
}
