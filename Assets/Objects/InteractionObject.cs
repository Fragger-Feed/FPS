using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public delegate void fun();
    public fun Interaction_ON_Function;
    public fun Interaction_OFF_Function;
    public fun Interaction_START_Function;
    public bool interactionAbility = true;




    void Interaction_ON() {
        print(gameObject.name + " interaction on!");
    }
    void Interaction_OFF() {
        print(gameObject.name + " interaction off!");
    }
    void Interaction_START() {
        print(gameObject.name + " interaction start!");
        interactionAbility = false;
    }



    public void Start()
    {
        Interaction_ON_Function = Interaction_ON;
        Interaction_OFF_Function = Interaction_OFF;
        Interaction_START_Function = Interaction_START;
    }


}
