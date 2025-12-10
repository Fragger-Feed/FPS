using UnityEngine;
using UnityEngine.Events;

public class Entity_Object : MonoBehaviour
{
    public int HealthPoints;

    public UnityEvent onSpawn;
    public UnityEvent onDeath;
    public UnityEvent onDamageGetting;

    public void Spawn() {

        //
        onSpawn?.Invoke();
    }


    public void Damage(int damage) {

        //
        print($"took {damage} damage");
        HealthPoints -= damage;
        if(HealthPoints <= 0) Death();
        onDamageGetting?.Invoke();
    }


    public void Death() {

        //
        print($"I'm object {gameObject} and I'm dead");
        HealthPoints = 100;
        onDeath?.Invoke();
    }
}
