using Gameplay.NPC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Level
{
	public class LevelGenerator : MonoBehaviour
	{
		[SerializeField]
		protected Npc npcPrefab;

		[SerializeField]
		private Map map;

		[Header("Generation Parameters")]
		[SerializeField]
		int nbClues;
		[SerializeField]
		[Range(0.1f, 1f)]
		float mimicRatio;
		[SerializeField]
		int minimumGatheringAreas;
		[SerializeField]
		int minimumSingleAreas;

		private List<Clue> clueList;
		public List<Clue> ClueList { get { return clueList; } }


		private NpcAttribute[] attributes;

		private List<NpcAttribute> targetAttributes;

		private List<GatheringArea> gatheringAreas;
		private List<SpawnArea> singleAreas;

		private List<Npc> npcList;

		private int nbMimics;

		private void Awake()
		{
			attributes = NpcAttributesController.Init();
			gatheringAreas = new List<GatheringArea>();
			singleAreas = new List<SpawnArea>();
			npcList = new List<Npc>();
		}

		[ContextMenu("Generate Level")]
		public async void GenerateLevel()
		{
			// Pick Target profile
			GenerateTargetAttributes();

			// Generate all clues
			clueList = new List<Clue>();
			for (int i = 0; i < nbClues; i++)
			{
				Clue clue = new Clue();
				clue.Attribute = targetAttributes[i];
				clue.IsEqual = true;
				clueList.Add(clue);
			}

			// Pick map layout (gathering and single areas)
			PickSpawnAreas();

			// Fill gathering areas with NPCs and one clue
			foreach (GatheringArea area in gatheringAreas)
			{
				SpawnNpc(area);
				int clueIndex = Random.Range(0, clueList.Count);
				area.Clue = clueList[clueIndex];
				area.Clue.IsEqual = true; // For now, every clue is an affiramtion
				clueList.Remove(clueList[clueIndex]);
			}

			// Fill single areas
			foreach (SpawnArea area in singleAreas)
			{
				SpawnNpc(area);
			}

			// Give target profile to a random npc
			int randomNpcIndex = Random.Range(0, npcList.Count);
			npcList[randomNpcIndex].Attributes = targetAttributes.ToArray();
			npcList[randomNpcIndex].IsTarget = true;
			npcList.RemoveAt(randomNpcIndex);

			// Give other Npcs attributes
			nbMimics = (int)(npcList.Count * mimicRatio);
			int mimicCount = 0;

			while(npcList.Count > 0)
			{
				int randIndex = Random.Range(0, npcList.Count);

				// if a mimic must be generated, give it clue attributes
				npcList[randIndex].Attributes = GenerateOtherNpcAttributes(mimicCount < nbMimics);
				npcList[randIndex].IsMimic = mimicCount < nbMimics;

				mimicCount++;

				npcList.RemoveAt(randIndex);
			}

			// (Bonus) create props
		}

		[ContextMenu("Generate Target Attributes")]
		public void GenerateTargetAttributes()
		{
			targetAttributes = new List<NpcAttribute>();
			List<NpcAttribute> tempAttributes = new List<NpcAttribute>(attributes);

			for (int i = 0; i < nbClues; i++)
			{
				NpcAttribute newAttribute = tempAttributes[Random.Range(0, tempAttributes.Count)];
				targetAttributes.Add(newAttribute);

				// Remove uncompatibilities from the available attributes list
				NpcAttributesController.AttributeCompatibilityCleanUp(tempAttributes, newAttribute);
			}
		}

		private NpcAttribute[] GenerateOtherNpcAttributes(bool hasClue)
		{
			List<NpcAttribute> npcAttributes = new List<NpcAttribute>();
			List<NpcAttribute> tempAttributes = new List<NpcAttribute>(attributes);

			int nbAttributes = nbClues;

			// Delete similarities in tempAttributes to avoid target ditto
			tempAttributes = tempAttributes.Except(targetAttributes).ToList();

			// if the npc must wear clues, pick one or two from target profile
			if (hasClue)
			{
				List<NpcAttribute> tempTargetAttributes = new List<NpcAttribute>(targetAttributes);

				int clueAttributes = Random.Range(1, nbClues); // number of clues to pick
				nbAttributes -= clueAttributes;

				for (int i = 0; i < clueAttributes; i++)
				{
					int randomIndex = Random.Range(0, tempTargetAttributes.Count); // pick a random attribute in target profile
					npcAttributes.Add(tempTargetAttributes[randomIndex]);
					tempTargetAttributes.RemoveAt(randomIndex);
				}
			}

			// pick remaining attributes
			for (int i = 0; i < nbAttributes; i++)
			{
				NpcAttribute newAttribute = tempAttributes[Random.Range(0, tempAttributes.Count)];
				npcAttributes.Add(newAttribute);

				// Remove uncompatibilities from the available attributes list
				NpcAttributesController.AttributeCompatibilityCleanUp(tempAttributes, newAttribute);
			}

			return npcAttributes.ToArray();
		}

		private void PickSpawnAreas()
		{
			List<GatheringArea> tempGatheringAreas = new List<GatheringArea>(map.GatherSpawnAreas);
			List<SpawnArea> tempSingleAreas = new List<SpawnArea>(map.SingleSpawnAreas);

			int nbGathering = Random.Range(minimumGatheringAreas, tempGatheringAreas.Count);
			int nbSingle = Random.Range(minimumSingleAreas, tempSingleAreas.Count);

			// Gathering areas
			for (int i = 0; i < nbGathering; i++)
			{
				int randIndex = Random.Range(0, tempGatheringAreas.Count);
				gatheringAreas.Add(tempGatheringAreas[randIndex]);
				tempGatheringAreas.RemoveAt(randIndex);
			}

			// Single areas
			for (int i = 0; i < nbSingle; i++)
			{
				int randIndex = Random.Range(0, tempSingleAreas.Count);
				singleAreas.Add(tempSingleAreas[randIndex]);
				tempSingleAreas.RemoveAt(randIndex);
			}
			Debug.Log("Generated " + nbGathering + " gathering areas and " + nbSingle + " single areas.");
		}

		private void SpawnNpc(SpawnArea area)
		{
			foreach (Transform t in area.SpawnPoints)
			{
				Npc newNpc = Instantiate(npcPrefab, transform);
				newNpc.transform.SetPositionAndRotation(t.position, t.rotation);
				npcList.Add(newNpc);
			}
		}
	}
}
