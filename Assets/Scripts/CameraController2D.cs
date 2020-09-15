using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset_from_character;

    void Start()
    {
        offset_from_character = player.transform.position - transform.position;
    }

    void LateUpdate()
    {
        // TODO: smoothing without blurring character sprites
        transform.position = player.transform.position - offset_from_character;  
    }
}
