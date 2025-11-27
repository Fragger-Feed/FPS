using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIForwarder : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] GameObject target;

    Button localButton;

    private void Start()
    {
        localButton = GetComponent<Button>();
        if (localButton != null)
        {
            localButton.onClick.AddListener(OnClick);
        }
    }

    void OnClick()
    {
        target.GetComponent<Button>().onClick.Invoke();
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

    public void OnBeginDrag(PointerEventData e) =>
        Send<IBeginDragHandler>(e, ExecuteEvents.beginDragHandler);

    public void OnDrag(PointerEventData e) =>
        Send<IDragHandler>(e, ExecuteEvents.dragHandler);

    public void OnEndDrag(PointerEventData e) =>
        Send<IEndDragHandler>(e, ExecuteEvents.endDragHandler);
}

    /* [SerializeField] Canvas uiCanvas;
     [SerializeField] GraphicRaycaster raycaster;

     private void Awake()
     {
         raycaster = uiCanvas.GetComponent<GraphicRaycaster>();
     }

     public void OnPointerClick(PointerEventData eventData)
     {
         Debug.Log("Proxy canvas clicked at: " + eventData.position);
         ForwardEvent(eventData, ExecuteEvents.pointerClickHandler);
     }

     public void OnPointerDown(PointerEventData eventData)
     {
         print(1);
         ForwardEvent(eventData, ExecuteEvents.pointerDownHandler);
     }
     public void OnPointerUp(PointerEventData eventData)
     {
         ForwardEvent(eventData, ExecuteEvents.pointerUpHandler);
     }
     public void OnBeginDrag(PointerEventData eventData)
     {
         print(1);
         ForwardEvent(eventData, ExecuteEvents.beginDragHandler);
     }
     public void OnDrag(PointerEventData eventData)
     {
         print(1);
         ForwardEvent(eventData, ExecuteEvents.dragHandler);
     }
     public void OnEndDrag(PointerEventData eventData)
     {
         print(1);
         ForwardEvent(eventData, ExecuteEvents.endDragHandler);
     }
     public void OnScroll(PointerEventData eventData)
     {
         print(1);
         ForwardEvent(eventData, ExecuteEvents.scrollHandler);
     }


     private void ForwardEvent<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> func)
         where T : IEventSystemHandler
     {
         print(1);
         var results = new List<RaycastResult>();
         raycaster.Raycast(eventData, results);

         Debug.Log("Number of UI elements hit: " + results.Count);

         foreach (var r in results)
         {
             Debug.Log("Forwarding to: " + r.gameObject.name);
             ExecuteEvents.Execute(r.gameObject, eventData, func);
         }
     }
 */

