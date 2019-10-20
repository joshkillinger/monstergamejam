using System.Collections;
using UnityEngine;

public class RunAnyCoroutine : MonoBehaviour
{
    public void Run(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
