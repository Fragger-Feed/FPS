using UnityEngine;
using UnityEngine.InputSystem;

public class BoxButton : MonoBehaviour
{
    // == Data ==
    public MenuController menuController;

    // == Configs ==
    public string ID;
    public Color highlightColor = Color.yellow;

    // == Hash ==
    private Color originalColor;
    private Renderer rend;



    // == Start ==
    void Start() {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }



    // == Update ==
    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject == gameObject) {
                rend.material.color = highlightColor;
                if (Mouse.current.leftButton.wasPressedThisFrame) {
                    menuController.TryToOpenMenu(ID);
                }
            }
            else { rend.material.color = originalColor; }
        }
        else { rend.material.color = originalColor; }
    }



    // == Functions ==
    // Quit
    private void Quit() {
        
    }

    // Settings
    private void Settings() {

    }

    // Load Save N
    private void LoadSave(int N) {

    }
}
