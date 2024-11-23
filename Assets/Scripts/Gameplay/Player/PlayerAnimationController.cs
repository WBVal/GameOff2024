using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Player
{
	public class PlayerAnimationController : MonoBehaviour
	{
		#region Animations
		readonly int hashIdle = Animator.StringToHash("Idle");
		readonly int hashWalk = Animator.StringToHash("Walk");
		readonly int hashRun = Animator.StringToHash("Run");
		readonly int hashCrouch = Animator.StringToHash("Crouch");
		readonly int hashJump = Animator.StringToHash("Jump");
		readonly int hashSlide = Animator.StringToHash("Slide");
		readonly int hashClimb = Animator.StringToHash("Climb");
		readonly int hashKilled = Animator.StringToHash("Killed");
		readonly int hashVelocity = Animator.StringToHash("Velocity");
		readonly int hashExecute = Animator.StringToHash("Execute");
		readonly int hashExecuteBool = Animator.StringToHash("ExecuteSuccess");
		#endregion

		#region Variables Editables
		[SerializeField]
		Animator animator;
		#endregion

		float velocity;
		public float Velocity
		{
			get => velocity;
			set
			{
				velocity = value;
				animator.SetFloat(hashVelocity, velocity);
			}
		}

		int currentHash;

		#region PublicsMethods

		public void Idle()
		{
			SetUniqueTrigger(hashIdle);
		}
		public void Walk()
		{
			SetUniqueTrigger(hashWalk);
		}
		public void Run()
		{
			SetUniqueTrigger(hashRun);
		}
		public void Crouch()
		{
			SetUniqueTrigger(hashCrouch);
		}
		public void Jump()
		{
			animator.SetTrigger(hashJump);
		}
		public void Slide()
		{
			animator.SetTrigger(hashSlide);
		}
		public void Climb()
		{
			animator.SetTrigger(hashClimb);
		}
		public void Killed()
		{
			animator.SetTrigger(hashKilled);
		}
		public void Execute(bool success)
		{
			animator.SetBool(hashExecuteBool, success);
			animator.SetTrigger(hashExecute);
		}
		#endregion

		private void SetUniqueTrigger(int hash)
		{
			if (currentHash != 0)
				animator.ResetTrigger(currentHash);

			currentHash = hash;
			animator.SetTrigger(hash);
		}
	}
}