using UnityEngine;

public class Generator : MonoBehaviour
{
    public float EnergyProduction;
    private EnergyManager energyManager;

    void Start()
    {
        energyManager = FindFirstObjectByType<EnergyManager>();
        
        if (energyManager != null)
        {
            energyManager.ChangeTotalEnergy(EnergyProduction);
        }
        else
        {
            Debug.LogError("EnergyManager not found in scene!");
        }
    }
}
