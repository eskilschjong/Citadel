using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public float LevelHealth;
    public TextMeshProUGUI levelHealthText;
    public TextMeshProUGUI levelTimerText;
    

    public void ReduceHealth(float Damage)
    {
        LevelHealth -= Damage;
        Debug.Log($"Level wave health reduced by {Damage}. Current health: {LevelHealth}");
        levelHealthText.text = LevelHealth.ToString("F1");
    }
    
    private bool levelStarted = false;
    private float levelTimer = 30f;

    void Update()
    {
        if (!levelStarted && Input.GetKeyDown(KeyCode.Space))
        {
            levelStarted = true;
            levelTimer = 30f;
        }

        if (!levelStarted) return;

        levelTimer -= Time.deltaTime;
        levelTimerText.text = levelTimer.ToString("F2");

        if (LevelHealth <= 0)
        {
            Debug.Log("You win!");
            levelStarted = false;
        }
        else if (levelTimer <= 0f)
        {
            Debug.Log("You lose! Time's up.");
            levelStarted = false;
        }
    }
}
