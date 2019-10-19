using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerAnimator))]
public class PlayerMover : MonoBehaviour
{
	Rigidbody body = null;
	PlayerAnimator anim = null;
	[SerializeField]
	Vector3 horizontalAxis = Vector3.right;
	[SerializeField]
	Vector3 verticalAxis = Vector3.up;
	[SerializeField]
	float maxSpeed = 10f;
	[SerializeField]
	float acceleration = 10f;
	[SerializeField, Tooltip("Degrees Per Frame")]
	float turnSpeed = 10f;
	[SerializeField]
	float dragFactor = 0.8f;
	bool moving = false;

	Vector3 externalForce = Vector3.zero;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		anim = GetComponent<PlayerAnimator>();
	}

	private void FixedUpdate()
	{
		var horizontal = Input.GetAxis($"Horizontal");
		var vertical = Input.GetAxis($"Vertical");

		if (Mathf.Abs(horizontal) > Helper.Epsilon || Mathf.Abs(vertical) > Helper.Epsilon)
		{
			if (moving == false)
			{
				moving = true;
				anim.StartAnimation("move");
			}
		}
		else
		{
			if (moving == true)
			{
				moving = false;
				anim.StopAnimation("move");
			}
		}

		body.AddForce(externalForce, ForceMode.Impulse);

		var internalForce = (((horizontalAxis * horizontal) + (verticalAxis * vertical))).normalized * acceleration;

		if (internalForce.sqrMagnitude > 0)
		{
			var lookAt = transform.position + internalForce;
			transform.LookAt(lookAt);
		}

		if (body.velocity.sqrMagnitude < (maxSpeed * maxSpeed))
		{
			body.AddForce(internalForce);
		}

		if ((internalForce.sqrMagnitude + externalForce.sqrMagnitude) > Helper.Epsilon)
		{
			body.velocity *= dragFactor;
		}
		else
		{
			body.velocity = Vector3.zero;
		}

		externalForce = Vector3.zero;
	}

	public void ApplyExternalForce(Vector3 force)
	{
		externalForce += force;
	}
}
