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
    bool initialized = false;

	private IEnumerator Start()
	{
		cineCamera = GetComponent<CinemachineVirtualCamera>();
        while (target == null)
        {
            target = GameObject.FindWithTag(targetTag);
            yield return null;
        }
        initialized = true;
	}

	private void Update()
	{
        if (initialized)
        {
            if (target != null && cineCamera.Follow == null || cineCamera.LookAt != null)
            {
                cineCamera.Follow = target.transform;
                cineCamera.LookAt = target.transform;
            }
        }
	}

	
}
