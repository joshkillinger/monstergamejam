using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
	Rigidbody body = null;
	PlayerAnimator anim = null;
	[SerializeField]
	Vector3 horizontalAxis = Vector3.right;
	[SerializeField]
	Vector3 verticalAxis = Vector3.up;
	Vector3 rotationAxis = Vector3.forward;
	[SerializeField]
	MoveStats defaultStats = null;
	MoveStats stats = null;
	bool moving = false;

	Vector3 externalForce = Vector3.zero;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		anim = GetComponent<PlayerAnimator>();

		rotationAxis = Vector3.Cross(horizontalAxis, verticalAxis);
		stats = defaultStats;
	}

	private void FixedUpdate()
	{
		rotationAxis = Vector3.Cross(horizontalAxis, verticalAxis);

		var horizontal = Input.GetAxis($"Horizontal");
		var vertical = Input.GetAxis($"Vertical");


		if (Mathf.Abs(horizontal) > Helper.Epsilon || Mathf.Abs(vertical) > Helper.Epsilon)
		{
			if (moving == false)
			{
				moving = true;
				if (anim != null)
				{
					anim.StartAnimation("move");
				}
			}
		}
		else
		{
			if (moving == true)
			{
				moving = false;
				if (anim != null)
				{
					anim.StopAnimation("move");
				}
			}
		}

		body.AddForce(externalForce, ForceMode.Impulse);

		var internalForce = (((horizontalAxis * horizontal) + (verticalAxis * vertical))).normalized * stats.acceleration;

		if (internalForce.sqrMagnitude > 0)
		{
			var lookAt = internalForce.normalized;
			var angle = Mathf.Acos(Vector3.Dot(Vector3.forward, lookAt)) * Mathf.Rad2Deg;
			if (Vector3.Dot(Vector3.right, lookAt) > 0)
			{
				angle *= -1;
			}

			// If attempting to turn fully around, randomly choose a direction to turn.
			if (Mathf.Abs(Vector3.Dot(transform.forward, lookAt) + 1) < Helper.Epsilon && Random.Range(0f, 1f) < 0.5f)
			{
				angle = -180;
			}

			body.rotation = Quaternion.RotateTowards(body.rotation, Quaternion.Euler(rotationAxis * angle), stats.turnSpeed);
		}

		body.AddForce(internalForce);

		if (body.velocity.sqrMagnitude > (stats.maxSpeed * stats.maxSpeed))
		{
			body.velocity = body.velocity.normalized * stats.maxSpeed;
		}

		if ((internalForce.sqrMagnitude + externalForce.sqrMagnitude) < Helper.Epsilon)
		{
			body.velocity *= stats.dragFactor;
		}

		externalForce = Vector3.zero;
	}

	public void ApplyExternalForce(Vector3 force)
	{
		externalForce += force;
	}

	public void SetMoveStats(MoveStats newStats)
	{
		stats = newStats;
	}

	public void ResetMoveStats()
	{
		stats = defaultStats;
	}
}

[System.Serializable]
public class MoveStats
{
	[SerializeField]
	public float maxSpeed = 1000f;
	[SerializeField]
	public float acceleration = 100f;
	[SerializeField, Tooltip("Degrees Per Frame")]
	public float turnSpeed = 15f;
	[SerializeField]
	public float dragFactor = 0.8f;
}
