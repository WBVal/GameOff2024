using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Player;
using Managers;

namespace Gameplay.Level 
{
	public class EndArea : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Player.Player>().HasEye)
			{
				GameManager.Instance.OnEndAreaEnter();
			}
			else
			{
				HudManager.Instance.Message("Bring the eye back here.");
			}
		}
	}
}
