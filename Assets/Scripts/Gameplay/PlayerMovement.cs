using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay.Player
{
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Parameters")]
		[SerializeField]
		float gravity;
		[SerializeField]
		float jumpForce;
		[SerializeField]
		float walkSpeed;
		[SerializeField]
		float runSpeed;
		[SerializeField]
		float airMultiplier;
		[SerializeField]
		float jumpCooldown;

		[Header("Crouch")]
		[SerializeField]
		float crouchSpeed;
		[SerializeField]
		float crouchYScale;
		[SerializeField]
		float startYScale;
		[SerializeField]
		float getUpRange;

		[Header("Climbing")]
		[SerializeField]
		Transform climbOriginTransform;
		[SerializeField]
		float climbRayLength;
		[SerializeField]
		float climbingTime;
		[SerializeField]
		AnimationCurve climbingCurve;

		[Header("Gound Check")]
		[SerializeField]
		float playerHeight;
		[SerializeField]
		float groundDrag;
		[SerializeField]
		LayerMask groundLayer;
		Rigidbody rb;
		Vector3 moveDirection;

		[Header("Slope Handling")]
		[SerializeField]
		float maxSlopeAngle;
		private RaycastHit slopeHit;

		private Vector2 inputDirection;
		public Vector2 InputDirection { get => inputDirection; set => inputDirection = value; }

		bool canMove;
		public bool CanMove { get => canMove; set => canMove = value; }

		float currentSpeed;

		CapsuleCollider capCollider;
		bool tooSteep;
		bool exitingSlope;

		RaycastHit climbHit;
		Coroutine climbCoroutine;
		bool isClimbing;

		private void Awake()
		{
			canMove = true;
			rb = GetComponent<Rigidbody>();
			capCollider = GetComponent<CapsuleCollider>();
			Physics.gravity = Vector3.down * gravity;
			startYScale = capCollider.height;
		}

		private void Update()
		{
			rb.drag = IsGrounded() ? groundDrag : 0f;
			SpeedControl();
		}

		void FixedUpdate()
		{
			Move();
		}

		private void Move()
		{
			if(!canMove) return;

			moveDirection = transform.forward * inputDirection.y + transform.right * inputDirection.x;

			if (tooSteep)
			{
				moveDirection = Vector2.zero;
			}

			if (OnSlope() && !exitingSlope)
			{
				rb.AddForce(GetSlopeMoveDirection() * currentSpeed, ForceMode.VelocityChange);

				if (rb.velocity.y > 0f)
					rb.AddForce(Vector3.down * 3000f, ForceMode.Force);
			}
			else if(IsGrounded())
				rb.AddForce(moveDirection.normalized * currentSpeed, ForceMode.VelocityChange);
			else
				rb.AddForce(moveDirection.normalized * airMultiplier, ForceMode.VelocityChange);

			rb.useGravity = !OnSlope();
		}

		public void Stop()
		{
			currentSpeed = 0f;
		}

		public void Walk()
		{
			currentSpeed = walkSpeed;
		}

		public void Run()
		{
			currentSpeed = runSpeed;
		}

		public void Crouch()
		{
			currentSpeed = crouchSpeed;
		}

		public void GetDown()
		{
			capCollider.height = crouchYScale;
		}

		public void GetUp()
		{
			capCollider.height = startYScale;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(climbOriginTransform.position, climbOriginTransform.position + Vector3.down * climbRayLength);

			Gizmos.DrawSphere(climbOriginTransform.position, 0.2f);
			Gizmos.DrawSphere(climbHit.point, 0.2f);
		}

		public void Jump()
		{
			// Check for Climbing
			if(Physics.Raycast(climbOriginTransform.position, Vector3.down, out climbHit, climbRayLength, groundLayer))
			{
				Climb();
			}

			//Normal Jump
			exitingSlope = true;
			rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			Invoke("OnJumpEnd", jumpCooldown);
		}

		public void Climb()
		{
			canMove = false;

			if (isClimbing) return;

			isClimbing = true;
			if(climbCoroutine != null)
			{
				StopCoroutine(climbCoroutine);
			}
			climbCoroutine = StartCoroutine(ClimbCoroutine());
		}

		public bool IsGrounded()
		{
			return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
		}

		public bool CanGetUp()
		{
			return !Physics.Raycast(transform.position, Vector3.up, playerHeight + getUpRange);
		}

		private bool OnSlope()
		{
			if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.4f))
			{
				float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
				tooSteep = angle >= maxSlopeAngle;
				return angle < maxSlopeAngle && angle != 0f;
			}
			return false;
		}

		private Vector3 GetSlopeMoveDirection()
		{
			return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
		}

		private void SpeedControl()
		{
			if (OnSlope() && !exitingSlope)
			{
				if (rb.velocity.magnitude > currentSpeed)
					rb.velocity = rb.velocity.normalized * currentSpeed;
				return;
			}
			Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

			if (flatVel.magnitude > currentSpeed)
			{
				Vector3 limitedVel = flatVel.normalized * currentSpeed;
				rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
			}
		}

		private void OnJumpEnd()
		{
			exitingSlope = false;
		}

		private IEnumerator ClimbCoroutine()
		{
			float elapsedTime = 0f;
			rb.useGravity = false;

			Vector3 startPos = transform.position;
			Vector3 targetPos = new Vector3(transform.position.x, climbHit.point.y, transform.position.z);

			while(elapsedTime < climbingTime)
			{
				elapsedTime += Time.deltaTime;
				transform.position = Vector3.Slerp(startPos, targetPos, elapsedTime / climbingTime);
				yield return null;
			}

			transform.position = targetPos;
			canMove = true;
			rb.useGravity = true;
			isClimbing = false;
		}
	}
}
