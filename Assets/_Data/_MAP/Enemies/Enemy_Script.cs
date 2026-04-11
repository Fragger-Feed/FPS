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

    public virtual void Awake()
    {
        canMove = true;
        state = GetComponent<EntityStates>();
        nav = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerController>();

        nav.acceleration = data.Acceleration;
        nav.speed = data.Speed;
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
    }

    public virtual void Attack()
    {
    }

    public virtual void Stunned()
    {
    }

    private void SwitchCanMove()
    {
        canMove = !canMove;
    }
}