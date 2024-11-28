using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
	public class RunTimer : MonoBehaviour
	{
		private float elapsedTime = 0f; // Total elapsed time in seconds
		private bool isRunning = false; // Timer state

		HudManager hudManager;
		private void Awake()
		{
			hudManager = HudManager.Instance;
		}

		/// <summary>
		/// Starts or resumes the timer.
		/// </summary>
		public void StartTimer()
		{
			HudManager.Instance.ShowTimer();
			isRunning = true;
		}

		/// <summary>
		/// Pauses the timer.
		/// </summary>
		public void PauseTimer()
		{
			isRunning = false;
		}

		/// <summary>
		/// Stops the timer and resets elapsed time.
		/// </summary>
		public void StopTimer()
		{
			isRunning = false;
			HudManager.Instance.HideTimer();
			GameManager.Instance.EndTime = elapsedTime;
			elapsedTime = 0f;
		}

		public float GetTimeRaw()
		{
			return elapsedTime;
		}

		private void Update()
		{
			if (isRunning)
			{
				elapsedTime += Time.deltaTime;
				hudManager.SetTime(TimeUtils.GetFormattedTime(elapsedTime));
			}
		}
	}
}
