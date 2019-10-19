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
	Vector3 rotationAxis = Vector3.forward;
	[SerializeField]
	float maxSpeed = 10f;
	[SerializeField]
	float acceleration = 10f;
	[SerializeField, Tooltip("Degrees Per Frame")]
	float turnSpeed = 360f;
	[SerializeField]
	float dragFactor = 0.8f;
	bool moving = false;

	Vector3 externalForce = Vector3.zero;

	private void Start()
	{
		body = GetComponent<Rigidbody>();
		anim = GetComponent<PlayerAnimator>();

		rotationAxis = Vector3.Cross(horizontalAxis, verticalAxis);
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
			var lookAt = internalForce.normalized;

			var angle = Mathf.Acos(Vector3.Dot(Vector3.forward, lookAt)) * Mathf.Rad2Deg;
			if (Vector3.Dot(Vector3.right, lookAt) > 0)
			{
				angle *= -1;
			}

			body.rotation = Quaternion.RotateTowards(body.rotation, Quaternion.Euler(rotationAxis * angle), turnSpeed);
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
