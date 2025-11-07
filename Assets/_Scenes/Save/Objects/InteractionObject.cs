using UnityEngine;
using UnityEngine.Events;


public class InteractionObject : MonoBehaviour
{

    public UnityEvent Interaction_ON_Function;
    public UnityEvent Interaction_OFF_Function;
    public UnityEvent Interaction_START_Function;



    public void Interaction_ON() {
        Interaction_ON_Function?.Invoke();
    }
    public void Interaction_OFF() {
        Interaction_OFF_Function?.Invoke();
    }
    public void Interaction_START() {
        Interaction_START_Function?.Invoke();
    }
}
