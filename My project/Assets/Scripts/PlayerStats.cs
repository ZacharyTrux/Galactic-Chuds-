using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public const int MaxHealth = 3;
    private int score = 0;

    private int currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = MaxHealth;
        UpdateUI();
    }

    public void DamagePlayer()
    {
        currentHealth -= 1;
        UpdateUI();
        if(currentHealth <= 0)
        {
            DeathScreen();
        }
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        healthText.text = "Health: " + currentHealth;
        scoreText.text = "Score: " + score;
    }

    void DeathScreen()
    {
        return;
    }
}
