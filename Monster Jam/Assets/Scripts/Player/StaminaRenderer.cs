using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaRenderer : MonoBehaviour
{
	Stamina stamina = null;

	[SerializeField]
	Image fill = null;

	private void Start()
	{
		stamina = GameObject.FindWithTag("Player").GetComponentInChildren<Stamina>();
	}

	private void Update()
	{
		if (stamina != null)
		{
			if (fill != null)
			{
				fill.fillAmount = stamina.Total;
			}
		}
	}
}
