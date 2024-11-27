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
		[SerializeField]
		float coyoteTimeCounter;

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
		float climbingEndJump;

		[Header("Sliding")]
		[SerializeField]
		float slideDuration;
		[SerializeField]
		float slideStrength;

		[Header("Gound Check")]
		[SerializeField]
		float playerHeight;
		[SerializeField]
		float groundDrag;
		[SerializeField]
		LayerMask groundLayer;

		[Header("Slope Handling")]
		[SerializeField]
		float maxSlopeAngle;
		private RaycastHit slopeHit;

		private Vector2 inputDirection;
		public Vector2 InputDirection { get => inputDirection; set => inputDirection = value; }

		bool canMove;
		public bool CanMove { get => canMove; set => canMove = value; }

		bool jumpPressed;
		public bool JumpPressed { get => jumpPressed; set => jumpPressed = value; }

		[SerializeField]
		float coyoteTime;
		public float CoyoteTime { get => coyoteTime; set => coyoteTime = value; }

		Rigidbody rb;
		Vector3 moveDirection;

		float currentSpeed;

		bool isJumping;

		CapsuleCollider capCollider;
		bool tooSteep;
		bool exitingSlope;

		RaycastHit climbHit;
		Coroutine climbCoroutine;
		bool isClimbing;
		
		Coroutine slideCoroutine;

		PlayerAnimationController playerAnimationController;

		private void Awake()
		{
			canMove = true;
			rb = GetComponent<Rigidbody>();
			capCollider = GetComponent<CapsuleCollider>();
			playerAnimationController = GetComponent<PlayerAnimationController>();
			Physics.gravity = Vector3.down * gravity;
			startYScale = capCollider.height;
		}

		private void Update()
		{
			rb.drag = IsGrounded() ? groundDrag : 0f;

			if (!isJumping)
				coyoteTime = IsGrounded() ? coyoteTimeCounter : coyoteTime -= Time.deltaTime;

			SpeedControl();

			// Check for climbable ledges
			if (jumpPressed)
			{
				// Check for Climbing
				if (Physics.Raycast(climbOriginTransform.position, Vector3.down, out climbHit, climbRayLength, groundLayer))
				{
					Climb();
					playerAnimationController.Climb();
				}
			}
		}

		void FixedUpdate()
		{
			Move();
		}

		private void Move()
		{
			if (!canMove) return;

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
			else if (IsGrounded())
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

		public void GoDown()
		{
			capCollider.height = crouchYScale;
		}

		public void GetUp()
		{
			capCollider.height = startYScale;
		}

		private void OnDrawGizmos()
		{
			//if (!isClimbing) return;
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(climbOriginTransform.position, climbOriginTransform.position + Vector3.down * climbRayLength);

			Gizmos.DrawSphere(climbOriginTransform.position, 0.2f);
			Gizmos.DrawSphere(climbHit.point, 0.2f);
		}

		public void Jump()
		{
			exitingSlope = true;
			isJumping = true;
			coyoteTime = 0f;
			rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
			rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			Invoke("OnJumpEnd", jumpCooldown);
		}

		public void Climb()
		{
			if (isClimbing) return;

			canMove = false;

			isClimbing = true;
			if (climbCoroutine != null)
			{
				StopCoroutine(climbCoroutine);
			}
			climbCoroutine = StartCoroutine(ClimbCoroutine());
		}

		private IEnumerator ClimbCoroutine()
		{
			float elapsedTime = 0f;
			rb.useGravity = false;

			Vector3 startPos = transform.position;
			Vector3 targetPos = new Vector3(transform.position.x, climbHit.point.y + climbingEndJump, transform.position.z);

			while (elapsedTime < climbingTime)
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

		public bool IsGrounded()
		{
			return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.01f, groundLayer);
		}

		public bool CanGetUp()
		{
			return !Physics.Raycast(transform.position, Vector3.up, playerHeight + getUpRange);
		}

		public void Slide()
		{
			GoDown();
			if (slideCoroutine != null)
			{
				StopCoroutine(slideCoroutine);
			}
			slideCoroutine = StartCoroutine(SlideCoroutine());
		}

		private IEnumerator SlideCoroutine()
		{
			float elapsedTime = 0f;
			float diminishingStrength = slideStrength;
			while (elapsedTime < slideDuration)
			{
				rb.AddForce(moveDirection.normalized * diminishingStrength, ForceMode.VelocityChange);
				diminishingStrength = Mathf.Lerp(diminishingStrength, crouchSpeed, elapsedTime / slideDuration);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}

		private bool OnSlope()
		{
			if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.4f))
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
			isJumping = false;
		}

		public void EnableGravity(bool enable)
		{
			rb.useGravity = enable;
		}
	}
}
