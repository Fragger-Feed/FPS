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
    public Color originalColor;
    public Renderer rend;

    public MenuController.states BoxState;







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
