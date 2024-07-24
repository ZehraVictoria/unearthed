using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent<int, int> healthChanged;

    Animator animator;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

            //if health drops below 0, character death
            if (_health <= 0)
            {
                IsAlive = false;
                Debug.Log("Character died");
            }
        }
    }


    [SerializeField]
    private bool isInvincible = false;

    [SerializeField]
    private bool _isAlive;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool("isAlive", value);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool Hit (int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            // Notify subscribed comps that damageable was hit, to handle knockback etc.
            animator.SetTrigger("hit");
            damageableHit?.Invoke(damage, knockback);

            return true;
        }
        //unable to hit
        return false;
    }
    private float timeSinceHit = 0;
    public float invincTime = 0.25f;

    public void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > invincTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
}
