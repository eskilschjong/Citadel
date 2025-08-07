using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public float nutsAmount = 100f;
    public float income = 50f;

    public void increaseIncome(float increase)
    {
        income += increase;
        Debug.Log("Income increased. New income: " + income);
    }

    public void increaseNutsAmount(float increase)
    {
        nutsAmount += increase;
        Debug.Log("Nuts amount increased. New nuts amount: " + nutsAmount);
    }

    public bool decreaseNutsAmount(float decrease)
    {
        if (decrease > nutsAmount)
        {
            Debug.Log("You cant afford this purchase.");
            return false;
        }
        nutsAmount -= decrease;
        Debug.Log("Nuts amount decreased. New nuts amount: " + nutsAmount);
        return true;
    }
}
