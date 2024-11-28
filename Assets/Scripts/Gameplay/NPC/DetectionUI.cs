using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionUI : MonoBehaviour
{
	[SerializeField]
	Image eyeIcon;
	[SerializeField]
	Animator eyeAnimator;

	private void Awake()
	{
		eyeIcon.gameObject.SetActive(false);
	}

	public void SetDetection(float value)
	{
		eyeIcon.gameObject.SetActive(value > 0f);
		eyeAnimator.Play("OpenEye", 0, value);
	}
}
