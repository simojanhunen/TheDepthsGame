using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportController2D : MonoBehaviour
{
    // public Transform destination;
    public int nextScene = 1;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(nextScene);
    }
}
