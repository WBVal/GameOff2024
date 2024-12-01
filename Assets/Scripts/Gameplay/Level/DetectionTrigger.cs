using Gameplay.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
	public class DetectionTrigger : MonoBehaviour
	{
		[SerializeField]
		GatheringArea area;

		private void OnTriggerEnter(Collider other)
		{
			area.OnNpcDetection();
		}
	}
}
