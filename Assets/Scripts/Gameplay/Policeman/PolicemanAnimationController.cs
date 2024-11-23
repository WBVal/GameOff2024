using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicemanAnimationController : MonoBehaviour
{
	[SerializeField]
	Animator animator;

	readonly int hashPatrol = Animator.StringToHash("Patrol");
	readonly int hashChase = Animator.StringToHash("Chase");
	readonly int hashKill = Animator.StringToHash("Kill");

	int currentHash;

	public void Patrol()
	{
		SetUniqueTrigger(hashPatrol);
	}
	public void Chase()
	{
		SetUniqueTrigger(hashChase);
	}
	public void Kill()
	{
		animator.SetTrigger(hashKill);
	}

	private void SetUniqueTrigger(int hash)
	{
		if (currentHash != 0)
			animator.ResetTrigger(currentHash);

		currentHash = hash;
		animator.SetTrigger(hash);
	}
}
