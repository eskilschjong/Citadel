using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class ShopUI : MonoBehaviour
{
    public static int itemTable = 8;
    // --- Skill definition ---
    [Serializable]
    public class Skill
    {
        public string Name;
        public string Description;
        public string Rarity;
    }

    // --- Skill data ---
    [Header("Skill Pool")]
    [SerializeField]
    private Skill[] skills = new Skill[]
    {
        new Skill { Name = "Collector",   Description = "Your weapons deal +5% damage for each unique weapon", Rarity = "Common" },
        new Skill { Name = "Egg Basket",  Description = "Your weapons deal +100% damage, but deal 10% less for each weapon", Rarity = "Common" },
        new Skill { Name = "Crank",       Description = "Your weapons deal -50% damage, but deal 10% more for every attack",    Rarity = "Common" },
        new Skill { Name = "CPU Cooler",  Description = "Remove overclock penalty from highest energy weapon",             Rarity = "Uncommon" }
    };

    public class Item
    {
        public string Name;
        public string Description;
        public float Price;
        public string Rarity;
        
    }

    [Header("Item Pool")]
    [SerializeField]
    private Item[] items = new Item[]
    {
        new Item { Name = "Generator", Description = "Powers Other Structures", Price = 25f, Rarity = "Generator" },
        new Item { Name = "Pulsar",  Description = "Low cost plasma weapon", Price = 100f, Rarity = "Common" }
    };

    private Skill[] selectedSkills = new Skill[3];
    private Button[] skillButtons = new Button[3];
    private Item[] selectedItems = new Item[itemTable];
    private Button[] itemButtons = new Button[itemTable];

    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        skillButtons[0] = root.Q<Button>("SkillButton1");
        skillButtons[1] = root.Q<Button>("SkillButton2");
        skillButtons[2] = root.Q<Button>("SkillButton3");


        for (int i = 0; i < skillButtons.Length; i++)
        {
            int index = i;
            skillButtons[i].clicked += () => BuySkill(index);
        }

        RefreshSkillOptions(3);

        var items = root.Query<Button>(name: "ItemButton").ToList();
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i] = items[i];
            int index = i;
            itemButtons[i].clicked += () => SelectItem(index);
        }

        RefreshItemOptions(itemTable);
    }

    public void HideShop()
    {
        root.style.display = DisplayStyle.None;
    }

    public void ShowShop()
    {
        root.style.display = DisplayStyle.Flex;
        RefreshSkillOptions(3);
    }

    private void Update()
    {
        if (!LevelManager.LevelIsRunning && Input.GetKeyDown(KeyCode.R))
        {
            RefreshSkillOptions(3);
        }
    }

    public void RefreshSkillOptions(int count)
    {
        Debug.Log($"Rolling {count} skills...");
        var rnd = new System.Random();
        selectedSkills = skills.OrderBy(s => rnd.Next()).Take(count).ToArray();

        for (int i = 0; i < count; i++)
        {
            skillButtons[i].text = selectedSkills[i].Name;
            Debug.Log($"Skill: {selectedSkills[i].Name} - {selectedSkills[i].Description} ({selectedSkills[i].Rarity})");
        }
    }

    public void RefreshItemOptions(int count)
    {
        Debug.Log($"Rolling {count} items...");

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, items.Length);
            selectedItems[i] = items[randomIndex];
            itemButtons[i].text = selectedItems[i].Name;
            Debug.Log($"Item: {selectedItems[i].Name} - {selectedItems[i].Description} ({selectedItems[i].Rarity})");
        }
    }

    private void BuySkill(int index)
    {
        if (index < 0 || index >= selectedSkills.Length)
        {
            Debug.LogError($"Invalid skill index: {index}");
            return;
        }

        var skill = selectedSkills[index];
        Debug.Log($"Buying skill: {skill.Name}");
    }
    Item selectedItem = new Item();
    private void SelectItem(int index)
    {
        selectedItem = selectedItems[index];

        GameObject gridSystemObj = GameObject.Find("GridSystem");
        var gridSystem = gridSystemObj.GetComponent<GridSystem>();
        gridSystem.SelectItem(selectedItem.Name, selectedItem.Price);
    }

    private void BuyItem(float price)
    {
        GameObject currencyManagerObj = GameObject.Find("CurrencyManager");
        var currencyManager = currencyManagerObj.GetComponent<CurrencyManager>();
        currencyManager.decreaseNutsAmount(selectedItem.Price);
        Debug.Log($"Bought item: {selectedItem.Name}");
        selectedItem = null;
    }
}
