using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public float LevelHealth;
    public TextMeshProUGUI levelHealthText;
    public TextMeshProUGUI levelTimerText;

    public static bool LevelIsRunning { get; private set; }

    public List<LevelData> levels = new List<LevelData>();
    private int currentLevelIndex = 0;
    private LevelData currentLevel => levels[currentLevelIndex];


    public void ReduceHealth(float Damage)
    {
        LevelHealth -= Damage;
        Debug.Log($"Level wave health reduced by {Damage}. Current health: {LevelHealth}");
        levelHealthText.text = LevelHealth.ToString("F1");
    }

    private float levelTimer = 30f;

    void Update()
    {
        if (!LevelIsRunning && Input.GetKeyDown(KeyCode.Space))
        {
            LevelHealth = currentLevel.health;
            levelHealthText.text = LevelHealth.ToString("F1");
            levelTimer = 30f;
            LevelIsRunning = true;
            
            Debug.Log($"Starting level {currentLevelIndex + 1} vs '{currentLevel.enemyName}'");
        }

        if (!LevelIsRunning) return;

        levelTimer -= Time.deltaTime;
        levelTimerText.text = levelTimer.ToString("F2");

        if (LevelHealth <= 0)
        {
            Debug.Log($"You defeated {currentLevel.enemyName}!");

            currentLevelIndex++;
            LevelIsRunning = false;
        
        }
        else if (levelTimer <= 0f)
        {
            Debug.Log("You lose! Time's up.");
            LevelIsRunning = false;
        }
    }
}

[System.Serializable]
public class LevelData
{
    public string enemyName;
    public float health;
}

