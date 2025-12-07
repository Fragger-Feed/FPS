using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    // == Additional ==
    public enum MenuStates { 
        MainView,
        TV,
        Cassettes,
        Table,

        Game, 
        Settings, 
        LevelSelect, 
        Quit, 
        Idle 
    };




    // Controlling 
    Controlling_Data controllingData;
    private void Awake() { controllingData = new Controlling_Data(); }
    private void OnEnable() { controllingData.Enable(); }




    // Current State
    public MenuStates CurrentMenuState = MenuStates.Idle;
    public void ChangeMenuState(MenuStates state) {
        CurrentMenuState = state;

        switch (CurrentMenuState) {

            case MenuStates.MainView:
                break;

            case MenuStates.TV:
                break;

            case MenuStates.Cassettes:
                break;

            case MenuStates.Table:
                break;

        }
    }






    public string CurrentMenuID = "Basic";

    public List<GameObject> gameObjects = new List<GameObject>();
    public List<MenuPair> menus = new List<MenuPair>();


    public MenuStates state = MenuStates.Idle;
    BoxButton box;

    public Animator animator;


    RaycastHit hit;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Physics.Raycast(ray, out hit);
        switch (state)
        {
            case MenuStates.Game:
                //print("Game");
                break;
            case MenuStates.Settings:
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Close");
                menus[0].SetActive(true);
                if (controllingData.Menu.Goback.WasPressedThisFrame() || (Mouse.current.leftButton.wasPressedThisFrame && !hit.collider.gameObject.GetComponent<RectTransform>()))
                {
                    state = MenuStates.Idle;
                    print(0);
                }
                //print("Settings");
                break;
            case MenuStates.LevelSelect:
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Select");
                if (controllingData.Menu.Goback.WasPressedThisFrame())
                {
                    state = MenuStates.Idle;
                    print(0);
                }
                //print("Level select");
                break;
            case MenuStates.Quit:
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Close");
                menus[1].SetActive(true);
                if (controllingData.Menu.Goback.WasPressedThisFrame() || (Mouse.current.leftButton.wasPressedThisFrame && !hit.collider.gameObject.GetComponent<RectTransform>()))
                {
                    state = MenuStates.Idle;
                    print(0);
                }
                break;
            case MenuStates.Idle:
                animator.ResetTrigger("Close");
                animator.ResetTrigger("Select");
                animator.SetTrigger("Idle");
                foreach (MenuPair menu in menus)
                {
                    menu.SetActive(false);
                }
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.SetActive(true);
                }
                if (hit.collider != null && gameObjects.Contains(hit.collider.gameObject))
                {
                    box = hit.collider.gameObject.GetComponent<BoxButton>();
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        state = box.BoxState;
                        foreach (GameObject gameObject in gameObjects)
                        {
                            gameObject.SetActive(false);
                        }
                    }
                }
                
                //print("Idle");
                break;

        }

    }

    public void SetIdle() => state = MenuStates.Idle;

}

[System.Serializable]
public class MenuPair
{
    public GameObject menuUI;
    public GameObject menuProxy;

    public void SetActive(bool active)
    {
        menuUI.SetActive(active);
        menuProxy.SetActive(active);
    }
}
