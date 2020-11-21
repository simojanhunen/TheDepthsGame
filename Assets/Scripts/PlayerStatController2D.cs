using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatController2D : MonoBehaviour
{
    public float hitpoints;
    public float damage;
    public float hitRange;

    // Start is called before the first frame update
    void Start()
    {
        hitpoints = PlayerStatStorage.inst.hitpoints;
        damage = PlayerStatStorage.inst.damage;
    }

    // Each loop hitpoints are stored to PlayerStatStorage
    void Update()
    {
        PlayerStatStorage.inst.hitpoints = hitpoints;
        PlayerStatStorage.inst.damage = damage;
    }
}
