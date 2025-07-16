using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float Damage;
    public float EnergyCost;
    private LevelManager LevelManager;
    private EnergyManager EnergyManager;

    private static Weapon selectedWeapon;
    public static Weapon SelectedWeapon => selectedWeapon;

    private float nextFireTime = 0f;
    [SerializeField] private float Energy = 0;
    void Start()
    {
        if (LevelManager == null)
        {
            GameObject LevelManagerObj = GameObject.Find("LevelManager");
            if (LevelManagerObj != null)
            {
                LevelManager = LevelManagerObj.GetComponent<LevelManager>();
            }
            else
            {
                Debug.LogWarning("LevelManager object not found in the scene.");
            }
        }
        if (EnergyManager == null)
        {
            GameObject energyManagerObj = GameObject.Find("EnergyManager");
            if (energyManagerObj != null)
            {
                EnergyManager = energyManagerObj.GetComponent<EnergyManager>();
            }
            else
            {
                Debug.LogWarning("EnergyManager object not found in the scene.");
            }
        }
    }

    private bool cooldownInitialized = false;

    // Update is called once per frame
    void Update()
    {
        // Combat Mode
        if (LevelManager.LevelIsRunning)
        {
            if (!cooldownInitialized)
            {
                if (Energy > 0f)
                    nextFireTime = Time.time + (EnergyCost / Energy);
                else
                    nextFireTime = float.MaxValue;
                
                cooldownInitialized = true;
            }

            if (Time.time >= nextFireTime && Energy > 0f)
            {
                Shoot();
                nextFireTime = Time.time + (EnergyCost / Energy);
            }
        }
        // Shop/Build Mode
        else
        {
            cooldownInitialized = false;
            if (SelectedWeapon == this)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    ChangeEnergy(-50f);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ChangeEnergy(50f);
                }
            }
        }
    }

    private void ChangeEnergy(float EnergyChange)
    {
        if (EnergyManager != null)
        {
            if (EnergyManager.ChangeExcessEnergy(EnergyChange))
            {
                Energy -= EnergyChange;
                nextFireTime = Time.time + (EnergyCost / Energy);
                Debug.Log($"Energy changed. Current Energy: {Energy}");
            }
        }

    }
    private void Shoot()
    {
        if (LevelManager != null)
        {
            LevelManager.ReduceHealth(Damage);
        }
    }

    void OnMouseDown()
    {
        selectedWeapon = this;
        Debug.Log($"Weapon '{gameObject.name}' selected.");
    }

}
