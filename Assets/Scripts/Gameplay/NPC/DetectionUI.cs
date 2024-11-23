using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionUI : MonoBehaviour
{
    [SerializeField]
    Slider detectionSlider;

	private void Awake()
	{
		detectionSlider.value = 0f;
	}

	public void SetDetection(float value)
	{
		detectionSlider.value = value;
	}
}
