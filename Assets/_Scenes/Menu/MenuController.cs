using UnityEngine;

public class MenuController : MonoBehaviour
{
    public string CurrentMenuID = "Basic";

    public GameObject BasicMenu;
    public GameObject Save1Menu;
    public GameObject Save2Menu;
    public GameObject Save3Menu;
    public GameObject SettingsMenu;
    public GameObject QuitMenu;

    public void TryToOpenMenu(string menuName) {
        BasicMenu.SetActive(false);
        Save1Menu.SetActive(false);
        Save2Menu.SetActive(false);
        Save3Menu.SetActive(false);
        SettingsMenu.SetActive(false);
        QuitMenu.SetActive(false);

        if (menuName == "Basic") { BasicMenu.SetActive(true); }
        if (menuName == "Save1") { Save1Menu.SetActive(true); }
        if (menuName == "Save2") { Save2Menu.SetActive(true); }
        if (menuName == "Save3") { Save3Menu.SetActive(true); }
        if (menuName == "Settings") { SettingsMenu.SetActive(true); }
        if (menuName == "Quit") { QuitMenu.SetActive(true); }
    }
}
