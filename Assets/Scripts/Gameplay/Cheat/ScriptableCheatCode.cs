using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cheat
{
	public class ScriptableCheatCode : ScriptableObject
	{
		[SerializeField]
		protected string code;
		public string Code { get => code; }
		public virtual void Apply()
		{
			if (AudioManager.Instance != null)
				AudioManager.Instance.PlayCheatSound();
		}
	}
}
