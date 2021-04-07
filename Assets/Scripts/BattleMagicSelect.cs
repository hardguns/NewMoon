using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{
    //Sets the spell name
    public string spellName;

    //Sets the spell cost
    public int spellCost;

    //Reference the spell name object
    public Text nameText;

    //Reference the cost text object
    public Text costText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Press()
    {
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        }
        else
        {
            //not enough mana
            BattleManager.instance.battleNotification.notificationText.text = "Not enough mana!";
            BattleManager.instance.battleNotification.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }
    }
}
