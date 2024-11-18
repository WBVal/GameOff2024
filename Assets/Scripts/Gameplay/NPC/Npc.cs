using Gameplay.NPC;
using Gameplay.Stealth;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Npc : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField]
	float detectionSpeed;
	[SerializeField]
	float detectionCancelSpeed;

	[Header("Visuals")]
	[SerializeField]
	NpcDetectionUI detectionUI;
	[SerializeField]
	Light spotlight;

	[Header("Attributes")]
	[SerializeField]
    protected NpcAttribute[] attributes;
    public NpcAttribute[] Attributes { get=>attributes; set => attributes = value; }

    private bool isTarget;
    public bool IsTarget 
	{ 
		get=>isTarget;
		set
		{
			isTarget = value;
			if (value) name = "Target Npc";
		} 
	}

	private bool isMimic;
	public bool IsMimic 
	{ 
		get => isMimic;
		set
		{
			isMimic = value;
			if (value) name = "Mimic Npc";
		}
	}


	private SightDetection sightDetection;
	private HearDetection hearDetection;

	private Color baseLightColor;

	private bool detectingPlayer;
	private bool playerDetected;

	private float detectionGauge;

	private void Awake()
	{
		sightDetection = GetComponent<SightDetection>();
		hearDetection = GetComponent<HearDetection>();

		baseLightColor = spotlight.color;
		detectionGauge = 0f;
	}

	private void Update()
	{
		if (playerDetected) return;

		if (sightDetection.PlayerInSight() || hearDetection.PlayerHeard())
		{
			detectingPlayer = true;
		}
		else
		{
			detectingPlayer = false;
		}
		UpdateGauge();
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

	protected virtual void DetectPlayer()
	{
		playerDetected = true;
		spotlight.color = Color.red;
	}

	private void OnDrawGizmos()
	{
        if (isTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.2f);
		}
		if (isMimic)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.2f);
		}
	}
}
