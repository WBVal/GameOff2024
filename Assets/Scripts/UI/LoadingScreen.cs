using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    float fadeTime;
	[SerializeField]
    CanvasGroup canvasGroup;

	public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while(elapsedTime < fadeTime)
        {
            canvasGroup.alpha = elapsedTime / fadeTime;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
		canvasGroup.alpha = 1f;
    }

	public IEnumerator FadeOut()
	{
		float elapsedTime = 0f;
		while (elapsedTime < fadeTime)
		{
			canvasGroup.alpha = 1f - elapsedTime / fadeTime;
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}
		canvasGroup.alpha = 0f;
	}
}
