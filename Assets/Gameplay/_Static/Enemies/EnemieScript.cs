using UnityEngine;
using UnityEngine.AI;

public abstract class EnemieScript : MonoBehaviour
{
    public Enemie_Data data;

    public int currentHealth;

    public PlayerController player;

    public NavMeshAgent nav;
    public enum EnemieState
    {
        Idle,
        Move,
        Attack,
        Stunned
    }

    public EnemieState currentState = EnemieState.Idle;

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerController>();
        currentHealth = data.health;
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

    }
    public virtual void Attack()
    {

    }
    public virtual void Stunned()
    {

    }

}
