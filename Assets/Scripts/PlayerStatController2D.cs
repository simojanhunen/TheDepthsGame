using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController2D : MonoBehaviour
{
    public float hitpoints;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player hitpoints were fetched.");
        hitpoints = PlayerStatStorage.inst.hitpoints;
    }

    // Each loop hitpoints are stored to PlayerStatStorage
    void Update()
    {
        PlayerStatStorage.inst.hitpoints = hitpoints;
    }
}
