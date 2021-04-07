using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleReward : MonoBehaviour
{
    public static BattleReward instance;

    public GameObject battleRewardsScreen;
    public Text xpText, itemsText;

    public int xpEarned;
    public string[] rewardItemsEarned;

    public bool markQuestComplete;
    public string questToMark;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenBattleRewardScreen(int xp, string[] rewardItems)
    {
        xpEarned = xp;
        rewardItemsEarned = rewardItems;

        xpText.text = "Everyone earned " + xpEarned + " xp!";
        itemsText.text = "";

        for (int i = 0; i < rewardItems.Length; i++)
        {
            itemsText.text += "- " + rewardItems[i] + "\n";
        }

        battleRewardsScreen.SetActive(true);
    }

    public void CloseBattleRewardScreen()
    {
        for (int i = 0; i < GameManager.instance.charStat.Length; i++)
        {
            if (GameManager.instance.charStat[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.charStat[i].AddExp(xpEarned);
            }
        }

        for (int i = 0; i < rewardItemsEarned.Length; i++)
        {
            GameManager.instance.AddItem(rewardItemsEarned[i]);
        }

        battleRewardsScreen.SetActive(false);
        GameManager.instance.battleActive = false;

        if (markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
