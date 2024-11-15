using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
	public class PlayerController : MonoBehaviour
	{
		public enum State
		{
			IDLE,
			WALK,
			RUN,
			CROUCH,
			DEAD,
			EXECUTE
		}

		PlayerInputs inputs;
		public PlayerInputs Inputs { get => inputs; set => inputs = value; }

		State currentState = State.IDLE;
		public State CurrentState { get => currentState; set => currentState = value; }

		[SerializeField]
		bool isRunning;
		[SerializeField]
		bool isCrouched;

		Vector2 moveDir;

		PlayerMovement playerMovement;
		PlayerCamera playerCamera;
		PlayerAnimationController playerAnimationController;

		private void Awake()
		{
			inputs = new PlayerInputs();
			playerMovement = GetComponent<PlayerMovement>();
			playerCamera = GetComponent<PlayerCamera>();
			playerAnimationController = GetComponent<PlayerAnimationController>();
		}

		private void OnEnable()
		{
			inputs.Player.Camera.performed += OnCamera;
			inputs.Player.Camera.canceled += OnCameraStop;
			inputs.Player.Peak.performed += OnPeak;
			inputs.Player.Peak.canceled += OnStopPeak;

			inputs.Player.Movement.performed += OnMove;
			inputs.Player.Movement.canceled += OnStopMove;
			inputs.Player.Jump.performed += OnJump;
			inputs.Player.Jump.canceled += OnJumpReleased;
			inputs.Player.Run.performed += OnRun;
			inputs.Player.Run.canceled += OnStopRun;
			inputs.Player.Crouch.started += OnCrouch;

			inputs.Player.Enable();
		}

		void Update()
		{
			UpdateState();

			switch (currentState)
			{
				case State.IDLE:
					Idle();
					break;
				case State.WALK:
					Walk();
					break;
				case State.RUN:
					Run();
					break;
				case State.CROUCH:
					Crouch();
					break;
				case State.DEAD:
					break;
				case State.EXECUTE:
					break;
			}
		}

		#region Inputs
		private void OnCamera(InputAction.CallbackContext ctx)
		{
			playerCamera.MoveCamera(ctx.ReadValue<Vector2>());
		}
		private void OnCameraStop(InputAction.CallbackContext ctx)
		{
			playerCamera.MoveCamera(Vector2.zero);
		}

		private void OnPeak(InputAction.CallbackContext ctx)
		{
			playerCamera.StartPeaking(ctx.ReadValue<float>());
		}

		private void OnStopPeak(InputAction.CallbackContext ctx)
		{
			playerCamera.StopPeaking();
		}

		private void OnMove(InputAction.CallbackContext ctx)
		{
			moveDir = ctx.ReadValue<Vector2>();
		}

		private void OnStopMove(InputAction.CallbackContext ctx)
		{
			moveDir = Vector2.zero;
		}

		private void OnJump(InputAction.CallbackContext ctx)
		{
			playerMovement.JumpPressed = true;
			if (playerMovement.IsGrounded())
			{
				if (isCrouched)
				{
					if (playerMovement.CanGetUp())
					{
						isCrouched = false;
						playerMovement.GetUp();
						playerCamera.OnCrouchEnd();
					}
				}

				playerMovement.Jump();
				playerAnimationController.Jump();
			}
		}

		private void OnJumpReleased(InputAction.CallbackContext ctx)
		{
			playerMovement.JumpPressed = false;
		}

		private void OnRun(InputAction.CallbackContext ctx)
		{
			if (isCrouched && !playerMovement.CanGetUp()) return;

			isRunning = true;

			if (isCrouched)
			{
				playerCamera.OnCrouchEnd();
				playerMovement.GetUp();
			}

			isCrouched = false;
		}

		private void OnStopRun(InputAction.CallbackContext ctx)
		{
			isRunning = false;
			playerCamera.OnSprintEnd();
		}

		private void OnCrouch(InputAction.CallbackContext ctx)
		{
			if (!playerMovement.IsGrounded()) return;

			if (!isCrouched)
			{
				playerCamera.OnCrouchBegin();
				isCrouched = true;
				// Slide
				if (isRunning)
				{
					playerMovement.Slide();
					playerAnimationController.Slide();
					playerCamera.OnSprintEnd();
				}
				else
				{
					playerMovement.GoDown();
				}
			}
			else if (playerMovement.CanGetUp())
			{
				isCrouched = false;
				playerCamera.OnCrouchEnd();
				playerMovement.GetUp();
			}

			isRunning = false;
		}

		private void OnNote(InputAction.CallbackContext ctx)
		{

		}

		private void OnPause(InputAction.CallbackContext ctx)
		{

		}
		#endregion

		#region States Management
		private void UpdateState()
		{
			playerMovement.InputDirection = moveDir;

			if (moveDir == Vector2.zero)
			{
				currentState = State.IDLE;
			}
			else if (isCrouched) currentState = State.CROUCH;
			else if (isRunning) currentState = State.RUN;
			else currentState = State.WALK;
		}
		private void Idle()
		{
			playerMovement.Stop();
			playerAnimationController.Idle();
		}

		private void Walk()
		{
			playerMovement.Walk();
			playerAnimationController.Walk();
		}

		private void Run()
		{
			playerMovement.Run();
			playerCamera.OnSprintBegin();
			playerAnimationController.Run();
		}

		private void Crouch()
		{
			playerMovement.Crouch();
			playerAnimationController.Crouch();
		}
		#endregion
	}
}
