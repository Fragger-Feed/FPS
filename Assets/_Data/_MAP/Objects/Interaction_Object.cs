using UnityEngine;
using UnityEngine.Events;

public class InteractionObject : MonoBehaviour
{
    // == View ==
    [Header("View")]
    public UnityEvent Interaction_LOOK_ON_Function;

    public UnityEvent Interaction_LOOK_OFF_Function;

    // == Interact ==
    [Header("Interact")]
    public UnityEvent Interaction_START_Function;

    public UnityEvent Interaction_HOLDING_Function;
    public UnityEvent Interaction_END_Function;
    public UnityEvent<float> Interaction_HOLDING_TIME_Function;

    // == View ==
    public void Interaction_LOOK_ON()
    { Interaction_LOOK_ON_Function?.Invoke(); }

    public void Interaction_LOOK_OFF()
    { Interaction_LOOK_OFF_Function?.Invoke(); }

    // == Interact ==
    public void Interaction_START()
    { Interaction_START_Function?.Invoke(); }

    public void Interaction_HOLDING()
    { Interaction_HOLDING_Function?.Invoke(); }

    public void Interaction_END()
    { Interaction_END_Function?.Invoke(); }

    public void Interaction_HOLDING_TIME(float deltaTime)
    { Interaction_HOLDING_TIME_Function?.Invoke(deltaTime); }
}