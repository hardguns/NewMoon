using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject Menu;

    private CharacterStats[] playerStats;

    //Array to show different tabs of menu
    public GameObject[] windows;

    //Array to know which buttons are gonna be replace 
    public GameObject[] buttons;

    //Text arrays for updating menu
    public Text[] nameText, hpText, mpText, lvlText, expText;

    //Sliders and Image array for each slider and character image
    public Slider[] expSlider;
    public Image[] charImage;

    //CharStatHolder is the gameobjects array that shows or not the character card on menu
    public GameObject[] charStatHolder;

    //Text variables to fill in status menu
    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusEqdWpn, statusWpnPwr, statusEqdArm, statusArmPwr, statusExpToNextLvl;

    //Image variable to fill in status menu
    public Image statusImage;

    //Contains the buttons in items menu
    public ItemButton[] itemButtons;

    //--------------------------- Show item details -----------------------------
    //Create an item variable to set as the active item on items menu screen
    public Item activeItem;

    //selectedItem is use to set the string value of the selected item
    public string selectedItem;

    //Text objects which are in the items panel for replacing 
    public Text itemName, itemDescription, useButtonText;

    //Create an instance of the script to use it in other scripts
    public static GameMenu instance;
    //--------------------------- Show item details -----------------------------

    //--------------------------- Use item for character options -----------------------------
    //Create a gameObject to show or hide the character names panel 
    public GameObject itemCharMenu;

    //Text array of text buttons to set the characters name
    public Text[] itemCharacterNames;
    //--------------------------- Use item for character options -----------------------------

    //--------------------------- Show current amount of gold -----------------------------
    //Text object reference to update current update in menu 
    public Text goldText;
    //--------------------------- Show current amount of gold -----------------------------

    //Load the scene manu
    public string menuSceneName;

    //Main canvas menu to show stats

    // Start is called before the first frame update
    void Start()
    {
        instance = this;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (!GameManager.instance.battleActive && !GameManager.instance.dialogActive && !GameManager.instance.shopActive)
            {
                if (Menu.activeInHierarchy)
                {
                    //Closes the menu, deactivate windows and allow player movement
                    CloseMenu();
                }
                else
                {

                    Menu.SetActive(true);

                    //Call Update method when setActive
                    UpdatePlayerStats();

                    //Set variable in game manager to allow or not player movement
                    GameManager.instance.gameMenuOpen = true;
                }

                AudioManager.instance.PlaySFX(5);
            }
        }
    }

    //Updates the stats from GameManager
    void UpdatePlayerStats()
    {
        playerStats = GameManager.instance.charStat;

        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);

                nameText[i].text = playerStats[i].characterName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Level: " + playerStats[i].playerLevel;
                expText[i].text = playerStats[i].currentXP.ToString() + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentXP;
                charImage[i].sprite = playerStats[i].characterImage;

            }
            else
            {
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    //Menu navigation
    public void ToggleMenu(int windowNumber)
    {
        UpdatePlayerStats();

        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }

        CloseItemCharacterChoice();
    }

    //Closes the menu, deactivate windows and allow player movement
    public void CloseMenu()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }

        Menu.SetActive(false);

        //Set variable in game manager to allow or not player movement
        GameManager.instance.gameMenuOpen = false;

        CloseItemCharacterChoice();
    }

    //Update the information that is shown
    public void OpenStatus()
    {
        UpdatePlayerStats();

        StatusChar(0);

        for (int i = 0; i < playerStats.Length; i++)
        {
            buttons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            buttons[i].GetComponentInChildren<Text>().text = playerStats[i].characterName;
        }
    }

    //Updates status character menu information
    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].characterName;
        statusHP.text = playerStats[selected].currentHP.ToString() + "/" + playerStats[selected].maxHP.ToString();
        statusMP.text = playerStats[selected].currentMP.ToString() + "/" + playerStats[selected].maxMP.ToString();
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defense.ToString();
        if (playerStats[selected].equippedWeapon != "")
        {
            statusEqdWpn.text = playerStats[selected].equippedWeapon;
        }
        statusWpnPwr.text = playerStats[selected].weaponPower.ToString();
        if (playerStats[selected].equippedArmor != "")
        {
            statusEqdArm.text = playerStats[selected].equippedArmor;
        }
        statusArmPwr.text = playerStats[selected].armorPower.ToString();
        statusExpToNextLvl.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - playerStats[selected].currentXP).ToString();
        statusImage.sprite = playerStats[selected].characterImage;
    }

    //Show current items in menu
    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }
        else if (activeItem.isWeapon || activeItem.isArmor)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharacterChoice()
    {
        itemCharMenu.SetActive(true);

        for (int i = 0; i < itemCharacterNames.Length; i++)
        {
            itemCharacterNames[i].text = GameManager.instance.charStat[i].characterName;
            itemCharacterNames[i].transform.parent.gameObject.SetActive(GameManager.instance.charStat[i].gameObject.activeInHierarchy);
        }
    }

    public void CloseItemCharacterChoice()
    {
        itemCharMenu.SetActive(false);
    }

    public void UseItem(int selectedChar)
    {
        activeItem.Use(selectedChar);
        CloseItemCharacterChoice();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(menuSceneName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.player.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
