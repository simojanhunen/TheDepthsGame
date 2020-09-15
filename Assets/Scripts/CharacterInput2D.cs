using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput2D : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator anim;
    public float jumping_force = 1000.0f;
    public float movement_speed = 8.0f;
    private float horizontal_input = 0.0f;
    private bool jumping = false;

    void Update()
    {
        // horizontal movement
        horizontal_input = Input.GetAxisRaw("Horizontal");

        // attack 1
        bool isAttack1 = Input.GetButtonDown("Attack1");
        if (isAttack1) anim.SetBool("PlayerAttack1", true);
        else anim.SetBool("PlayerAttack1", false);

        // attack 2
        bool isAttack2 = Input.GetButtonDown("Attack2");
        if (isAttack2) anim.SetBool("PlayerAttack2", true);
        else anim.SetBool("PlayerAttack2", false);

        // jumping
        jumping = Input.GetButtonDown("Jump");

        // give inputs to CharacterController2D
        controller.ExecuteMoving(horizontal_input, movement_speed, jumping, jumping_force);
    }

}
