using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

		/// <summary>
		/// Gets the formatted time as minutes:seconds:milliseconds.
		/// </summary>
		/// <returns>Formatted string (e.g., "02:15:125")</returns>
		public string GetFormattedTime(float time)
		{
			int minutes = Mathf.FloorToInt(time / 60); // Calculate minutes
			int seconds = Mathf.FloorToInt(time % 60); // Calculate seconds
			int milliseconds = Mathf.FloorToInt((time * 1000) % 1000); // Calculate milliseconds

			// Format as mm:ss:ms (e.g., 02:15:125)
			return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
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
				hudManager.SetTime(GetFormattedTime(elapsedTime));
			}
		}
	}
}
