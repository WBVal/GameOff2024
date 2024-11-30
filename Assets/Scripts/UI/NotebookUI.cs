using Gameplay.Cheat;
using Gameplay.Player;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class NotebookUI : MonoBehaviour
	{
		[SerializeField]
		TMP_InputField inputField;

		ScriptablePlayerStats stats;

		private void Awake()
		{
			stats = PlayerStatsManager.Instance.PlayerStats;
		}

		private void OnEnable()
		{
			GameManager.Instance.CurrentTimeScale = Time.timeScale;
			Time.timeScale = 0f;
			inputField.onEndEdit.AddListener(CheckWords);
		}
		private void OnDisable()
		{
			inputField.onEndEdit.RemoveListener(CheckWords);
		}

		private void CheckWords(string content)
		{
			foreach(ScriptableCheatCode cheat in stats.Cheats)
			{
				if (StringUtility.ContainsWord(content, cheat.Code))
				{
					cheat.Apply();
				}
			}
		}
	}
}
