using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Upgrade
{
	public class ScriptableUpgrade: ScriptableObject
	{
		[SerializeField]
		protected string title;
		public string Title { get => title; }
		[SerializeField]
		protected string description;
		public string Description { get => description; }
		[SerializeField]
        protected int cost;
		public int Cost { get => cost; }

		[SerializeField]
		protected bool isUsed;
		public bool IsUsed { get => isUsed; set => isUsed = value; }

		public virtual bool CheckCompatibility() {  return true; }
		public virtual void Apply()
		{
			if (AudioManager.Instance != null)
				AudioManager.Instance.PlayUpgradeSound();
		}
		public virtual void Disable() { }
    }
}
