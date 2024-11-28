using Gameplay.Player;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class ScoreBoard : MonoBehaviour
{
	[Header("Time Labels")]
	[SerializeField]
	TextMeshProUGUI standardRunLabel;
	[SerializeField]
	TextMeshProUGUI ghostRunLabel;
	[SerializeField]
	TextMeshProUGUI luckyRunLabel;
	[SerializeField]
	TextMeshProUGUI noPowerRunLabel;
	[SerializeField]
	TextMeshProUGUI cheaterRunLabel;

	private void Awake()
	{
		cheaterRunLabel.gameObject.SetActive(false);
		Init();
	}

	public void Init()
	{
		ScriptablePlayerStats stats = PlayerStatsManager.Instance.PlayerStats;

		standardRunLabel.text = stats.BestStandardRunTime == float.MaxValue ? "xx:xx:xxx" : TimeUtils.GetFormattedTime(stats.BestStandardRunTime);
		ghostRunLabel.text = stats.BestGhostRunTime == float.MaxValue ? "xx:xx:xxx" : TimeUtils.GetFormattedTime(stats.BestGhostRunTime);
		luckyRunLabel.text = stats.BestLuckyRunTime == float.MaxValue ? "xx:xx:xxx" : TimeUtils.GetFormattedTime(stats.BestLuckyRunTime);
		noPowerRunLabel.text = stats.BestNoPowerRunTime == float.MaxValue ? "xx:xx:xxx" : TimeUtils.GetFormattedTime(stats.BestNoPowerRunTime);
		if(stats.BestCheaterRunTime != float.MaxValue)
		{
			cheaterRunLabel.gameObject.SetActive(true);
			cheaterRunLabel.text = TimeUtils.GetFormattedTime(stats.BestCheaterRunTime);
		}
	}
}
