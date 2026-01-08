using UnityEngine;

public class PipeForwarder : MonoBehaviour
{

        PipeScript weapon;

        void Awake()
        {
            weapon = GetComponentInParent<PipeScript>();
        }


        public void OnAttackEvent()
        {
            weapon.Attack();
        }
    

}
