using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimationController : MonoBehaviour
{
	readonly int hashIdle = Animator.StringToHash("Idle");
	readonly int hashScared = Animator.StringToHash("Scared");
	readonly int hashDeath = Animator.StringToHash("Death");
	readonly int hashDeathBool = Animator.StringToHash("DeathBool");

	[SerializeField]
	Animator animator;

	private void Start()
	{
		Idle();
	}

	public void Idle()
	{
		animator.Play("Idle", 0, Random.Range(0f, 1f));
	}

	public void Scared()
	{
		animator.Play("Scared", 0, Random.Range(0f, 1f));
	}

	public void Death(bool isTarget)
	{
		animator.SetBool(hashDeathBool, isTarget);
		animator.SetTrigger(hashDeath);
	}
}
