using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatStorage : MonoBehaviour
{
    public static PlayerStatStorage inst;
    public float maximumHitpoints = 10;
    public float hitpoints = 10;
    public float damage = 1;

    // Start is called before the first frame update
    void Awake()
    {
        if (inst == null)
        {
            DontDestroyOnLoad(gameObject);
            inst = this;
        }
        else if (inst != this)
        {
            Destroy(gameObject);
        }
    }
}
