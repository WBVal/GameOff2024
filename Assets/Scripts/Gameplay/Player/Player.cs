using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player
{
	public class Player : MonoBehaviour
	{
		[SerializeField]
		private float noiseLevel;
		public float NoiseLevel { get => noiseLevel; set => noiseLevel = Mathf.Clamp(value, 0f, 1f); }
	}
}
