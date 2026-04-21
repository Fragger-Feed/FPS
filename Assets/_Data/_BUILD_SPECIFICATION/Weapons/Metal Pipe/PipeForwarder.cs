using UnityEngine;

public class PipeForwarder : MonoBehaviour
{
    private PipeScript weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<PipeScript>();
    }

    public void OnAttackEvent()
    {
        weapon.StartHitting();
    }
}