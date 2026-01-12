using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen_Controller : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene(1);
    }
}
