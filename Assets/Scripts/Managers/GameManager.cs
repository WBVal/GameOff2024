using Audio;
using DG.Tweening;
using Gameplay.Level;
using Gameplay.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UI;
using UnityEngine;
using Utils;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
	{
		[SerializeField]
		bool isLobby;

		[SerializeField]
        LevelGenerator levelGenerator;

		[SerializeField]
        Player player;
		public Player Player { get => player; }

		[SerializeField]
		Policeman policeman;
		public Policeman Policeman { get => policeman; set => policeman = value; }

		[SerializeField]
		RunTimer timer;

		Npc target;
		public Npc Target { get => target; set => target = value; }

		float currentTimeScale;
		public float CurrentTimeScale { get=>currentTimeScale; set => currentTimeScale = value; }

		float endTime;
        public float EndTime { get => endTime; set => endTime = value; }

		bool isPaused;

		private void Start()
		{
			if (isLobby) return;
			
			PlayerStatsManager.Instance.InitCheats();
			timer.StartTimer();
		}

		public void Pause()
        {
			if(!isPaused)
				currentTimeScale = Time.timeScale;

			isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : currentTimeScale;
            HudManager.Instance.Pause(isPaused);
			Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
			Cursor.visible = !isPaused;
		}

        public void OnExecute()
        {
            player.OnExecuteEnd();
            // Eye + 1
            // Policeman starts searching
            // Every npc is scared
            foreach(Transform t in levelGenerator.NpcSpawnParent)
            {
                t.GetComponent<Npc>().Scare();
            }
			levelGenerator.DisableClues();

			if (policeman == null) return;

			HudManager.Instance.Message("The policeman knows where you are.");
			policeman.StartChasing();
        }

        public void OnMissExecute()
        {
            Time.timeScale = 0;
            player.DisableInputs();
			HudManager.Instance.OnMissExecute();
        }

        public void OnEndAreaEnter()
		{
			policeman?.Stop();
			DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 1f);
            Player.DisableInputs();
			HudManager.Instance.HideTimer();
			endTime = timer.GetTimeRaw();
			PlayerStatsManager.Instance.PublishTime(endTime);
			HudManager.Instance.OnVictory();
			PlayerStatsManager.Instance.AddEye();

			StartCoroutine(EndCoroutine(() => { HudManager.Instance.OnVictory(); }));
		}

		public void PlayerKilled()
		{
			player.Killed();
		}

		public void OnDeath()
		{
			DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 1f);
			HudManager.Instance.HideTimer();
			endTime = timer.GetTimeRaw();

			StartCoroutine(EndCoroutine(() => { HudManager.Instance.OnDeath(); }));
		}

		[ContextMenu("GoToLobby")]
		public void GoToLobby()
		{
			SceneFlowManager.Instance.LoadScene("LobbyScene");
		}

		IEnumerator EndCoroutine(Action callback)
		{
			float elapsedTime = 0f;
			while(elapsedTime < 1f)
			{
				elapsedTime += Time.unscaledDeltaTime;

				yield return null;
			}
			callback?.Invoke();
			AudioManager.Instance.StopMusic();
		}

		#region Timer

		public void StartTimer()
		{
			timer.gameObject.SetActive(true);
		}

		public void PauseTimer()
		{
			timer.PauseTimer();
		}

		public void StopTimer()
		{
			timer.StopTimer();
		}
		#endregion
	}
}
