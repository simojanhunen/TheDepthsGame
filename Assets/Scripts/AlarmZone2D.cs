using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmZone2D : MonoBehaviour
{
    public float hitRadius = 5.0f;
    private EnemyController2D controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponentInParent<EnemyController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, hitRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Player")
            {
                if ((hitCollider.transform.position.x - gameObject.transform.position.x) > 0.0f)
                {
                    controller.ExecuteMoving(true, 1.0f);
                } else {
                    controller.ExecuteMoving(true, -1.0f);
                }
                return;
            }
        }
        controller.ExecuteMoving(false, 0.0f);
    }
}
