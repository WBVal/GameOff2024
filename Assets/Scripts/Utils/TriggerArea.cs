using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public Action OnEnter;
	public Action OnExit;

	private void OnTriggerEnter(Collider other)
	{
		OnEnter?.Invoke();
	}

	private void OnTriggerExit(Collider other)
	{
		OnExit?.Invoke();
	}
}
