using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HomeHinter : MonoBehaviour
{
    RectTransform rect;
    Transform player;
    Transform home;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        home = GameObject.FindWithTag("Home").transform;
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        var y = (player.position - home.position).y;
        rect.localRotation = Quaternion.Euler(0, 0, y);
    }
}
