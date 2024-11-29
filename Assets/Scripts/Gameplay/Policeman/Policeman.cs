using Gameplay.Player;
using Gameplay.Stealth;
using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Policeman : DetectionBehaviour
{
	[Header("Parameters")]
	[SerializeField]
	float attackRadius;
	[SerializeField]
	float patrolSpeed;
	[SerializeField]
	float patrolRotateSpeed;
	[SerializeField]
	float chaseSpeed;
	[SerializeField]
	float chaseMaxSpeed;
	[SerializeField]
	float chaseAccelerationDuration;

	[Header("Logic")]
	[SerializeField]
	LayerMask attackMask;
	[SerializeField]
	State startState = State.PATROL;

	Transform[] waypoints;
	public Transform[] Waypoints { get => waypoints; set => waypoints = value; }

	PolicemanAnimationController policemanAnimationController;

	public enum State
	{
		PATROL,
		CHASE,
		KILL
	}

	private State currentState;

	private int targetWaypointIndex;
	private Vector3 targetWaypoint;
	private Vector3 targetDir;
	private float targetAngle;

	private NavMeshAgent agent;

	protected override void Awake()
	{
		base.Awake();
		policemanAnimationController = GetComponent<PolicemanAnimationController>();
		agent = GetComponent<NavMeshAgent>();
		agent.enabled = false;
	}

	private void Start()
	{
		currentState = startState;
		targetWaypointIndex = 1;
	}

	void Update()
	{
		switch (currentState)
		{
			case State.PATROL:
				Patrol();
				break;
			case State.CHASE:
				Chase();
				break;
			case State.KILL:
				KillPlayer();
				break;
		}
	}

	protected override void DetectPlayer()
	{
		base.DetectPlayer();
		patrolSpeed = 0f;
		transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
		KillPlayer();
	}

	private void KillPlayer()
	{
		chaseSpeed = 0f;
		GameManager.Instance.PlayerKilled();
		policemanAnimationController.Kill();
	}

	public void Stop()
	{
		chaseSpeed = 0f;
	}

	public void OnKillAnimEnd()
	{
		GameManager.Instance.OnDeath();
	}

	private bool CheckForAttackRange()
	{
		if (!Physics.Linecast(transform.position, player.transform.position, attackMask) 
			&& Vector3.Distance(transform.position, player.transform.position) <= attackRadius)
		{
			return true;
		}
		return false;
	}

	#region Patrol
	private void Patrol()
	{
		policemanAnimationController.Patrol();
		if (waypoints == null) return;
		// Look for player
		CheckDetection();

		// Move along waypoints
		targetWaypoint = waypoints[targetWaypointIndex].position;
		PatrolRotate(targetWaypoint);
		transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, patrolSpeed * Time.deltaTime);
		if (transform.position == targetWaypoint)
		{
			targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
		}
	}
	private void PatrolRotate(Vector3 lookTarget)
	{
		targetDir = (lookTarget - transform.position).normalized;
		targetAngle = 90f - Mathf.Atan2(targetDir.z, targetDir.x) * Mathf.Rad2Deg;
		if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, patrolRotateSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
		}
	}
	#endregion

	#region Chase
	[ContextMenu("StartChase")]
	public void StartChasing()
	{
		agent.enabled = true;
		currentState = State.CHASE;
		StartCoroutine(ChaseAccelerationCoroutine());
	}
	private void Chase()
	{
		policemanAnimationController.Chase();
		agent.SetDestination(player.transform.position);
		agent.speed = chaseSpeed;
		if (CheckForAttackRange())
		{
			currentState = State.KILL;
		}
	}

	IEnumerator ChaseAccelerationCoroutine()
	{
		float elapsedTime = 0f;
		float startSpeed = chaseSpeed;
		while (elapsedTime < chaseAccelerationDuration)
		{
			chaseSpeed = Mathf.Lerp(startSpeed, chaseMaxSpeed, elapsedTime / chaseAccelerationDuration);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		chaseSpeed = chaseMaxSpeed;
	}

	#endregion

	private void OnDrawGizmos()
	{
		if (waypoints == null || waypoints.Length == 0) return;

		Gizmos.color = Color.magenta;
		Vector3 previousPos = waypoints[0].position;
		foreach (Transform t in waypoints)
		{
			Gizmos.DrawSphere(t.position, 0.3f);
			Gizmos.DrawLine(previousPos, t.position);
			previousPos = t.position;
		}
		Gizmos.DrawLine(previousPos, waypoints[0].position);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, attackRadius);
	}
}
