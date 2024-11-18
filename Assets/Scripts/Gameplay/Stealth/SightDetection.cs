using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Gameplay.Stealth
{
    public class SightDetection : MonoBehaviour
    {
        [SerializeField]
        Light spotlight;

        [SerializeField]
        LayerMask viewMask;

		float viewRange;
        float viewAngle;

        Transform playerTransform;

		private void Awake()
		{
			viewRange = spotlight.range;
            viewAngle = spotlight.innerSpotAngle;

            playerTransform = GameManager.Instance.Player.transform;
		}

        public bool PlayerInSight()
        {
            if(Vector3.Distance(transform.position, playerTransform.position) < viewRange)
            {
                Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

                if(angleToPlayer < viewAngle / 2f)
                {
                    if(!Physics.Linecast(transform.position, playerTransform.position, viewMask))
                    {
                        return true;
                    }
                }
			}
			return false;
        }
	}
}
