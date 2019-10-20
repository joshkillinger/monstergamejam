using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineFindTarget : MonoBehaviour
{
	[SerializeField]
	string targetTag = "Player";
	GameObject target = null;
	CinemachineVirtualCamera cineCamera = null;

	private void Start()
	{
		cineCamera = GetComponent<CinemachineVirtualCamera>();
		FindTarget();
	}

	private void Update()
	{
		if (target != null && cineCamera.Follow == null || cineCamera.LookAt != null)
		{
			cineCamera.Follow = target.transform;
			cineCamera.LookAt = target.transform;
		}
	}

	public void FindTarget()
	{
		target = GameObject.FindWithTag(targetTag);
	}
}
