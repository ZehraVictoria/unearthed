using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//uses collider to check direction to see of obj is on ground, ceiling or touching the wall
public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D touchingCol;
    Animator animator;

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallCheckDistance = 0.2f;
    public float ceilingCheckDistance = 0.05f;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private Vector2 wallCheckDir => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    [SerializeField] //viewable in inspector
    private bool _isGrounded = true;

    public bool isGrounded { get
        {
            return _isGrounded;
        } private set
        {
            _isGrounded = value;
            animator.SetBool("isGrounded", value);
        } }

    [SerializeField] //viewable in inspector
    private bool _isOnWall = true;

    public bool isOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            animator.SetBool("isOnWall", value);
        }
    }

    [SerializeField] //viewable in inspector
    private bool _isOnCeiling = true;

    public bool isOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool("isOnCeiling", value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        isGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        isOnWall = touchingCol.Cast(wallCheckDir, castFilter, wallHits, wallCheckDistance) > 0;
        isOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingCheckDistance) > 0;
    }
}
