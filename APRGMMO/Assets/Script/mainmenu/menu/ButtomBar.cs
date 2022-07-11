using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtomBar : MonoBehaviour
{
    private UIButton combatButton;
    private UIButton knapsackButton;
    private UIButton taskButton;
    private UIButton skillButton;
    private UIButton shopButton;
    private UIButton systemButton;

    private PlayerAutoMove playerAutoMove;
    private PlayerAutoMove PlayerAutoMove
    {
        get
        {
            if (playerAutoMove == null)
            {
                playerAutoMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAutoMove>();
            }
            return playerAutoMove;
        }
    }

    private void Awake()
    {
        combatButton = FindButton("Combat");
        knapsackButton = FindButton("Knapsack");
        taskButton = FindButton("Task");
        skillButton = FindButton("Skill");
        shopButton = FindButton("Shop");
        systemButton = FindButton("System");

        EventDelegate ed1 = new EventDelegate(this, "OnCombat");
        combatButton.onClick.Add(ed1);

        EventDelegate ed2 = new EventDelegate(this, "OnKnapsack");
        knapsackButton.onClick.Add(ed2);

        EventDelegate ed3 = new EventDelegate(this, "OnTask");
        taskButton.onClick.Add(ed3);

        EventDelegate ed4 = new EventDelegate(this, "OnSkill");
        skillButton.onClick.Add(ed4);

        EventDelegate ed5 = new EventDelegate(this, "OnShop");
        shopButton.onClick.Add(ed5);

        EventDelegate ed6 = new EventDelegate(this, "OnSystem");
        systemButton.onClick.Add(ed6);
    }

    UIButton FindButton(string btnName)
    {
        return transform.Find(btnName).GetComponent<UIButton>();
    }

    void OnCombat()
    {
        PlayerAutoMove.SetDestination(NPCManage._instance.transcriptGo.transform.position);
    }
    void OnKnapsack()
    {
        KnapSack._instance.Show();
    }
    void OnTask()
    {
        TaskUI._instance.Show();
    }
    void OnSkill()
    {
        SkillUI._instance.Show();
    }
    void OnShop()
    {

    }
    void OnSystem()
    {

    }
}
