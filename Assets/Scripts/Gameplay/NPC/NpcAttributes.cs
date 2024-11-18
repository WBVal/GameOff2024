using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.NPC
{
	public enum NpcAttribute
	{
		CAP,
		DURAG,
		GLASSES,
		CIG,
		CHAIN,
		HOODIE,
		TANKTOP,
		SHIRT,
		SNEAKERS,
		BOOTS,
	}

	public static class NpcAttributesController
	{
		public static NpcAttribute[] Init()
		{
			return (NpcAttribute[])Enum.GetValues(typeof(NpcAttribute));
		}

		public static void AttributeCompatibilityCleanUp(List<NpcAttribute> list, NpcAttribute element)
		{
			list.Remove(element);

			if(element == NpcAttribute.CAP || element == NpcAttribute.DURAG)
			{
				list.Remove(NpcAttribute.DURAG);
				list.Remove(NpcAttribute.CAP);
				return;
			}
			if (element == NpcAttribute.HOODIE || element == NpcAttribute.TANKTOP || element == NpcAttribute.SHIRT)
			{
				list.Remove(NpcAttribute.HOODIE);
				list.Remove(NpcAttribute.SHIRT);
				list.Remove(NpcAttribute.TANKTOP);
				return;
			}
			if (element == NpcAttribute.SNEAKERS || element == NpcAttribute.BOOTS)
			{
				list.Remove(NpcAttribute.SNEAKERS);
				list.Remove(NpcAttribute.BOOTS);
				return;
			}
		}
	}
}
