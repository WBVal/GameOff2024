using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
	public class InteractionHelper : MonoBehaviour
	{
		[SerializeField]
		TextMeshProUGUI key;
		public string Key { get { return key.text; } set { key.text = value; } }

		[SerializeField]
		TextMeshProUGUI helper;
		public string Helper { get { return helper.text; } set { helper.text = value; } }

	}
}
