using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] TMP_Text text;
    [SerializeField] bool yes;
    Image img;

    public enum type { Bool, MultipleChoice, Slider}
    public type settingType;

    private void Start()
    {
        img = GetComponent<Image>();
        yes = true;
    }

    public void ChangeSetting()
    {
        yes = !yes;
        text.text = yes ? "< YES >" : "< NO >";
    }

    public void OnPointerEnter(PointerEventData e) =>
        img.color = new Color32(0, 0, 0, 252);

    public void OnPointerExit(PointerEventData e) =>
        img.color = new Color32(0, 0, 0, 0);

    public void OnPointerDown(PointerEventData e) =>
        img.color = new Color32(0, 0, 0, 255);

    public void OnPointerUp(PointerEventData e) =>
        img.color = new Color32(0, 0, 0, 252);
}
