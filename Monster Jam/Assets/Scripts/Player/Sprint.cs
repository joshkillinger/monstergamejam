using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class Sprint : MonoBehaviour
{
	PlayerMover mover = null;

	[SerializeField]
	MoveStats sprintStats = null;
	bool sprinting = false;

	[SerializeField]
	Stamina stamina = null;
	[SerializeField]
	float staminaConsumption = 0.3f;

	public bool PreventSprinting = false;

	private void Start()
	{
		mover = GetComponent<PlayerMover>();
	}

	private void Update()
	{
		if (!PreventSprinting)
		{
			bool shouldSprint = Input.GetAxis("Sprint") > 0;
			if (shouldSprint && stamina != null)
			{
				shouldSprint = stamina.Consume(staminaConsumption * Time.deltaTime);
			}

			if (shouldSprint != sprinting)
			{
				sprinting = shouldSprint;
				if (shouldSprint)
				{
					mover.SetMoveStats(sprintStats);
				}
				else
				{
					mover.ResetMoveStats();
				}
			}
		}
		else
		{
			sprinting = false;
		}
	}
}
