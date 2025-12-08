using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIForwarder : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler
{
    [SerializeField] GameObject target;
    [SerializeField] ScrollRect targetScrollRect;
    [SerializeField] Slider targetSlider;

    Button localButton;
    ScrollRect localScrollRect;
    Slider localSlider;

    private void Start()
    {
        localButton = GetComponent<Button>();
        localScrollRect = GetComponent<ScrollRect>();
        localSlider = GetComponent<Slider>();
        if (localSlider != null && targetSlider != null)
        {
            localSlider.onValueChanged.AddListener((value) =>
            {
                targetSlider.value = value;
            });
        }
    }

    private void Update()
    {
        if (localScrollRect != null && targetScrollRect != null)
        {
            targetScrollRect.verticalNormalizedPosition = localScrollRect.verticalNormalizedPosition;
        }
    }

    private void Send<T>(BaseEventData data, ExecuteEvents.EventFunction<T> func)
        where T : IEventSystemHandler
    {
        ExecuteEvents.Execute(target, data, func);
    }

    public void OnPointerEnter(PointerEventData e) =>
        Send<IPointerEnterHandler>(e, ExecuteEvents.pointerEnterHandler);

    public void OnPointerExit(PointerEventData e) =>
        Send<IPointerExitHandler>(e, ExecuteEvents.pointerExitHandler);

    public void OnPointerDown(PointerEventData e) =>
        Send<IPointerDownHandler>(e, ExecuteEvents.pointerDownHandler);

    public void OnPointerUp(PointerEventData e) =>
        Send<IPointerUpHandler>(e, ExecuteEvents.pointerUpHandler);

    public void OnPointerClick(PointerEventData e) =>
        Send<IPointerClickHandler>(e, ExecuteEvents.pointerClickHandler);
}

