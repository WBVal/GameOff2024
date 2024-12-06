using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Utils;

namespace Gameplay.Player
{
	public class Player : MonoBehaviour
	{
		[SerializeField]
		LayerMask interactableLayer;

		[SerializeField]
		float maxExecuteDistance;

		[SerializeField]
		private bool canBeDetected;
		public bool CanBeDetected {  get => canBeDetected; set => canBeDetected = value; }

		[SerializeField]
		private float noiseLevel;
		public float NoiseLevel 
		{ 
			get => noiseLevel;
			set
			{
				noiseLevel = Mathf.Clamp(value, 0f, 1f);
				HudManager.Instance.SetNoiseDetection(noiseLevel);
			}
		}

		[SerializeField]
		private float detectionLevel;
		public float DetectionLevel 
		{ 
			get => detectionLevel;
			set 
			{
				detectionLevel = Mathf.Clamp(value, 0f, 1f);
				HudManager.Instance.SetSightDetection(detectionLevel);
			}
		}

		[SerializeField]
		private bool hasEye;
		public bool HasEye { get=>hasEye; set => hasEye = value; }

		private IFpsInteractable playerTarget;
		public IFpsInteractable PlayerTarget { get => playerTarget; set => playerTarget = value; }

		RaycastHit hit;
		Camera mainCam;
		Vector3 camCenter = new Vector3(0.5f, 0.5f, 0f);
		bool isExecuting;

		PlayerController playerController;
		PlayerCamera playerCamera;
		private void Awake()
		{
			mainCam = Camera.main;
			playerController = GetComponent<PlayerController>();
			playerCamera = GetComponent<PlayerCamera>();
		}

		private void Update()
		{
			if (Physics.Raycast(mainCam.ViewportPointToRay(camCenter), out hit, maxExecuteDistance))
			{
				if (1 << hit.transform.gameObject.layer == interactableLayer)
				{
					playerTarget = hit.collider.GetComponent<IFpsInteractable>();
					playerTarget.ShowHelper();
					HudManager.Instance.CrossHairInteract(true);
				}
			}
			else
			{
				playerTarget = null;
				HudManager.Instance.CrossHairInteract(false);
				HudManager.Instance.ShowHelper(string.Empty);
			}

			if (canBeDetected)
			{
				detectionLevel = Mathf.Clamp(detectionLevel - Time.deltaTime, 0f, 1f);
				HudManager.Instance.SetSightDetection(detectionLevel);
			}
		}

		public void Execute(Npc npc)
		{
			if (isExecuting) return;

            isExecuting = true;
			canBeDetected = false;
			playerController.Execute(npc);
		}

		public void OnExecuteEnd()
		{
			isExecuting = false;
			playerController.OnExecuteEnd();
			hasEye = true;
		}

		public void DisableInputs()
		{
			playerController.DisableInputs();
		}

		public void Killed()
		{
			playerController.OnKilled();
		}

		public void SetSens(float sens)
		{
			playerCamera.SensX = sens;
			playerCamera.SensY = sens;
		}
	}
}
