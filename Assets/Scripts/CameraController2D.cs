using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset_from_character;
    public float Smoothing = 0.13f;
    public float Y_smoothing_multiplier = 2f;

    void Start()
    {
    }

    void FixedUpdate()
    {
        // New target position for the camera
        Vector3 targetPos;

        // Calculate new X position with 1x smoothing.
        targetPos.x = Mathf.MoveTowards(transform.position.x, player.transform.position.x, Smoothing);

        // For some reason camera stuttered while running on even ground so added threshold for camera movement in Y-direction,
        // the target position is calculated with parameterized smoothing multiplier, currently 2x smoothing.
        if (Mathf.Abs(transform.position.y - player.transform.position.y) < 0.1)
        {
            targetPos.y = transform.position.y;
        }
        else
        {
            targetPos.y = Mathf.MoveTowards(transform.position.y, player.transform.position.y, Smoothing * Y_smoothing_multiplier);
        }

        // Z coordinate shouldn't come from player (2D game), so use cameras own.
        targetPos.z = transform.position.z;

        transform.position = targetPos;
    }
}
