using Gameplay.NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Level
{
	public class Clue : MonoBehaviour
	{
		NpcAttribute attribute;
		public NpcAttribute Attribute { get => attribute; set => attribute = value; }

		private bool isEqual;
		public bool IsEqual { get => isEqual; set => isEqual = value; }
	}
}
