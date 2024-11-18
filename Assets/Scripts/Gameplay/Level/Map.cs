using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
	public class Map : MonoBehaviour
	{
		[Header("Spawn areas")]
		[SerializeField]
		List<GatheringArea> gatherSpawnAreas;
		public List<GatheringArea> GatherSpawnAreas { get => gatherSpawnAreas; private set => gatherSpawnAreas = value; }

		[SerializeField]
		List<SpawnArea> singleSpawnAreas;
		public List<SpawnArea> SingleSpawnAreas { get => singleSpawnAreas; private set => singleSpawnAreas = value; }
	}
}
