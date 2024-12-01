using Cinemachine;
using DG.Tweening;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Gameplay.Player
{
	public class PlayerCamera : MonoBehaviour
	{
		[SerializeField]
		CinemachineVirtualCamera m_Camera;

		[Header("Parameters")]
		[SerializeField]
		float sensX;
		public float SensX { get => sensX; set=>sensX=value; }
		[SerializeField]
		float sensY;
		public float SensY { get => sensY; set => sensY = value; }
		[SerializeField]
		float peakAmplitude;
		[Header("FOV FX")]
		[SerializeField]
		float fovChangeDuration;
		[SerializeField]
		float baseFOV;
		[SerializeField]
		float fastFOV;
		[Header("Vignette FX")]
		[SerializeField]
		float vignetteChangeDuration;
		[SerializeField]
		Volume volume;
		[SerializeField]
		float crouchVignetteSmoothness;

		float xRotation;
		float yRotation;

		Vector2 mouseDir;
		Transform cameraTransform;
		CinemachineCameraOffset offset;
		CinemachineRecomposer recomposer;

		private Vignette vignette;
		float baseVignetteSmoothness;

		Coroutine fovAnimCoroutine;
		Coroutine vignetteAnimCoroutine;

		bool canMove;
		public bool CanMove { get => canMove; set => canMove = value; }

		private void Awake()
		{
			cameraTransform = m_Camera.transform;
			m_Camera.m_Lens.FieldOfView = baseFOV;
			sensX = SceneFlowManager.Instance.Sensitivity;
			sensY = SceneFlowManager.Instance.Sensitivity;
			volume.profile.TryGet<Vignette>(out vignette);
			offset = m_Camera.GetComponent<CinemachineCameraOffset>();
			recomposer = m_Camera.GetComponent<CinemachineRecomposer>();
			baseVignetteSmoothness = vignette.smoothness.value;
		}

		void Start()
		{
			canMove = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void OnDisable()
		{
			SceneFlowManager.Instance.Sensitivity = sensX;
		}

		// Update is called once per frame
		void Update()
		{
			if (canMove)
			{
				yRotation += mouseDir.x * Time.deltaTime * sensX;
				xRotation -= mouseDir.y * Time.deltaTime * sensY;
				xRotation = Mathf.Clamp(xRotation, -90f, 90f);

				cameraTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
				transform.rotation = Quaternion.Euler(0, yRotation, 0);
			}
		}

		public void MoveCamera(Vector2 dir)
		{
			mouseDir = dir;
		}

		public void StartPeaking(float direction)
		{
			DOTween.To(() => offset.m_Offset.x, x => offset.m_Offset.x = x, Mathf.Sign(direction) * peakAmplitude, 0.5f);
			DOTween.To(() => recomposer.m_Dutch, x => recomposer.m_Dutch = x, Mathf.Sign(direction) * -10f, 0.5f);
		}

		public void StopPeaking()
		{
			DOTween.To(() => offset.m_Offset.x, x => offset.m_Offset.x = x, 0f, 0.5f);
			DOTween.To(() => recomposer.m_Dutch, x => recomposer.m_Dutch = x, 0f, 0.5f);
		}

		public void OnSprintBegin()
		{
			if (fovAnimCoroutine != null)
			{
				StopCoroutine(fovAnimCoroutine);
			}
			fovAnimCoroutine = StartCoroutine(ChangeFOV(fastFOV));
		}

		public void OnSprintEnd()
		{
			if (fovAnimCoroutine != null)
			{
				StopCoroutine(fovAnimCoroutine);
			}
			fovAnimCoroutine = StartCoroutine(ChangeFOV(baseFOV));
		}

		private IEnumerator ChangeFOV(float fov)
		{
			float elapsedTime = 0f;
			float currentFov = m_Camera.m_Lens.FieldOfView;
			while (elapsedTime < fovChangeDuration)
			{
				elapsedTime += Time.deltaTime;
				m_Camera.m_Lens.FieldOfView = Mathf.Lerp(currentFov, fov, elapsedTime / fovChangeDuration);
				yield return null;
			}
			m_Camera.m_Lens.FieldOfView = fov;
		}

		public void OnCrouchBegin()
		{
			if (vignetteAnimCoroutine != null)
			{
				StopCoroutine(vignetteAnimCoroutine);
			}
			vignetteAnimCoroutine = StartCoroutine(ChangeVignette(crouchVignetteSmoothness));
		}

		public void OnCrouchEnd()
		{
			if (vignetteAnimCoroutine != null)
			{
				StopCoroutine(vignetteAnimCoroutine);
			}
			vignetteAnimCoroutine = StartCoroutine(ChangeVignette(baseVignetteSmoothness));
		}
		private IEnumerator ChangeVignette(float smoothness)
		{
			float elapsedTime = 0f;
			float currentSmoothness = vignette.smoothness.value;
			while (elapsedTime < vignetteChangeDuration)
			{
				elapsedTime += Time.deltaTime;
				vignette.smoothness.value = Mathf.Lerp(currentSmoothness, smoothness, elapsedTime / vignetteChangeDuration);
				yield return null;
			}
			vignette.smoothness.value = smoothness;
		}
	}
}
