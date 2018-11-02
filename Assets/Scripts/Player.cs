using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput; 

public class Player : MonoBehaviour {

	// Config
	[SerializeField] float runSpeed = 5f;
	[SerializeField] float jumpSpeed = 5f;
	[SerializeField] float climbSpeed = 5f;
	[SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

	// State
	bool isAlive = true;

	// Cached component references
	private Rigidbody2D myRigidBody;
	private Animator myAnimator;
	private CapsuleCollider2D myBodyCollider;
	private BoxCollider2D myFeetCollider;
	float gravityScaleAtStart;

	// Message then methods
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
		myBodyCollider = GetComponent<CapsuleCollider2D>();
		myFeetCollider = GetComponent<BoxCollider2D>();
	
		gravityScaleAtStart = myRigidBody.gravityScale;
	}


	// Update is called once per frame
	void Update () {
		if (!isAlive) { return; }
		Run();
		ClimbLadder();
		Jump();
		FlipSprite();
		Die();
	}


	private void Run() {
		float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1
		Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
		myRigidBody.velocity = playerVelocity;

		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
		myAnimator.SetBool("Running", playerHasHorizontalSpeed);
	}

	private void ClimbLadder() {
		if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) { 
			myAnimator.SetBool("Climbing", false);
			myRigidBody.gravityScale = gravityScaleAtStart;
			return; 
		}

		float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
		Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
		myRigidBody.velocity = climbVelocity;
		myRigidBody.gravityScale = 0f;
		bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
		myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
	}

	private void Jump() {
		if(!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

		if (CrossPlatformInputManager.GetButtonDown("Jump")) {
			Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
			myRigidBody.velocity += jumpVelocityToAdd;
		}
	}

	private void Die() {
		if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) {
			myAnimator.SetTrigger("Dying");
			GetComponent<Rigidbody2D>().velocity = deathKick;

			isAlive = false;
			FindObjectOfType<GameSession>().ProcessPlayerDeath();
		}
	}

	private void FlipSprite() {
		bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
		if (playerHasHorizontalSpeed) {
			transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
		}
	}
}
