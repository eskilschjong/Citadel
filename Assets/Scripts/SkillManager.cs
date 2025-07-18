using UnityEngine;
using System.Linq;

public class SkillManager : MonoBehaviour
{
    
    private Skill[] Skills = new Skill[]
    {
        new Skill { Name = "Collector", Description = "Your weapons deal +5% damage for each unique weapon", Rarity = "Common" },
        new Skill { Name = "Egg Basket", Description = "Your weapons deal +100% damage, but deal 10% less for each weapon", Rarity = "Common" },
        new Skill { Name = "Crank", Description = "Your weapons deal -50% damage, but deal 10% more for every attack", Rarity = "Common" },
        new Skill { Name = "CPU Cooler", Description = "Remove overclock penalty from highest energy weapon", Rarity = "Uncommon"}
    };

    Skill[] selectedSkills = new Skill[3];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.LevelIsRunning && Input.GetKeyDown(KeyCode.R))
        {
            selectedSkills = RollSkills();
        }
    }

    public Skill[] RollSkills()
    {
        Debug.Log("Rolling skills...");
        var random = new System.Random();
        var rolledSkills = Skills.OrderBy(x => random.Next()).Take(3).ToArray();
        foreach (var skill in rolledSkills)
        {
            Debug.Log($"Skill: {skill.Name}, Description: {skill.Description}, Rarity: {skill.Rarity}");
        }
        return rolledSkills;
    }

    public void BuySkill(int index)
    {
        if (index < 0 || index >= Skills.Length)
        {
            Debug.LogError("Invalid skill index.");
            return;
        }

        Debug.Log($"Buying skill: {Skills[index].Name}");
        // Implement skill purchase logic here
    }
    

}

public class Skill
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Rarity { get; set; }
}
