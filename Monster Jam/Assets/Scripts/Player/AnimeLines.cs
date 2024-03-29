﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeLines : MonoBehaviour
{
    public ParticleSystem particles;
    private Boost boost;


    private bool cachedBoost = false;
    private bool initialized = false;

    IEnumerator Start()
    {
        while (boost == null)
        {
            boost = GameObject.FindWithTag("Player")?.GetComponent<Boost>();
            yield return null;
        }
        initialized = true;
        particles.gameObject.SetActive(false);
        StartCoroutine(show());
    }

    void Update()
    {
        if (initialized)
        {
            if (boost.Boosting && !cachedBoost)
            {
                StopAllCoroutines();
                cachedBoost = true;
                StartCoroutine(show());
            }
        }
    }

    private IEnumerator show()
    {
        particles.gameObject.SetActive(true);
        particles.Play();

        while (boost.Boosting)
        {
            var angle = Vector3.SignedAngle(Vector3.forward, boost.transform.forward, Vector3.up);
            transform.localRotation = Quaternion.Euler(0, 0, -angle);
            yield return null;
        }

        particles.Stop();
        cachedBoost = false;

        yield return new WaitForSeconds(1f);
        particles.gameObject.SetActive(false);
    }
}
