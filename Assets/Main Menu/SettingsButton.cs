using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHovered;
    Outline outline;

    private void Start()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        Debug.Log("Hover start");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        Debug.Log("Hover end");
    }
}
