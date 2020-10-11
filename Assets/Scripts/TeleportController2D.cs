using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController2D : MonoBehaviour
{
    public Transform destination;


    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.transform.position = destination.transform.position;
    }
}
