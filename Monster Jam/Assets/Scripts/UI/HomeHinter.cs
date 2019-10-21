using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HomeHinter : MonoBehaviour
{
    RectTransform rect;
    Transform player;
    Transform home;
    bool initialized = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            yield return null;
        }
        

        home = GameObject.FindWithTag("Home")?.transform;
        rect = GetComponent<RectTransform>();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            var toHome = (home.position - player.position).normalized;
            var angle = Vector3.SignedAngle(Vector3.forward, toHome, Vector3.up);
            rect.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}
