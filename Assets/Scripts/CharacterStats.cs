using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public string characterName;
    public int playerLevel = 1;

    //XP
    public int currentXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseExp = 1000;

    //Health Points
    public int currentHP;
    public int maxHP = 100;

    //Magical Power
    public int currentMP;
    public int maxMP = 30;
    public int[] bonusMP;

    //Stats
    public int strength;
    public int defense;
    public int weaponPower;
    public int armorPower;
    public string equippedWeapon;
    public string equippedArmor;

    //Image
    public Sprite characterImage;

    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseExp;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.1f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentXP += expToAdd;

        if (playerLevel < maxLevel)
        {
            if (currentXP > expToNextLevel[playerLevel])
            {
                currentXP -= expToNextLevel[playerLevel];

                playerLevel++;

                //Determine whether to add to str or def depending on if a number is odd or even
                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defense++;
                }

                //Adds HP per level up
                maxHP = Mathf.FloorToInt(maxHP * 1.05f);

                //Add bonusMP in some levels and reset currentMP
                maxMP += bonusMP[playerLevel];
                currentMP = maxMP;

            }
        }

        if(playerLevel >= maxLevel)
        {
            currentXP = 0;
        }

    }
}
