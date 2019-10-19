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

	private void Start()
	{
		mover = GetComponent<PlayerMover>();
	}

	private void Update()
	{
		if (Input.GetAxis("Sprint") > 0)
		{
			if (!sprinting)
			{
				mover.SetMoveStats(sprintStats);
			}
			sprinting = true;
		}
		else
		{
			if (sprinting)
			{
				mover.ResetMoveStats();
			}
			sprinting = false;
		}
	}
}
