using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public float LevelHealth;
    public TextMeshProUGUI levelHealthText;
    public TextMeshProUGUI levelTimerText;

    public static bool LevelIsRunning { get; private set; }

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
            LevelIsRunning = true;
            levelTimer = 30f;
            levelHealthText.text = LevelHealth.ToString("F1");
        }

        if (!LevelIsRunning) return;

        levelTimer -= Time.deltaTime;
        levelTimerText.text = levelTimer.ToString("F2");

        if (LevelHealth <= 0)
        {
            Debug.Log("You win!");
            LevelIsRunning = false;
        }
        else if (levelTimer <= 0f)
        {
            Debug.Log("You lose! Time's up.");
            LevelIsRunning = false;
        }
    }
}
