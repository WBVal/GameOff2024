using Gameplay.Level;
using Gameplay.NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Gameplay.Tutorial
{
	public class TutorialManager : Singleton<TutorialManager>
	{
		[SerializeField]
		Npc target;

		[SerializeField]
		GatheringArea[] areas;

		NpcAttribute[] tutoAttributes = { NpcAttribute.HAT, NpcAttribute.SHIRT };
		private void Awake()
		{
			target.Attributes = tutoAttributes;
			target.IsTarget = true;

			Clue firstClue = new Clue();
			firstClue.Attribute = tutoAttributes[0];
			firstClue.IsEqual = true;

			Clue secondClue = new Clue();
			secondClue.Attribute = tutoAttributes[1];
			secondClue.IsEqual = true;

			areas[0].Clue = firstClue;
			areas[1].Clue = secondClue;
		}
	}
}
