using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
        Stunned,
        Dead
    }

    public EnemieState currentState = EnemieState.Idle;

    protected virtual void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        player = FindFirstObjectByType<PlayerController>();
        currentHealth = data.health;

        nav.acceleration = data.acceleration;
        nav.speed = data.speed;
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

    }
    public virtual void Attack()
    {

    }
    public virtual void Stunned()
    {

    }

    public virtual void Damage(int damage)
    {
        print($"took {damage} damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentState = EnemieState.Dead;
            Die();
        }
    }


    public void Die()
    {
        print($"I'm object {gameObject} and I'm dead");
        nav.enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 11241;
        transform.Rotate(90, 90, 90);
    }
}
