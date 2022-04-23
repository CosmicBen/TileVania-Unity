using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;

    private Animator myAnimator = null;
    private Rigidbody2D myRigidbody = null;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (IsFacingRight())
        {
            myRigidbody.velocity = new Vector2(moveSpeed, 0.0f);
        }
        else
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, 0.0f);
        }
    }

    private void OnTriggerExit2D()
    {
        transform.localScale = new Vector2(-Mathf.Sign(myRigidbody.velocity.x), 1.0f);
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
}
