using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaView : MonoBehaviour
{
	StaminaManager stamina = null;

	[SerializeField]
	Image fill = null;

	private void Start()
	{
		stamina = GameObject.FindWithTag("GameController").GetComponentInChildren<StaminaManager>();
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
