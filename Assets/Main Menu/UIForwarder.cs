using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIForwarder : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    [SerializeField] Canvas uiCanvas;
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
        ForwardEvent(eventData, ExecuteEvents.pointerDownHandler);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ForwardEvent(eventData, ExecuteEvents.pointerUpHandler);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ForwardEvent(eventData, ExecuteEvents.beginDragHandler);
    }
    public void OnDrag(PointerEventData eventData)
    {
        ForwardEvent(eventData, ExecuteEvents.dragHandler);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        ForwardEvent(eventData, ExecuteEvents.endDragHandler);
    }
    public void OnScroll(PointerEventData eventData)
    {
        ForwardEvent(eventData, ExecuteEvents.scrollHandler);
    }


    private void ForwardEvent<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> func)
        where T : IEventSystemHandler
    {
        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        Debug.Log("Number of UI elements hit: " + results.Count);

        foreach (var r in results)
        {
            Debug.Log("Forwarding to: " + r.gameObject.name);
            ExecuteEvents.Execute(r.gameObject, eventData, func);
        }
    }

}
