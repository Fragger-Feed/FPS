using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public abstract class EnemieScript : MonoBehaviour
{
    public Enemie_Data data;
    public PlayerController player;
    public NavMeshAgent nav;
    public Animator animator;

    private EntityStates state;

    public float cooldownTimer = 0f;

    [SerializeField] private bool canMove = true;

    public enum EnemieState
    {
        Idle,
        Move,
        Attack,
        Stunned,
        Dead
    }

    public EnemieState currentState = EnemieState.Idle;

    protected virtual void Awake()
    {
        canMove = true;
        state = GetComponent<EntityStates>();
        nav = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerController>();

        nav.acceleration = data.acceleration;
        nav.speed = data.speed;
        nav.stoppingDistance = data.attackRange;
    }

    public virtual void Update()
    {
        switch (currentState)
        {
            case EnemieState.Idle:
                Idle();
                break;

            case EnemieState.Move:
                Move();
                break;

            case EnemieState.Attack:
                Attack();
                break;

            case EnemieState.Stunned:
                Stunned();
                break;

            case EnemieState.Dead:
                break;
        }
    }

    public virtual void Idle()
    {
        if (player.ControllingData.Player.Interact.WasPressedThisFrame())
        {
            currentState = EnemieState.Move;
        }
    }

    public virtual void Move()
    {
        if (!canMove)
            return;
        if (Vector3.Distance(transform.position, player.transform.position) <= data.attackRange)
        {
            currentState = EnemieState.Attack;
            return;
        }
        animator.SetTrigger("Move");
        animator.ResetTrigger("Attack");
    }

    public virtual void Attack()
    {
        animator.ResetTrigger("Move");
        animator.SetTrigger("Attack");
    }

    public virtual void Stunned()
    {
    }

    private void SwitchCanMove()
    {
        canMove = !canMove;
    }
}