using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2D : MonoBehaviour
{
    public float monsterHitpoints = 5;
    public float monsterDamage = 1;
    public Animator animator;
    private bool playerIsColliding = false;
    private float timer = 0.0f;
    private float timeToDoDamage = 0.35f;
    private float cooldownTime = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsColliding)
        {
            timer -= Time.deltaTime;
        }
    }

    void TakeDamage(float damage)
    {
        monsterHitpoints -= damage;

        if (monsterHitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player")
        {
            playerIsColliding = true;
            animator.SetTrigger("MushroomAttack1");
            timer = timeToDoDamage;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.name == "Player" && playerIsColliding == true)
        {
            if(timer < 0)
            {
                collider.gameObject.SendMessage("PlayerTakeDamage", monsterDamage);
                timer = cooldownTime;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.name == "Player")
        {
            playerIsColliding = false;
            timer = timeToDoDamage;
        }
    }
}
