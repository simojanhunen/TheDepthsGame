using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController2D : MonoBehaviour
{
    public float monsterHitpoints = 5;
    public float monsterDamage = 1;
    public float movementSpeed = 3.0f;
    private Animator animator;
    private Rigidbody2D monsterRigidbody;
    private bool playerIsColliding = false;
    private float timer = 0.0f;
    private float timeToDoDamage = 0.35f;
    private float cooldownTime = 2.0f;
    private Vector3 zeroVelocity = Vector3.zero;
    private float movementSmoothing = 0.1f;
    private bool monsterFacingRight = true;
    private Collider2D monsterCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        monsterCollider = gameObject.GetComponent<CapsuleCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        monsterRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("MushroomSpeed", Mathf.Abs(monsterRigidbody.velocity.x));

        if (playerIsColliding)
        {
            timer -= Time.deltaTime;
        }
    }

    // true = right, false = left
    public void ExecuteMoving(bool enable, float moveCommand)
    {
        if (enable)
        {
            monsterRigidbody.constraints = RigidbodyConstraints2D.None;
            monsterRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            Vector3 new_velocity = new Vector2(moveCommand * movementSpeed, monsterRigidbody.velocity.y);
            monsterRigidbody.velocity = Vector3.SmoothDamp(monsterRigidbody.velocity, new_velocity, ref zeroVelocity, movementSmoothing);

            if (moveCommand > 0 && !monsterFacingRight)
            {
                FlipXOnMonsterSprite();
            }
            else if (moveCommand < 0 && monsterFacingRight)
            {
                FlipXOnMonsterSprite();
            }
        } else {
            monsterRigidbody.velocity = zeroVelocity;
            monsterRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

	private void FlipXOnMonsterSprite()
	{
		monsterFacingRight = !monsterFacingRight;
		Vector3 new_scale = transform.localScale;
		new_scale.x *= -1;
		transform.localScale = new_scale;
	}

    void TakeDamage(float damage)
    {
        animator.SetTrigger("MushroomTakeHit");
        monsterHitpoints -= damage;

        if (monsterHitpoints <= 0)
        {
            animator.SetTrigger("MushroomDeath");
            if (gameObject.name == "GiantMushroom")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            Destroy(gameObject, 0.6f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (monsterCollider.IsTouching(collider) && collider.name == "Player")
        {
            playerIsColliding = true;
            animator.SetTrigger("MushroomAttack1");
            timer = timeToDoDamage;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(monsterCollider.IsTouching(collider) && collider.name == "Player" && playerIsColliding == true)
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
        if(monsterCollider.IsTouching(collider) && collider.name == "Player")
        {
            playerIsColliding = false;
            timer = timeToDoDamage;
        }
    }
}
