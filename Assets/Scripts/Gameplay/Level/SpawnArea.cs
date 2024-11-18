using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField]
        protected Transform[] spawnPoints;
        public Transform[] SpawnPoints { get { return spawnPoints; } }

        public int Capacity { get { return spawnPoints.Length; } }
    }
}
