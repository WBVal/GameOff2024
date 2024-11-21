using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.NPC
{
	public enum NpcAttribute
	{
		BERET,
		HAT,
		NECKTIE,
		BOWTIE,
		GLASSES,
		CIG,
		SHIRT,
		SUSPENDERS,
		BOOTS
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

			if(element == NpcAttribute.BERET || element == NpcAttribute.HAT)
			{
				list.Remove(NpcAttribute.BERET);
				list.Remove(NpcAttribute.HAT);
				return;
			}
			if (element == NpcAttribute.BOWTIE || element == NpcAttribute.NECKTIE)
			{
				list.Remove(NpcAttribute.BOWTIE);
				list.Remove(NpcAttribute.NECKTIE);
				return;
			}
		}
	}
}
