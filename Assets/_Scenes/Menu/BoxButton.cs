using UnityEngine;
using UnityEngine.InputSystem;

public class BoxButton : MonoBehaviour
{
    // == UI ==
    public GameObject Save1Menu;
    public GameObject Save2Menu;
    public GameObject Save3Menu;
    public GameObject SettingsMenu;
    public GameObject QuitMenu;

    // == Configs ==
    public int ID;
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
                    if (ID == 0) {
                        QuitMenu.SetActive(false);
                        SettingsMenu.SetActive(false);
                        Save1Menu.SetActive(true);
                        Save2Menu.SetActive(false);
                        Save3Menu.SetActive(false);
                    }
                    if(ID == 1) {
                        QuitMenu.SetActive(false);
                        SettingsMenu.SetActive(false);
                        Save1Menu.SetActive(false);
                        Save2Menu.SetActive(true);
                        Save3Menu.SetActive(false);
                    }
                    if (ID == 2) {
                        QuitMenu.SetActive(false);
                        SettingsMenu.SetActive(false);
                        Save1Menu.SetActive(false);
                        Save2Menu.SetActive(false);
                        Save3Menu.SetActive(true);
                    }
                    if (ID == 3) {
                        QuitMenu.SetActive(false);
                        SettingsMenu.SetActive(true);
                        Save1Menu.SetActive(false);
                        Save2Menu.SetActive(false);
                        Save3Menu.SetActive(false);
                    }
                    if (ID == 4) {
                        QuitMenu.SetActive(true);
                        SettingsMenu.SetActive(false);
                        Save1Menu.SetActive(false);
                        Save2Menu.SetActive(false);
                        Save3Menu.SetActive(false);
                    }
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
