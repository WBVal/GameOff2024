using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Message : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI contentText;

        Color baseTextColor;
        Coroutine displayCoroutine;

		public void DisplayMessage(string content, float duration)
        {
            contentText.text = content;
            if(displayCoroutine != null )
            {
                StopCoroutine(displayCoroutine);
            }
            displayCoroutine = StartCoroutine(DisplayCoroutine(duration));
		}

        private IEnumerator DisplayCoroutine(float duration)
		{
			baseTextColor = contentText.color;
			Color newColor = baseTextColor;
			newColor.a = 1f;

            float elapsedTime = 0f;
            while(elapsedTime < 0.5f)
            {
                elapsedTime += Time.deltaTime;
                newColor.a = elapsedTime / 0.5f;
                contentText.color = newColor;
                yield return null;
            }

            newColor.a = 1f;
            contentText.color = newColor;

            yield return new WaitForSeconds(duration);


			elapsedTime = 0f;
			while (elapsedTime < 0.5f)
			{
				elapsedTime += Time.deltaTime;
				newColor.a = 1f - elapsedTime / 0.5f;
				contentText.color = newColor;
				yield return null;
			}

			newColor.a = 0f;
			contentText.color = newColor;
		}
    }
}
