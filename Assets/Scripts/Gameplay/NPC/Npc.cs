using Cinemachine;
using Gameplay.NPC;
using Gameplay.Player;
using Gameplay.Stealth;
using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class Npc : MonoBehaviour, IFpsInteractable
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
	[SerializeField]
	CinemachineVirtualCamera executeCam;

	[Header("Attributes")]
	[SerializeField]
    protected NpcAttribute[] attributes;
    public NpcAttribute[] Attributes 
	{ 
		get=>attributes;
		set
		{
			attributes = value;
			GetComponent<NpcAttributesDisplay>().InitAttributes(attributes);
		}
	}

	private Action onPlayerDetected;
	public Action OnPlayerDetected { get=>onPlayerDetected; set => onPlayerDetected = value; }

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

	private Player player;

	private NpcAnimationController animationController;

	private bool isDead;

	private void Awake()
	{
		executeCam.gameObject.SetActive(false);
		sightDetection = GetComponent<SightDetection>();
		hearDetection = GetComponent<HearDetection>();
		animationController = GetComponent<NpcAnimationController>();

		baseLightColor = spotlight.color;
		detectionGauge = 0f;

		player = GameManager.Instance.Player;
	}

	private void Update()
	{
		if (playerDetected || !player.CanBeDetected) return;

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
		if (!player.CanBeDetected) return;

		playerDetected = true;
		spotlight.color = Color.red;
		onPlayerDetected?.Invoke();
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

	public void OnInteraction()
	{
		GameManager.Instance.Player.Execute(this);
		executeCam.gameObject.SetActive(true);
		OnExecute();
	}

	public void OnExecute()
	{
		isDead = true;
		animationController.Death(isTarget);
	}

	public void OnExecuteEnd()
	{
		if (isTarget)
		{
			// Eye collected -> go back to base
			GameManager.Instance.OnExecute();
			executeCam.gameObject.SetActive(false);
		}
	}

	public void OnMissExecuteEnd()
	{
		// Wrong victim, game over
		executeCam.gameObject.SetActive(false);
		GameManager.Instance.OnMissExecute();
	}

	public void Scare()
	{
		if(!isDead)
			animationController.Scared();
	}
}
