using UnityEngine;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    public float TotalEnergy = 0;
    public float ExcessEnergy = 0;
    public TextMeshProUGUI energyText;
    public void ChangeTotalEnergy(float Energy)
    {
        TotalEnergy += Energy;
        ExcessEnergy += Energy;
        energyText.text = ExcessEnergy.ToString() + "/" + TotalEnergy.ToString();
    }
    
    public bool ChangeExcessEnergy(float Energy)
    {
        if (ExcessEnergy + Energy >= 0)
        {
            ExcessEnergy += Energy;
            energyText.text = ExcessEnergy.ToString() + "/" + TotalEnergy.ToString();
            return true;
        }
        else return false;
    }
}
