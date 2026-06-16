using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class Enemy_Script : MonoBehaviour
{
    public Enemy_DataScript data;
    public PlayerController player;
    public NavMeshAgent nav;
    public Animator animator;

    public EntityStates state;

    public float cooldownTimer = 0f;
    public float stunTimer;
    public float lastPlayerAttackTime;

    public bool attacking;
    public bool canBeParried;
    public bool alreadyDamaged;

    public bool canMove = true;

    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Stunned,
        Dead
    }

    public EnemyState currentState = EnemyState.Idle;

    public virtual void Awake()
    {
        canMove = true;
        state = GetComponent<EntityStates>();
        nav = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerController>();

        if (nav != null)
        {
            nav.acceleration = data.Acceleration;
            nav.speed = data.Speed;
        }
    }

    public virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Move:
                Move();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Stunned:
                Stunned();
                break;

            case EnemyState.Dead:
                break;
        }
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void Idle()
    {
        if (player.ControllingData.Player.Interact.WasPressedThisFrame())
        {
            currentState = EnemyState.Move;
        }
    }

    public virtual void Move()
    {
        if (!canMove)
            return;
    }

    public virtual void Attack()
    {
    }

    public virtual void StartDamaging()
    {
        attacking = true;
        alreadyDamaged = false;
    }

    public virtual void StopDamaging()
    {
        attacking = false;
        alreadyDamaged = false;
        canBeParried = false;
    }

    public virtual void OpenParryWindow()
    {
        canBeParried = true;
        if (state.lastHitBy != null && Time.time - lastPlayerAttackTime < state.lastHitBy.data.hitTime)
        {
            GetParried();
        }
    }

    public virtual void CloseParryWindow()
    {
        canBeParried = false;
    }

    public virtual void GetParried()
    {
    }

    public virtual void Stunned()
    {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
        }
        else
        {
            animator.SetBool("Stunned", false);
            currentState = EnemyState.Move;
        }
    }

    private void SwitchCanMove()
    {
        canMove = !canMove;
    }
}