using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public string CurrentMenuID = "Basic";

    public List<GameObject> gameObjects = new List<GameObject>();
    public List<GameObject> menus = new List<GameObject>();
    BoxButton box;

    public enum states { Game, Settings, LevelSelect, Quit, Idle };

    public states state = states.Idle;

    public Animator animator;

    Controlling_Data controllingData;

    private void Awake() { controllingData = new Controlling_Data(); }
    private void OnEnable() { controllingData.Enable(); }

    RaycastHit hit;
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        Physics.Raycast(ray, out hit);

        switch (state)
        {
            case states.Game:
                //print("Game");
                break;
            case states.Settings:
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Close");
                if (controllingData.Menu.Goback.WasPressedThisFrame())
                {
                    state = states.Idle;
                }
                //print("Settings");
                break;
            case states.LevelSelect:
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Select");
                if (controllingData.Menu.Goback.WasPressedThisFrame())
                {
                    state = states.Idle;
                }
                //print("Level select");
                break;
            case states.Quit:
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Close");
                if (controllingData.Menu.Goback.WasPressedThisFrame())
                {
                    state = states.Idle;
                }
                break;
            case states.Idle:
                animator.ResetTrigger("Close");
                animator.ResetTrigger("Select");
                animator.SetTrigger("Idle");
                if (gameObjects.Contains(hit.collider.gameObject))
                {
                    box = hit.collider.gameObject.GetComponent<BoxButton>();
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        state = box.BoxState;

                    }
                    print(hit.collider.gameObject);
                }
                
                //print("Idle");
                break;

        }

    }



}
