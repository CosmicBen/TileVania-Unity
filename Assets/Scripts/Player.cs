using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private const string GROUND_LAYER = "Ground";
    private const string CLIMBABLE_LAYER = "Climbable";
    private const string ENEMY_LAYER = "Enemy";
    private const string HAZARDS_LAYER = "Hazards";

    private const string CLIMBING_ANIMATOR = "Climbing";
    private const string RUNNING_ANIMATOR = "Running";
    private const string DYING_ANIMATOR = "Dying";

    private const string VERTICAL_AXIS = "Vertical";
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string JUMP_BUTTON = "Jump";

    [Header("Configuration")]
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float climbSpeed = 5.0f;

    [SerializeField] private Vector2 deathKick = new Vector2(5.0f, 10.0f);

    // State
    private bool isAlive = true;
    private float gravityScaleAtStart = 0.0f;

    // Cached component references
    private Rigidbody2D myRigidbody = null;
    private Animator myAnimator = null;
    private CapsuleCollider2D myBodyCollider = null;
    private BoxCollider2D myGroundCollider = null;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myGroundCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();

        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    private void Update()
    {
        if (!isAlive) { return; }

        Run();
        Jump();
        Climb();
        Die();
        FlipSprite();
    }

    private void Climb()
    {
        if (!myGroundCollider.IsTouchingLayers(LayerMask.GetMask(CLIMBABLE_LAYER)))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool(CLIMBING_ANIMATOR, false);
            return;
        }

        float controlThrow = Input.GetAxis(VERTICAL_AXIS);
        Vector2 playerVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * climbSpeed);
        myRigidbody.velocity = playerVelocity;
        myRigidbody.gravityScale = 0.0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool(CLIMBING_ANIMATOR, playerHasVerticalSpeed);
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis(HORIZONTAL_AXIS);
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool(RUNNING_ANIMATOR, playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!myGroundCollider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER))) { return; }

        if (Input.GetButtonDown(JUMP_BUTTON))
        {
            Vector2 playerVelocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
            myRigidbody.velocity = playerVelocity;
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask(ENEMY_LAYER, HAZARDS_LAYER)))
        {
            isAlive = false;

            myAnimator.SetTrigger(DYING_ANIMATOR);
            myRigidbody.velocity = deathKick;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1.0f);
        }
    }
}
