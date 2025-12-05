using System.Collections.Generic;
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

    public List<Vector2Int> resolutionOptions = new List<Vector2Int>();
    public int currentResolutionIndex = 0;

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

    // public void CycleResolution()
    // {
    //     if (resolutionOptions == null || resolutionOptions.Count == 0) return;

    //     currentResolutionIndex = (currentResolutionIndex + 1) % resolutionOptions.Count;
    //     Vector2Int res = resolutionOptions[currentResolutionIndex];
    //     text.text = $"< {res.x} x {res.y} >";
    //     Screen.SetResolution(res.x, res.y, Screen.fullScreen);
    // }

    public void CycleResolution(int a)
    {
        currentResolutionIndex = (currentResolutionIndex + a + resolutionOptions.Count) % resolutionOptions.Count;
        text.text = $"<{resolutionOptions[currentResolutionIndex].x}x{resolutionOptions[currentResolutionIndex].y}>";
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
