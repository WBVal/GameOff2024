using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public class LookAtPlayer : MonoBehaviour
	{
		void Update()
		{
			transform.localScale = new Vector3(-1f, 1f, 1f);
			transform.LookAt(GameManager.Instance.Player.transform.position);
		}
	}
}

