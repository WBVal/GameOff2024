using DG.Tweening;
using Gameplay.Level;
using Gameplay.Player;
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
        LevelGenerator levelGenerator;

		[SerializeField]
        Player player;
		public Player Player { get => player; }

		[SerializeField]
		Policeman policeman;
		public Policeman Policeman { get => policeman; set => policeman = value; }

		[SerializeField]
		RunTimer timer;


		float endTime;
        public float EndTime { get => endTime; set => endTime = value; }

		bool isPaused;

		private void Start()
		{
			timer.StartTimer();
		}

		public void Pause()
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
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

			policeman.StartChasing();
        }

        public void OnMissExecute()
        {
            Time.timeScale = 0;
            player.DisableInputs();
        }

        public void OnEndAreaEnter()
		{
			policeman.Stop();
			DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 1f);
            Player.DisableInputs();
			HudManager.Instance.HideTimer();
			endTime = timer.GetTimeRaw();
			HudManager.Instance.Message("mission time: " + timer.GetFormattedTime(endTime));
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
			HudManager.Instance.Message("You died");
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
