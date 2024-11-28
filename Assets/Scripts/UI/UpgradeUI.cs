using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class UpgradeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		[Header("UI Components")]
		[SerializeField]
		TextMeshProUGUI title;
		public string Title { get=>title.text; set=>title.text = value; }

		[SerializeField]
		TextMeshProUGUI description;
		public string Description { get => description.text; set => description.text = value; }

		[SerializeField]
		TextMeshProUGUI cost;
		public string Cost { get => cost.text; set => cost.text = value; }

		[SerializeField]
		Image background;

		[SerializeField]
		Image eyeIcon;

		[SerializeField]
		TextMeshProUGUI points;

		[Header("Debug")]
		[SerializeField]
		bool isAvailable;
		[SerializeField]
		bool isSelected;

		[Header("Text Colors")]
		[SerializeField]
		Color defaultTextColor;
		[SerializeField]
		Color hoverTextColor;
		[SerializeField]
		Color selectedTextColor;
		[SerializeField]
		Color disabledTextColor;

		[Header("Background Colors")]
		[SerializeField]
		Color defaultBackgroundColor;
		[SerializeField]
		Color hoverBackgroundColor;
		[SerializeField]
		Color selectedBackgroundColor;
		[SerializeField]
		Color disabledBackgroundColor;

		public bool IsAvailable { get => isAvailable; set => isAvailable = value; }

		public bool IsSelected {  get => isSelected; set => isSelected = value;}

		private Action onClick;
		public Action OnClick { get => onClick;set => onClick = value; }

		public delegate bool Compatibility();
		public Compatibility CompatibilityCheck;

		void Awake()
		{
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if (isSelected)
			{
				title.color = selectedTextColor;
				description.color = selectedTextColor;
				cost.color = selectedTextColor;
				eyeIcon.color = selectedTextColor;
				points.color = selectedTextColor;
				background.color = selectedBackgroundColor;
				return;
			}

			if (isAvailable)
			{
				title.color = defaultTextColor;
				description.color = defaultTextColor;
				cost.color = defaultTextColor;
				eyeIcon.color = defaultTextColor;
				points.color = defaultTextColor;
				background.color = defaultBackgroundColor;
			}
			else
			{
				title.color = disabledTextColor;
				description.color = disabledTextColor;
				cost.color = disabledTextColor;
				eyeIcon.color = disabledTextColor;
				points.color = disabledTextColor;
				background.color = disabledBackgroundColor;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!isAvailable) return;
			title.color = hoverTextColor;
			description.color = hoverTextColor;
			cost.color = hoverTextColor;
			eyeIcon.color = hoverTextColor;
			points.color = hoverTextColor;
			background.color = hoverBackgroundColor;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			UpdateDisplay();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (!isAvailable) return;

			title.color = selectedTextColor;
			description.color = selectedTextColor;
			cost.color = selectedTextColor;
			eyeIcon.color = selectedTextColor;
			points.color = selectedTextColor;
			background.color = selectedBackgroundColor;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (isSelected)
			{
				title.color = selectedTextColor;
				description.color = selectedTextColor;
				cost.color = selectedTextColor;
				eyeIcon.color = selectedTextColor;
				points.color = selectedTextColor;
				background.color = selectedBackgroundColor;
				onClick?.Invoke();
				return;
			}
			if (isAvailable)
			{
				title.color = defaultTextColor;
				description.color = defaultTextColor;
				cost.color = defaultTextColor;
				eyeIcon.color = defaultTextColor;
				points.color = defaultTextColor;
				background.color = defaultBackgroundColor;
				onClick?.Invoke();
				return;
			}

			title.color = disabledTextColor;
			description.color = disabledTextColor;
			cost.color = disabledTextColor;
			eyeIcon.color = disabledTextColor;
			points.color = disabledTextColor;
			background.color = disabledBackgroundColor;
		}
	}
}
