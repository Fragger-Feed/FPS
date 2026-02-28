using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    // == Data ==
    [Header("Data")]
    public PlayerController PlayerController;
    public int maxHealth = 100;

    public int currentHealth;
    [SerializeField] private Slider hpSlider;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        PlayerController.playerUIController.UpdateCurrentMenu();
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        PlayerController.playerUIController.UpdateCurrentMenu();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("Hub_Scene");
    }
}