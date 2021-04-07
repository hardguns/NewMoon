using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isArmor;
    public bool isWeapon;

    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Details")]
    public ItemMaterial itemMaterial;
    public int amountToChange;
    public bool affectHP;
    public bool affectMP;
    public bool affectStr;

    [Header("Armor/Weapon Details")]
    public int weaponStrength;
    public int armorStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum ItemMaterial
    {
        Wood,
        Metal,
        Magic,
        Leather,
        Fire,
    }

    public void Use(int charToUseOn)
    {
        CharacterStats selectedCharacter = GameManager.instance.charStat[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedCharacter.currentHP += amountToChange;

                if (selectedCharacter.currentHP > selectedCharacter.maxHP)
                {
                    selectedCharacter.currentHP = selectedCharacter.maxHP;
                }
            }

            if (affectMP)
            {
                selectedCharacter.currentMP += amountToChange;

                if (selectedCharacter.currentMP > selectedCharacter.maxMP)
                {
                    selectedCharacter.currentMP = selectedCharacter.maxMP;
                }
            }

            if (affectStr)
            {
                selectedCharacter.strength += amountToChange;
            }
        }

        if (isWeapon)
        {
            if (selectedCharacter.equippedWeapon != "")
            {
                GameManager.instance.AddItem(selectedCharacter.equippedWeapon);
            }

            selectedCharacter.equippedWeapon = itemName;
            selectedCharacter.weaponPower = weaponStrength;
        }

        if (isArmor)
        {
            if (selectedCharacter.equippedArmor != "")
            {
                GameManager.instance.AddItem(selectedCharacter.equippedArmor);
            }

            selectedCharacter.equippedArmor = itemName;
            selectedCharacter.armorPower = armorStrength;
        }

        GameManager.instance.RemoveItem(itemName);
    }

    public void UseInBattle(int charToUseOn)
    {
        BattleChar selectedCharacter = BattleManager.instance.activeBattlers[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedCharacter.currentHP += amountToChange;

                if (selectedCharacter.currentHP > selectedCharacter.maxHP)
                {
                    selectedCharacter.currentHP = selectedCharacter.maxHP;
                }
            }

            if (affectMP)
            {
                selectedCharacter.currentMP += amountToChange;

                if (selectedCharacter.currentMP > selectedCharacter.maxMP)
                {
                    selectedCharacter.currentMP = selectedCharacter.maxMP;
                }
            }

            if (affectStr)
            {
                selectedCharacter.strength += amountToChange;
            }
        }

        if (isWeapon)
        {
            //if (selectedCharacter.equippedWeapon != "")
            //{
            //    GameManager.instance.AddItem(selectedCharacter.equippedWeapon);
            //}

            //selectedCharacter.equippedWeapon = itemName;
            selectedCharacter.wpnPower = weaponStrength;
        }

        if (isArmor)
        {
            //if (selectedCharacter.equippedArmor != "")
            //{
            //    GameManager.instance.AddItem(selectedCharacter.equippedArmor);
            //}

            //selectedCharacter.equippedArmor = itemName;
            selectedCharacter.armPower = armorStrength;
        }

        GameManager.instance.RemoveItem(itemName);
    }

}


