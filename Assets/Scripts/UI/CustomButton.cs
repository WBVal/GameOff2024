using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		[Header("Components")]
		[SerializeField]
		TextMeshProUGUI text;
		[SerializeField]
		Image line;

		[Header("Colors")]
		[SerializeField]
		Color defaultColor;
		[SerializeField]
		Color hoverColor;
		[SerializeField]
		Color pressedColor;

		private void Awake()
		{
			line.gameObject.SetActive(false);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			text.color = hoverColor;
			line.gameObject.SetActive(true);
			line.color = hoverColor;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			text.color = defaultColor;
			line.gameObject.SetActive(false);
		}
		public void OnPointerDown(PointerEventData eventData)
		{
			text.color = pressedColor;
			line.gameObject.SetActive(true);
			line.color = pressedColor;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			text.color = hoverColor;
			line.color = hoverColor;
		}
	}
}
