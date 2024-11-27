using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HudDetectionUI : MonoBehaviour
    {
        [Header("Eye")]
        [SerializeField]
        Image eyeImage;
        [SerializeField]
        Animator eyeAnimator;

        [Header("Hear")]
        [SerializeField]
        RectTransform[] bars;
		public void SetNormalizedSightDetection(float value)
        {
            eyeAnimator.Play("OpenEye", 0, value);
        }

        public void SetHearingDetectionLevel(float value)
        {
            float noiseLevel = Mathf.Clamp(value, 0.1f, 1.0f);
            foreach(RectTransform bar in bars)
            {
                bar.sizeDelta = new Vector2(bar.sizeDelta.x, Mathf.Lerp(bar.sizeDelta.y, Random.Range(10f, 50) * noiseLevel, Time.deltaTime * 10f));
            }
        }
    }
}
