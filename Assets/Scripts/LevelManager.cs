using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public float LevelHealth;
    public TextMeshProUGUI levelHealthText;
    public TextMeshProUGUI levelTimerText;
    public TextMeshProUGUI enemyNameText;
    public ShopUI uiScript;

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
            StartLevel();
        }

        if (!LevelIsRunning) return;

        levelTimer -= Time.deltaTime;
        levelTimerText.text = levelTimer.ToString("F2");

        if (LevelHealth <= 0)
        {
            LevelWin();
        }
        else if (levelTimer <= 0f)
        {
            LevelDefeat();
        }
    }

    private void StartLevel()
    {
        uiScript.HideShop();

        LevelHealth = currentLevel.health;
        levelHealthText.text = LevelHealth.ToString("F1");
        enemyNameText.text = currentLevel.enemyName;
        levelTimer = 30f;
        LevelIsRunning = true;
        
        Debug.Log($"Starting level {currentLevelIndex + 1} vs '{currentLevel.enemyName}'");
    }

    private void LevelWin()
    {
        Debug.Log($"You defeated {currentLevel.enemyName}!");
        currentLevelIndex++;
        LevelIsRunning = false;

        uiScript.ShowShop();
    }

    private void LevelDefeat()
    {
        Debug.Log("You lose! Time's up.");
        LevelIsRunning = false;
    }


}

[System.Serializable]
public class LevelData
{
    public string enemyName;
    public float health;
}

