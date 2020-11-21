using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput2D : MonoBehaviour
{
    private PlayerStatController2D stat_controller;
    private CharacterController2D controller;
    private Animator animator;
    public float jumping_force = 1000.0f;
    public float movement_speed = 8.0f;
    private float horizontal_input = 0.0f;
    private bool jumping = false;
    private float hitRadius = 1.5f;
    private bool cooldown = false;
    private float cooldownTimer = 0.0f;

    void Start()
    {
        stat_controller = GetComponent<PlayerStatController2D>();
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (cooldown)
        {
            if (cooldownTimer < 0)
            {
                cooldown = false;
            } else {
                cooldownTimer -= Time.deltaTime;
            }
        }

        // Exit game
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        // Horizontal movement
        horizontal_input = Input.GetAxisRaw("Horizontal");

        // Attack 1
        if (Input.GetButtonDown("Attack1")) 
        {
            if (!cooldown)
            {
                cooldown = true;
                cooldownTimer = 0.6f;
                PlayerAttack();
                animator.SetBool("PlayerAttack1", true);
            }
        } else {
            animator.SetBool("PlayerAttack1", false);
        }

        // Attack 2
        if (Input.GetButtonDown("Attack2"))
        {
            if (!cooldown)
            {
                cooldown = true;
                cooldownTimer = 0.6f;
                animator.SetBool("PlayerAttack2", true);
                PlayerAttack();
            }
        } else {
            animator.SetBool("PlayerAttack2", false);
        }

        // jumping
        jumping = Input.GetButtonDown("Jump");

        // give inputs to CharacterController2D
        controller.ExecuteMoving(horizontal_input, movement_speed, jumping, jumping_force);
    }
    void PlayerAttack()
    {
        Vector2 origin = transform.position;
        Vector2 offset_without_direction = new Vector2(1.8f, 0);
        Vector2 offset = controller.getCharacterDirection() ? offset_without_direction : -1 * offset_without_direction;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(origin + offset, hitRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Enemy")
            {
                hitCollider.SendMessage("TakeDamage", stat_controller.damage);
            }
        }
    }

    void PlayerTakeDamage(float damage)
    {
        stat_controller.hitpoints -= damage;

        if (stat_controller.hitpoints <= 0)
        {
            Destroy(gameObject);
            // TODO: Add game over scene
        }
    }
}
