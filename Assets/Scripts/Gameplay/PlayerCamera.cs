using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
	public class PlayerCamera : MonoBehaviour
	{
		[SerializeField]
		Transform cameraTransform;

		[Header("Parameters")]
		[SerializeField]
		float sensX;
		[SerializeField]
		float sensY;

		float xRotation;
		float yRotation;

		Vector2 mouseDir;


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
	}
}
