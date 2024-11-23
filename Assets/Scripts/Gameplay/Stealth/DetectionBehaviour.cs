using Gameplay.Player;
using Gameplay.Stealth;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBehaviour : MonoBehaviour
{
	[Header("Detection")]
	[SerializeField]
	protected float detectionSpeed;
	[SerializeField]
	protected float detectionCancelSpeed;
	[SerializeField]
	protected DetectionUI detectionUI;
	[SerializeField]
	protected Light spotlight;

	private SightDetection sightDetection;
	private HearDetection hearDetection;

	protected bool detectingPlayer;
	protected bool playerDetected;

	protected float detectionGauge;

	protected Player player;

	private Color baseLightColor;

	private float baseDetectionSpeed;

	protected bool isSeen;
	protected bool isHeard;

	protected virtual void Awake()
	{
		sightDetection = GetComponent<SightDetection>();
		hearDetection = GetComponent<HearDetection>();

		baseLightColor = spotlight.color;
		detectionGauge = 0f;

		player = GameManager.Instance.Player;

		baseDetectionSpeed = detectionSpeed;
	}

	protected void CheckDetection()
	{
		if (playerDetected || !player.CanBeDetected) return;
		isSeen = sightDetection.PlayerInSight();
		isHeard = hearDetection.PlayerHeard();
		if (isSeen || isHeard)
		{
			detectingPlayer = true;

			// Detects player quicker if in sight
			if(isSeen) 
			{ 
				detectionSpeed = baseDetectionSpeed * 2f; 
			}
			else
			{
				detectionSpeed = baseDetectionSpeed;
			}
		}
		else
		{
			detectingPlayer = false;
		}
		UpdateGauge();
	}

	protected virtual void DetectPlayer()
	{
		if (!player.CanBeDetected) return;

		playerDetected = true;
		spotlight.color = Color.red;
	}

	private void UpdateGauge()
	{
		if (detectingPlayer)
		{
			detectionGauge += Time.deltaTime * detectionSpeed;
		}
		else
		{
			detectionGauge -= Time.deltaTime * detectionCancelSpeed;
		}

		if (detectionGauge >= 1f)
			DetectPlayer();
		detectionGauge = Mathf.Clamp(detectionGauge, 0f, 1f);
		detectionUI.SetDetection(detectionGauge);
	}
}