using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.NPC
{
	public class NpcAttributesDisplay : MonoBehaviour
	{
		[System.Serializable]
		public class AttributeModel
		{
			public NpcAttribute attribute;
			public GameObject asset;
		}

		[Header("Attribute models")]
		[SerializeField]
		List<AttributeModel> models;

		public void InitAttributes(NpcAttribute[] attributes)
		{
			foreach (AttributeModel model in models)
			{
				model.asset.SetActive(false);
			}
			foreach (NpcAttribute attribute in attributes)
			{
				foreach(AttributeModel model in models)
				{
					if(model.attribute == attribute)
						model.asset.SetActive(true);
				}
			}
		}
	}
}
