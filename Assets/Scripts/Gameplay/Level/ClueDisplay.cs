using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Level
{
    public class ClueDisplay : MonoBehaviour
	{
        [SerializeField]
        Slider loadingSlider;
		[SerializeField]
        TextMeshProUGUI clueText;

		private void Awake()
		{
			loadingSlider.gameObject.SetActive(false);
            clueText.gameObject.SetActive(false);
		}

		public void DisplayClue(Clue clue)
		{
			if(clue != null)
			{
				loadingSlider.gameObject.SetActive(false);
				clueText.gameObject.SetActive(true);
				clueText.text = "Target "
					+ (clue.IsEqual ? "has " : "has no ")
					+ clue.Attribute.ToString();
			}
        }

        public void LoadClue(float progress)
		{
			loadingSlider.gameObject.SetActive(true);
			loadingSlider.value = progress;
        }

        public void LockClue()
		{
			loadingSlider.gameObject.SetActive(false);
			clueText.gameObject.SetActive(true);
			clueText.text = "CLUE LOCKED";
		}

        public void HideClue()
		{
			loadingSlider.gameObject.SetActive(false);
			clueText.gameObject.SetActive(true);
			clueText.text = "CLUE HIDDEN";
		}
    }
}
