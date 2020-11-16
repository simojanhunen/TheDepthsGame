using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
	// inputs for script
	public float movement_smoothing = 0.1f;
	public bool enable_controlling_midair = true;
	public LayerMask ground_layer;
	public Transform check_ground_using;
	public Transform player;
	public Animator anim;
	public long wait_time_to_grounded_ms = 150;

	// inner implementation
	private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
	private float check_if_grounded_radius = 0.2f;
	private bool player_is_grounded;
	private bool player_facing_right = true;
	private Rigidbody2D player_rigidbody;
	private Vector3 velocity = Vector3.zero;
	private float latest_move_command = 0.0f;
	private bool latest_jump_command = false;
	private CharacterInput2D character_input2D;
    public Text hitpointsBar;

	private void Awake()
	{
		character_input2D 	= GetComponent<CharacterInput2D>();
		player_rigidbody 	= GetComponent<Rigidbody2D>();
		player 				= GetComponent<Transform>();
	}

	private void Update()
	{
		// update hp text
		string fullHp = PlayerStatStorage.inst.maximumHitpoints.ToString();
		string currentHp = PlayerStatStorage.inst.hitpoints.ToString();
		hitpointsBar.text = "HP " + currentHp + "/" + fullHp;

		// saturate movement speed
		if (player_rigidbody.velocity.x > character_input2D.movement_speed)
		{
			Vector3 targetVelocity 	= new Vector2(character_input2D.movement_speed, player_rigidbody.velocity.y);
			player_rigidbody.velocity 	= targetVelocity;
		}
		else if (player_rigidbody.velocity.x < -character_input2D.movement_speed)
		{
			Vector3 targetVelocity 	= new Vector2(-character_input2D.movement_speed, player_rigidbody.velocity.y);
			player_rigidbody.velocity 	= targetVelocity;
		}
	}

	private void FixedUpdate()
	{
		// Start stopwatch if player has jumped
		if (!player_is_grounded)
		{
			stopwatch.Start();
		}

		anim.SetFloat("PlayerHorizontalSpeed", Mathf.Abs(player_rigidbody.velocity.x));
		anim.SetFloat("PlayerVerticalSpeed", player_rigidbody.velocity.y);

		bool wasGrounded = player_is_grounded;
		player_is_grounded = false;

		// check overlap between ground_layer and check_ground_using transform using check_if_grounded_radius
		Collider2D[] colliders = Physics2D.OverlapCircleAll(check_ground_using.position, check_if_grounded_radius, ground_layer);

		// loop through all the found colliders
		for (int i = 0; i < colliders.Length; i++)
		{
			// if found object is not the player (gameObject) -> we're grounded
			if (colliders[i].gameObject != gameObject)
			{
				player_is_grounded = true;

				// If parametrized time has passed, turn animation off
				if (stopwatch.ElapsedMilliseconds > wait_time_to_grounded_ms)
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
		latest_move_command = move_command;
		latest_jump_command = jumping;

		// if grounded and input is to jump
		if (player_is_grounded && jumping)
		{
			player_is_grounded = false;
			anim.SetBool("PlayerJumping", true);
			player_rigidbody.AddForce(new Vector2(0.0f, jumping_force));
		}

		// set new velocity
		if (player_is_grounded || enable_controlling_midair)
		{
			Vector3 new_velocity = new Vector2(move_command * movement_speed, player_rigidbody.velocity.y);
			player_rigidbody.velocity = Vector3.SmoothDamp(player_rigidbody.velocity, new_velocity, ref velocity, movement_smoothing);

			if (move_command > 0 && !player_facing_right)
			{
				FlipXOnCharacterSprite();
			}
			else if (move_command < 0 && player_facing_right)
			{
				FlipXOnCharacterSprite();
			}
		}
	}

	private void FlipXOnCharacterSprite()
	{
		player_facing_right = !player_facing_right;
		Vector3 new_scale = transform.localScale;
		new_scale.x *= -1;
		transform.localScale = new_scale;
	}
}
