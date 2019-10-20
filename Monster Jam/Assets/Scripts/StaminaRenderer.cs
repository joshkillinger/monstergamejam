using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaRenderer : MonoBehaviour
{
	Stamina stamina = null;

	[SerializeField]
	Text text;

	private void Start()
	{
		stamina = GameObject.FindWithTag("Player").GetComponentInChildren<Stamina>();
	}

	private void Update()
	{
		if (stamina != null)
		{
			if (text != null)
			{
				text.text = "" + (int)(stamina.Total * 100);
			}
		}
	}
}
