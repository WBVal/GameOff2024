using Gameplay.Level;
using Gameplay.NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	[CreateAssetMenu(fileName = "AttributeData", menuName = "ScriptableObjects/AttributeData")]
	public class AttributeIconsData : ScriptableObject
	{
		[System.Serializable]
		public class AttributeIcon
		{
			public NpcAttribute attribute;
			public Sprite icon;
		}

		public AttributeIcon[] attributeIcons;
    }
}
