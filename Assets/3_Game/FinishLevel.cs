using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : InteractableOject
{
    [SerializeField] private string scene;
    public override void Interact(PlayerController player)
    {
        SceneManager.LoadScene(scene);
    }
}
