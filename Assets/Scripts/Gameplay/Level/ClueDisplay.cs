using DG.Tweening;
using Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Level
{
    public class ClueDisplay : MonoBehaviour
	{
		[SerializeField]
		AttributeIconsData iconData;
		[SerializeField]
        Slider loadingSlider;
		[SerializeField]
		Sprite clueLocked;
		[SerializeField]
		Sprite clueHidden;
		[SerializeField]
		GameObject clueDisplay;
		[SerializeField]
		Image clueState;
		[SerializeField]
		Image clueIcon;

		bool isLocked;
		RectTransform sliderRect;
		private void Awake()
		{
			clueState.sprite = clueHidden;
			clueDisplay.gameObject.SetActive(false);
			sliderRect = loadingSlider.GetComponent<RectTransform>();
		}

		public void DisplayClue(Clue clue)
		{
			clueState.gameObject.SetActive(false);
			sliderRect.DOSizeDelta(Vector2.one * 250f, 0.3f);
			clueDisplay.gameObject.SetActive(true);
			if (clue != null)
			{
				clueIcon.sprite = GetIcon(clue);
			}
        }

        public void LoadClue(float progress)
		{
			if (isLocked) return;

			loadingSlider.value = progress;
        }

        public void LockClue()
		{
			clueDisplay.gameObject.SetActive(false);
			clueState.gameObject.SetActive(true);
			sliderRect.DOSizeDelta(Vector2.one * 100f, 0.3f);
			loadingSlider.value = 1f;
			clueState.sprite = clueLocked;
			isLocked = true;
		}

        public void HideClue()
		{
			clueState.gameObject.SetActive(true);
			clueDisplay.gameObject.SetActive(false);
			sliderRect.DOSizeDelta(Vector2.one * 100f, 0.3f);
			loadingSlider.value = 0f;
			if (PlayerStatsManager.Instance.PlayerStats.NpcForgiveness) isLocked = false;
			clueState.sprite = clueHidden;
		}

		private Sprite GetIcon(Clue clue)
		{
			foreach(var attributeData in iconData.attributeIcons)
			{
				if(attributeData.attribute == clue.Attribute)
					return attributeData.icon;
			}
			return null;
		}
    }
}
