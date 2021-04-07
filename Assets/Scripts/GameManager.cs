using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterStats[] charStat;

    //gameMenuOpen: Is menu open?
    //dialogActive: Is dialog active?
    //fadingBetweenAreas: Show/hide fading
    //shopActive: Is shop menu open?
    //battleActive: Is player in battle?
    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, battleActive;

    //Itemsheld array saves the info of which items does player has
    public string[] itemsHeld;

    //numberOfItems array shows how many items (of the Itemsheld array) do player has
    //Uses the same position of itemsHeld
    public int[] numberOfItems;

    //referenceItems contains all the items that the game will have 
    public Item[] referenceItems;

    //Show current amunt of gold
    public int currentGold;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive)
        {
            PlayerController.player.canMove = false;
        }
        else
        {
            PlayerController.player.canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Iron Armor");
            AddItem("asdasd");

            RemoveItem("Health Potion");
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }
    }

    public Item GetItemDetails(string itemToGrab)
    {
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName == itemToGrab)
            {
                return referenceItems[i];
            }
        }

        return null;
    }

    public void SortItems()
    {
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if (itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        bool foundSpace = false;
        int newItemPosition = 0;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                foundSpace = true;
                break;
            }
        }

        if (foundSpace)
        {
            bool itemExist = false;
            for (int i = 0; i < referenceItems.Length; i++)
            {
                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExist = true;
                    break;
                }
            }

            if (itemExist)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " does not exist!!");
            }
        }

        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;
                break;
            }
        }

        if (foundItem)
        {
            numberOfItems[itemPosition]--;

            if (numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }
        }
        else
        {
            Debug.LogError("Couldn't find " + itemToRemove);
        }

        GameMenu.instance.ShowItems();
    }

    public void SaveData()
    {
        string[] ignore_fields = { "characterImage", "expToNextLevel", "bonusMP", "characterName" };
        System.Type playerType = charStat.GetType().GetElementType();

        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.player.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.player.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.player.transform.position.z);

        for (int i = 0; i < charStat.Length; i++)
        {
            if (charStat[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + charStat[i].characterName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + charStat[i].characterName + "_active", 0);
            }

            //go through each player and save their stats
            foreach (var field in playerType.GetFields())
            {
                if (ignore_fields.Contains(field.Name))
                {
                    continue; //skip fields in ignore list
                }
                var value = playerType.GetField(field.Name).GetValue(charStat[i]);
                string pref_name = "Player_" + charStat[i].characterName + "_" + field.Name;

                if (value.GetType() == typeof(string))
                {
                    PlayerPrefs.SetString(pref_name, (string)value);
                }
                if (value.GetType() == typeof(int))
                {
                    PlayerPrefs.SetInt(pref_name, (int)value);
                }
            }
        }
        //store inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    }

    public void LoadData()
    {
        PlayerController.player.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), PlayerPrefs.GetFloat("Player_Position_y"), PlayerPrefs.GetFloat("Player_Position_z"));

        string[] ignore_fields = { "characterImage", "expToNextLevel", "bonusMP", "characterName" };
        System.Type playerType = charStat.GetType().GetElementType();

        for (int i = 0; i < charStat.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + charStat[i].characterName + "_active") == 0)
            {
                charStat[i].gameObject.SetActive(false);
            }
            else
            {
                charStat[i].gameObject.SetActive(true);
            }

            //go through each player and save their stats
            foreach (var field in playerType.GetFields())
            {
                if (ignore_fields.Contains(field.Name))
                {
                    continue; //skip fields in ignore list
                }
                var value = playerType.GetField(field.Name).GetValue(charStat[i]);
                string pref_name = "Player_" + charStat[i].characterName + "_" + field.Name;

                if (value.GetType() == typeof(string))
                {
                    var fieldProperty = playerType.GetField(field.Name);
                    field.SetValue(charStat[i], PlayerPrefs.GetString(pref_name));
                }
                if (value.GetType() == typeof(int))
                {
                    var fieldProperty = playerType.GetField(field.Name);
                    field.SetValue(charStat[i], PlayerPrefs.GetInt(pref_name));
                }
            }
        }

        //get inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }

    }

}
