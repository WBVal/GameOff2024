using Cinemachine;
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
		[SerializeField]
		float sensY;
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

		private Vignette vignette;
		float baseVignetteSmoothness;

		Coroutine fovAnimCoroutine;
		Coroutine vignetteAnimCoroutine;

		private void Awake()
		{
			cameraTransform = m_Camera.transform;
			m_Camera.m_Lens.FieldOfView = baseFOV;
			volume.profile.TryGet<Vignette>(out vignette);
			baseVignetteSmoothness = vignette.smoothness.value;
		}

		void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		// Update is called once per frame
		void Update()
		{
			yRotation += mouseDir.x * Time.deltaTime * sensX;
			xRotation -= mouseDir.y * Time.deltaTime * sensY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			cameraTransform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
			transform.rotation = Quaternion.Euler(0, yRotation, 0);
		}

		public void MoveCamera(Vector2 dir)
		{
			mouseDir = dir;
		}

		public void OnSprintBegin()
		{
			if(fovAnimCoroutine != null)
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
