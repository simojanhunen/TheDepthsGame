using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
	public float movementSmoothing = 0.1f;
	public bool enableControlMidair = true;
	public LayerMask groundLayer;
	public Transform checkGroundUsing;
	public Transform player;
	public Animator anim;
	public long waitTimeToGroundedMs = 150;
	private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
	private float checkIfGroundedRadius = 0.2f;
	private bool playerIsGrounded;
	private bool playerFacingRight = true;
	private Rigidbody2D playerRigidbody;
	private Vector3 velocity = Vector3.zero;
	private float latestMoveCommand = 0.0f;
	private bool latestJumpCommand = false;
	private CharacterInput2D characterInput2D;
    public Text hitpointsBar;

	private void Awake()
	{
		characterInput2D = GetComponent<CharacterInput2D>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Transform>();
	}

	private void Update()
	{
		// update hp text
		string fullHp = PlayerStatStorage.inst.maximumHitpoints.ToString();
		string currentHp = PlayerStatStorage.inst.hitpoints.ToString();
		hitpointsBar.text = "HP " + currentHp + "/" + fullHp;

		// saturate movement speed
		if (playerRigidbody.velocity.x > characterInput2D.movement_speed)
		{
			Vector3 targetVelocity 	= new Vector2(characterInput2D.movement_speed, playerRigidbody.velocity.y);
			playerRigidbody.velocity 	= targetVelocity;
		}
		else if (playerRigidbody.velocity.x < -characterInput2D.movement_speed)
		{
			Vector3 targetVelocity 	= new Vector2(-characterInput2D.movement_speed, playerRigidbody.velocity.y);
			playerRigidbody.velocity 	= targetVelocity;
		}
	}

	private void FixedUpdate()
	{
		// Start stopwatch if player has jumped
		if (!playerIsGrounded)
		{
			stopwatch.Start();
		}

		anim.SetFloat("PlayerHorizontalSpeed", Mathf.Abs(playerRigidbody.velocity.x));
		anim.SetFloat("PlayerVerticalSpeed", playerRigidbody.velocity.y);

		bool wasGrounded = playerIsGrounded;
		playerIsGrounded = false;

		// check overlap between ground_layer and check_ground_using transform using check_if_grounded_radius
		Collider2D[] colliders = Physics2D.OverlapCircleAll(checkGroundUsing.position, checkIfGroundedRadius, groundLayer);

		// loop through all the found colliders
		for (int i = 0; i < colliders.Length; i++)
		{
			// if found object is not the player (gameObject) -> we're grounded
			if (colliders[i].gameObject != gameObject)
			{
				playerIsGrounded = true;

				// If parametrized time has passed, turn animation off
				if (stopwatch.ElapsedMilliseconds > waitTimeToGroundedMs)
				{
					anim.SetBool("PlayerJumping", false);
					stopwatch.Reset();
				}
			}
		}
	}

	public void ExecuteMoving(float move_command, float movement_speed, bool jumping, float jumping_force)
	{
		// save new command as the latest
		latestMoveCommand = move_command;
		latestJumpCommand = jumping;

		// if grounded and input is to jump
		if (playerIsGrounded && jumping)
		{
			playerIsGrounded = false;
			anim.SetBool("PlayerJumping", true);
			playerRigidbody.AddForce(new Vector2(0.0f, jumping_force));
		}

		// set new velocity
		if (playerIsGrounded || enableControlMidair)
		{
			Vector3 new_velocity = new Vector2(move_command * movement_speed, playerRigidbody.velocity.y);
			playerRigidbody.velocity = Vector3.SmoothDamp(playerRigidbody.velocity, new_velocity, ref velocity, movementSmoothing);

			if (move_command > 0 && !playerFacingRight)
			{
				FlipXOnCharacterSprite();
			}
			else if (move_command < 0 && playerFacingRight)
			{
				FlipXOnCharacterSprite();
			}
		}
	}

	private void FlipXOnCharacterSprite()
	{
		playerFacingRight = !playerFacingRight;
		Vector3 new_scale = transform.localScale;
		new_scale.x *= -1;
		transform.localScale = new_scale;
	}
	
	public bool getCharacterDirection()
	{
		return playerFacingRight;
	}
}
