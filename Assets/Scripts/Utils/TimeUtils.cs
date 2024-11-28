using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class TimeUtils
	{

		/// <summary>
		/// Gets the formatted time as minutes:seconds:milliseconds.
		/// </summary>
		/// <returns>Formatted string (e.g., "02:15:125")</returns>
		public static string GetFormattedTime(float time)
		{
			int minutes = Mathf.FloorToInt(time / 60); // Calculate minutes
			int seconds = Mathf.FloorToInt(time % 60); // Calculate seconds
			int milliseconds = Mathf.FloorToInt((time * 1000) % 1000); // Calculate milliseconds

			// Format as mm:ss:ms (e.g., 02:15:125)
			return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
		}
	}
}
