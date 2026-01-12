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
    public MenuStates CurrentMenuState = MenuStates.MainView;
    public SerializableDictionary<string, GameObject> MenuStateTriggers;
    public SerializableDictionary<string, List<MenuPair>> Menus;
    public Animator CameraAnimator;


    public void ChangeMenuState(MenuStates state) {
        CurrentMenuState = state;

        switch (CurrentMenuState) {

            case MenuStates.MainView:
                CameraAnimator.ResetTrigger("TV");
                CameraAnimator.ResetTrigger("Cassettes");
                CameraAnimator.ResetTrigger("Table");
                CameraAnimator.SetTrigger("MainView");

                foreach (var pair in MenuStateTriggers) {
                    pair.Value.SetActive(pair.Key == "MainView");
                }


                break;

            case MenuStates.TV:
                CameraAnimator.ResetTrigger("MainView");
                CameraAnimator.SetTrigger("TV");

                foreach (var pair in MenuStateTriggers) {
                    pair.Value.SetActive(pair.Key == "TV");
                }

                break;

            case MenuStates.Cassettes:
                CameraAnimator.ResetTrigger("MainView");
                CameraAnimator.SetTrigger("Cassettes");

                foreach (var pair in MenuStateTriggers) {
                    pair.Value.SetActive(pair.Key == "Cassettes");
                }

                break;

            case MenuStates.Table:
                CameraAnimator.ResetTrigger("MainView");
                CameraAnimator.SetTrigger("Table");

                foreach (var pair in MenuStateTriggers) {
                    pair.Value.SetActive(pair.Key == "Table");
                }

                break;

        }
    }





    public void Update()
    {
        // Go BACK
        if (controllingData.Menu.Goback.WasPressedThisFrame() && CurrentMenuState != MenuStates.MainView) {
            ChangeMenuState(MenuStates.MainView);
        }

        // Raycast
        if (Mouse.current.leftButton.wasPressedThisFrame) {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.collider.TryGetComponent(out BoxButton button)) {
                    ChangeMenuState(button.BoxState);
                }
            }
        }
    }
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
