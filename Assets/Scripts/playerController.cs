using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))] //safety
public class playerController : MonoBehaviour
{
    Vector2 moveInput;
    public KeyCode specialKey = KeyCode.Q;
    public float walkSpeed = 10f;
    public float jumpImpulse = 9f;
    public gameOver gameIsOver;
    TouchingDirections touchingDirections;
    private bool isInCombat = false;

    public float CurrentMoveSpeed { get
        {
            if (canMove)
            {
                if (IsMoving && !touchingDirections.isOnWall) 
                {
                    return walkSpeed;
                }
                else
                {
                    return 0; // idle is 0
                }
            }
            else
            {
                return 0; //movement locked
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }

    }

    public bool _isFacingRight = true;
    public bool isFacingRight { get { return _isFacingRight; } private set {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1); //reverse direction
            }
            _isFacingRight = value;
        }
    }

    public bool canMove { get
        {
            return animator.GetBool("canMove");
        } }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool("isAlive");
        }
    }

    public bool reachedFinish
    {
        get
        {
            return animator.GetBool("reachedFinish");
        }
    }

    public bool lockVelocity { get
        {
            return animator.GetBool("lockVelocity");
        }
        set
        {
            animator.SetBool("lockVelocity", value);
        }
    }

    public bool isComboReady
    {
        get
        {
            return animator.GetBool("isComboReady");
        }
        set
        {
            animator.SetBool("isComboReady", value);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake() //before start
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Start()
    {
        gameIsOver = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameOver>();
    }

    private void Update()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (!isComboReady && Input.GetKeyDown(specialKey))
        {
            isComboReady = true;
        }

        if (!IsAlive)
        {
            gameIsOver.gameLost();
        }

        if (reachedFinish)
        {
            gameIsOver.gameWon();
        }
    }
    private void FixedUpdate()
    {
        if (!lockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if ( moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        } else
        {
            IsMoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && canMove && !isInCombat) 
        {
            animator.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger("attack");

        }
    }

    public void OnComboAttack()
    {
        if (isComboReady)
        {
            animator.SetTrigger("comboAttack");
            isComboReady = false;
        }
    }

    public void OnRangedAttack()
    {
            animator.SetTrigger("rangedAttack");
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        lockVelocity = true;
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            gameIsOver.gameWon();
        }
    }

   private void combatActive()
    {
        isInCombat = true;
    }
    private void combatInactive()
    {
        isInCombat = false;
    }
}
