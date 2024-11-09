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

		private void Awake()
		{
			inputs = new PlayerInputs();
			playerMovement = GetComponent<PlayerMovement>();
			playerCamera = GetComponent<PlayerCamera>();
		}

		private void OnEnable()
		{
			inputs.Player.Camera.performed += OnCamera;
			inputs.Player.Camera.canceled += OnCameraStop;
			inputs.Player.Movement.performed += OnMove;
			inputs.Player.Movement.canceled += OnStopMove;
			inputs.Player.Jump.performed += OnJump;
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
			Debug.Log(currentState);
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
			if (playerMovement.IsGrounded())
			{
				if (isCrouched)
				{
					if (playerMovement.CanGetUp())
					{
						isCrouched = false;
						playerMovement.GetUp();
					}
				}

				playerMovement.Jump();
			}
		}

		private void OnRun(InputAction.CallbackContext ctx)
		{
			isRunning = true;

			if (isCrouched)
				playerMovement.GetUp();

			isCrouched = false;
		}

		private void OnStopRun(InputAction.CallbackContext ctx)
		{
			isRunning = false;
		}

		private void OnCrouch(InputAction.CallbackContext ctx)
		{
			if (!isCrouched)
			{
				isCrouched = true;
				playerMovement.GetDown();
			}
			else if (playerMovement.CanGetUp())
			{
				isCrouched = false;
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
				return;
			}

			if (isRunning) currentState = State.RUN;
			else if (isCrouched) currentState = State.CROUCH;
			else currentState = State.WALK;
		}
		private void Idle()
		{
			playerMovement.Stop();
		}

		private void Walk()
		{
			playerMovement.Walk();
		}

		private void Run()
		{
			playerMovement.Run();
		}

		private void Crouch()
		{
			playerMovement.Crouch();
		}
		#endregion
	}
}
