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

public class Npc : DetectionBehaviour, IFpsInteractable
{
	[Header("Visuals")]
	[SerializeField]
	CinemachineVirtualCamera executeCam;

	[Header("Attributes")]
	[SerializeField]
	protected NpcAttribute[] attributes;
	public NpcAttribute[] Attributes
	{
		get => attributes;
		set
		{
			attributes = value;
			GetComponent<NpcAttributesDisplay>().InitAttributes(attributes);
		}
	}
	[Header("Cheat")]
	[SerializeField]
	GameObject targetIndicator;

	private Action onPlayerDetected;
	public Action OnPlayerDetected { get => onPlayerDetected; set => onPlayerDetected = value; }

	public bool ShowIndicator
	{
		get { return targetIndicator.activeSelf; }
		set { targetIndicator.SetActive(value); }
	}

	private bool isTarget;
	public bool IsTarget
	{
		get => isTarget;
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

	private NpcAnimationController animationController;

	private bool isDead;

	protected override void Awake()
	{
		base.Awake();
		executeCam.gameObject.SetActive(false);
		animationController = GetComponent<NpcAnimationController>();
	}

	private void Update()
	{
		CheckDetection();

		// Send detection level if in range of player
		if (Vector3.Distance(transform.position, player.transform.position) <= DetectionRange)
		{
			if (DetectionGauge >= player.DetectionLevel)
				player.DetectionLevel = DetectionGauge;
		}
		else if(playerDetected && stats.NpcForgiveness)
		{
			playerDetected = false;
		}
	}

	protected override void DetectPlayer()
	{
		base.DetectPlayer();
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
		player.Execute(this);
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
		GetComponent<Collider>().enabled = false;
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
		GetComponent<Collider>().enabled = false;
		if (!isDead)
			animationController.Scared();
	}

	public void ShowHelper()
	{
		HudManager.Instance.ShowHelper("Kill and steal eye");
	}
}
