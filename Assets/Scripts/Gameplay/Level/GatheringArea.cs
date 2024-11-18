using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
    public class GatheringArea : SpawnArea
    {
		[SerializeField]
		ClueDisplay clueDisplay;

        private Clue clue;
        public Clue Clue 
		{ 
			get => clue;
			set
			{
				clue = value;
				clueDisplay.SetClue(value);
			} 
		}

		bool isLoading;
		bool clueLocked;

		private void OnTriggerEnter(Collider other)
		{
			if (!clueLocked)
			{
				isLoading = true;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			isLoading = false;
		}

		private void OnClueLoaded()
		{

		}

		private void OnDetected()
		{
			clueLocked = false;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			foreach(Transform t in spawnPoints)
			{
				Gizmos.DrawWireCube(t.position, Vector3.one);
			}
		}
	}
}
