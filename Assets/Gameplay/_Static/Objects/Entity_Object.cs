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
}
