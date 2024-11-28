using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
	[SerializeField]
	Animator animator;
	[SerializeField]
	TextMeshProUGUI label;

	public void OnDeath()
	{
		label.text = "You died";
	}

	public void OnVictory()
	{
		label.text = "An eye for an eye";
	}

	public void OnMissExecute()
	{
		label.text = "Innocent killed";
	}
}
