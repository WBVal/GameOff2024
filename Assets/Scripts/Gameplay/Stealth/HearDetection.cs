using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Gameplay.Player;

namespace Gameplay.Stealth
{
    public class HearDetection : MonoBehaviour
    {
		[SerializeField]
		NavMeshAgent soundAgent;

		[Header("Hearing range")]
		[SerializeField]
		float closeDetectionRange;
		[SerializeField]
		float farDetectionRange;

		Player.Player player;
		Transform playerTransform;

		private void Awake()
		{
			player = GameManager.Instance.Player;
			playerTransform = player.transform;
		}

		public bool PlayerHeard()
		{
			if(Vector3.Distance(transform.position, playerTransform.position) > farDetectionRange)
				return false;
			
			soundAgent.SetDestination(playerTransform.position);

			if(soundAgent.remainingDistance <= closeDetectionRange)
			{
				return (player.NoiseLevel > 0f);
			}
			else if(soundAgent.remainingDistance <= farDetectionRange)
			{
				return (player.NoiseLevel > 0.5f);
			}

			return false;
		}

		//private void OnDrawGizmos()
		//{
		//	Gizmos.color = Color.green;
		//	Gizmos.DrawWireSphere(transform.position, farDetectionRange);
		//	Gizmos.DrawWireSphere(transform.position, closeDetectionRange);
		//}
	}
}
