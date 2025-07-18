using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class ShopUI : MonoBehaviour
{
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

    private Skill[] selectedSkills = new Skill[3];
    private Button[] skillButtons = new Button[3];

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

        RefreshSkillOptions();
    }

    public void HideShop()
    {
        root.style.display = DisplayStyle.None;
    }

    public void ShowShop()
    {
        root.style.display = DisplayStyle.Flex;
        RefreshSkillOptions();
    }

    private void Update()
    {
        if (!LevelManager.LevelIsRunning && Input.GetKeyDown(KeyCode.R))
        {
            RefreshSkillOptions();
        }
    }

    public void RefreshSkillOptions()
    {
        selectedSkills = RollSkills(3);
        for (int i = 0; i < selectedSkills.Length; i++)
        {
            skillButtons[i].text = selectedSkills[i].Name;
        }
    }

    private Skill[] RollSkills(int count)
    {
        Debug.Log($"Rolling {count} skills...");
        var rnd = new System.Random();
        var rolled = skills.OrderBy(s => rnd.Next()).Take(count).ToArray();

        foreach (var s in rolled)
            Debug.Log($"Skill: {s.Name} — {s.Description} ({s.Rarity})");

        return rolled;
    }

    private void BuySkill(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= selectedSkills.Length)
        {
            Debug.LogError($"Invalid skill index: {buttonIndex}");
            return;
        }

        var skill = selectedSkills[buttonIndex];
        Debug.Log($"Buying skill: {skill.Name}");
    }
}
