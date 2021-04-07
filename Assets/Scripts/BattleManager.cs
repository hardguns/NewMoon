using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    //Creates the instance of the battle manager
    public static BattleManager instance;

    //Bo
    private bool battleActive;

    //Takes the gameObject to active the scene
    public GameObject battleScene;

    //Positions array to show in the battle scene
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    //Uses the player and enemy prefabs to spawn them in battle
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    //List to save the current battlers in the battle scene
    public List<BattleChar> activeBattlers = new List<BattleChar>();

    //public enum Turns {
    //    playerTurn,
    //    enemyTurn
    //}

    //Turns turnWaiting;
    public bool turnWaiting;
    public int currentTurn;

    //Instance of buttons panel
    public GameObject uiButtonsHandler;

    //Holds the array of moves
    public BattleMove[] moveList;

    //Particle system for the enemy
    public GameObject enemyAttackEffect;

    //GameObject to show damage on screen
    public DamageNumber damageNumber;

    //Text arrays to replace info into UI
    public Text[] playerNames;
    public Text[] playerHPS;
    public Text[] playerMPS;

    //References the target Menu
    public GameObject targetMenu;

    //Reference the buttons which have the BattleTargetButton component
    public BattleTargetButton[] targetButtons;

    //References the magic Menu
    public GameObject magicMenu;

    //Reference the buttons which have the BattleTargetButton component
    public BattleMagicSelect[] magicButtons;

    //Reference of the battle notification gameObject
    public BattleNotification battleNotification;

    //Set the chance to flee
    public int chanceToFlee = 35;

    //Sets if player is fleeing
    private bool fleeing;

    //References the Items Menu
    public GameObject itemsMenu;

    //References the use button in Items Menu
    public GameObject useButton;

    //Contains the buttons in battle items menu
    public ItemButton[] battleItemButtons;

    //Active item
    public Item selectedBattleItem;

    //Text objects to asign text values
    public Text battleItemName, battleItemDescription, useButtonText;

    //Create a gameObject to show or hide the character battle names panel 
    public GameObject itemBattleCharMenu;

    //Create the text array which contains the names of battle players
    public Text[] itemBattleCharacterNames;

    //Sets the gameOver scene name
    public string gameOverScene;

    public int rewardXP;
    public string[] rewardItems;

    public bool cannotFlee;


    //Finds the images to replace player's armor and items
    public Image[] equippedItemImages;

    public GameObject infoMenu;

    public Image playerStatsSprite;

    public Text nameStatsText;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Spider", "Eyeball", "Skeleton" }, false);
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHandler.SetActive(true);
                }
                else
                {
                    uiButtonsHandler.SetActive(false);

                    StartCoroutine(EnemyMoveCo());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }
    }

    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if (!battleActive)
        {
            FindObjectOfType<Camera>().orthographicSize = 5f;

            cannotFlee = setCannotFlee;

            battleActive = true;

            //Set battleActive to true on gameManager to stop moving the player
            GameManager.instance.battleActive = true;

            //Sets the current transform of camera to the battle scene
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            //Starts a bg song
            AudioManager.instance.PlayMusic(0);

            //Adds the players to the battle scene
            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (GameManager.instance.charStat[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.charStat[i].characterName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            newPlayer.transform.parent = playerPositions[i];

                            activeBattlers.Add(newPlayer);
                            CharacterStats playerStats = GameManager.instance.charStat[i];
                            activeBattlers[i].currentHP = playerStats.currentHP;
                            activeBattlers[i].maxHP = playerStats.maxHP;
                            activeBattlers[i].currentMP = playerStats.currentMP;
                            activeBattlers[i].maxMP = playerStats.maxMP;
                            activeBattlers[i].strength = playerStats.strength;
                            activeBattlers[i].defense = playerStats.defense;
                            activeBattlers[i].wpnPower = playerStats.weaponPower;
                            activeBattlers[i].armPower = playerStats.armorPower;

                        }
                    }
                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];

                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

            turnWaiting = true;
            //currentTurn = 0;
            currentTurn = Random.Range(0, activeBattlers.Count);
        }

        UpdateUIStats();
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;

        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            if (activeBattlers[i].currentHP == 0)
            {
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].battlerSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].battlerSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //end battle in failure
                StartCoroutine(GameOverCo());
            }

            //battleScene.SetActive(false);
            //GameManager.instance.battleActive = false;
            turnWaiting = false;
        }
        else
        {
            while (activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;

                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        List<int> players = new List<int>();

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];
        int movePower = 0;

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);

        for (int i = 0; i < moveList.Length; i++)
        {
            if (moveList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(moveList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = moveList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
        float defPwr = activeBattlers[target].defense + activeBattlers[target].armPower;

        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + "(" + damageToGive + ") to   " + activeBattlers[target].charName);

        activeBattlers[target].currentHP -= damageToGive;

        Instantiate(damageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
    }


    public void UpdateUIStats()
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    playerNames[i].gameObject.SetActive(true);
                    playerNames[i].text = playerData.charName;
                    playerHPS[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMPS[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                {
                    playerNames[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerNames[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowStats(int characterToShow)
    {
        infoMenu.SetActive(true);
        itemsMenu.SetActive(false);
        nameStatsText.text = playerNames[characterToShow].text;
        for (int i = 0; i < GameManager.instance.charStat.Length; i++)
        {
            if (GameManager.instance.charStat[i].characterName == playerNames[characterToShow].text)
            {
                playerStatsSprite.sprite = GameManager.instance.charStat[i].characterImage;

                if (GameManager.instance.charStat[i].equippedWeapon != "" && GameManager.instance.charStat[i].equippedWeapon != null)
                {
                    equippedItemImages[0].gameObject.SetActive(true);
                    equippedItemImages[0].sprite = GameManager.instance.GetItemDetails(GameManager.instance.charStat[i].equippedWeapon).itemSprite;
                }
                else
                {
                    equippedItemImages[0].gameObject.SetActive(false);
                }

                if (GameManager.instance.charStat[i].equippedArmor != "" && GameManager.instance.charStat[i].equippedArmor != null)
                {
                    equippedItemImages[1].gameObject.SetActive(true);
                    equippedItemImages[1].sprite = GameManager.instance.GetItemDetails(GameManager.instance.charStat[i].equippedArmor).itemSprite;
                }
                else
                {
                    equippedItemImages[1].gameObject.SetActive(false);
                }
            }
        }
    }

    public void CloseInfoMenu()
    {
        infoMenu.SetActive(false);
    }

    public void PlayerAttack(string moveName, int selectedTarget)
    {
        int movePower = 0;

        for (int i = 0; i < moveList.Length; i++)
        {
            if (moveList[i].moveName == moveName)
            {
                Instantiate(moveList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = moveList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);

        uiButtonsHandler.SetActive(false);
        targetMenu.SetActive(false);
        NextTurn();
    }

    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].moveName = moveName;
                targetButtons[i].targetText.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[i].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                for (int j = 0; j < moveList.Length; j++)
                {
                    if (magicButtons[i].spellName == moveList[j].moveName)
                    {
                        magicButtons[i].spellCost = moveList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if (cannotFlee)
        {
            battleNotification.notificationText.text = "Can not flee this battle!";
            battleNotification.Activate();
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);

            if (fleeSuccess < chanceToFlee)
            {
                //end the battle
                //battleActive = false;
                //GameManager.instance.battleActive = false;
                //battleScene.SetActive(false);
                fleeing = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                battleNotification.notificationText.text = "Couldn't escape!";
                battleNotification.Activate();

                NextTurn();
            }
        }

    }

    public void OpenItemsMenu()
    {
        infoMenu.SetActive(false);
        selectedBattleItem = null;
        battleItemName.text = "Select an item to use";
        battleItemDescription.text = "Here you'll see details of the object";
        useButton.GetComponent<Button>().interactable = false;

        itemsMenu.SetActive(true);

        ShowInBattleItems();
    }

    public void ShowInBattleItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0; i < battleItemButtons.Length; i++)
        {
            battleItemButtons[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                battleItemButtons[i].buttonImage.gameObject.SetActive(true);
                battleItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                battleItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                battleItemButtons[i].buttonImage.gameObject.SetActive(false);
                battleItemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectBattleItem(Item newItem)
    {
        selectedBattleItem = newItem;
        useButton.GetComponent<Button>().interactable = true;

        if (selectedBattleItem.isItem)
        {
            useButtonText.text = "Use";
        }
        else if (selectedBattleItem.isWeapon || selectedBattleItem.isArmor)
        {
            useButtonText.text = "Equip";
        }

        battleItemName.text = selectedBattleItem.itemName;
        battleItemDescription.text = selectedBattleItem.description;
    }

    public void OpenItemCharacterChoice()
    {
        itemBattleCharMenu.SetActive(true);

        for (int i = 0; i < itemBattleCharacterNames.Length; i++)
        {
            itemBattleCharacterNames[i].text = GameManager.instance.charStat[i].characterName;
            itemBattleCharacterNames[i].transform.parent.gameObject.SetActive(GameManager.instance.charStat[i].gameObject.activeInHierarchy);

            //var activeBattler = activeBattlers.Where(x => x.name == GameManager.instance.charStat[i].characterName).FirstOrDefault();
            //if (activeBattler.currentHP == 0)
            //{
            //    itemBattleCharacterNames[i].transform.parent.gameObject.SetActive(false);
            //}

            for (int j = 0; j < activeBattlers.Count; j++)
            {
                if (activeBattlers[j].charName == GameManager.instance.charStat[i].characterName)
                {
                    if (activeBattlers[j].currentHP == 0 && selectedBattleItem.itemName != GameManager.instance.referenceItems[0].itemName)
                    {
                        itemBattleCharacterNames[i].transform.parent.gameObject.GetComponent<Button>().interactable = false;
                        break;
                    }
                    else
                    {
                        itemBattleCharacterNames[i].transform.parent.gameObject.GetComponent<Button>().interactable = true;
                        break;
                    }
                }
            }
        }
    }

    public void CloseItemsMenu()
    {
        itemBattleCharMenu.SetActive(false);
        itemsMenu.SetActive(false);
    }

    public void UseItemInBattle(int selectedCharacter)
    {
        selectedBattleItem.UseInBattle(selectedCharacter);
        UpdateUIStats();
        CloseItemsMenu();

        //It depends on how the game is gonna work
        NextTurn();
    }

    public IEnumerator EndBattleCo()
    {
        uiButtonsHandler.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemsMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for (int j = 0; j < GameManager.instance.charStat.Length; j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.charStat[j].characterName)
                    {
                        if (activeBattlers[i].currentHP == 0)
                        {
                            GameManager.instance.charStat[j].currentHP = 1;
                        }
                        else
                        {
                            GameManager.instance.charStat[j].currentHP = activeBattlers[i].currentHP;
                        }

                        GameManager.instance.charStat[j].currentMP = activeBattlers[i].currentMP;
                        GameManager.instance.charStat[j].armorPower = activeBattlers[i].armPower;
                        GameManager.instance.charStat[j].weaponPower = activeBattlers[i].wpnPower;
                        GameManager.instance.charStat[j].strength = activeBattlers[i].strength;
                        GameManager.instance.charStat[j].defense = activeBattlers[i].defense;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        UIFade.instance.FadeFromBlack();

        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;
        if (fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleReward.instance.OpenBattleRewardScreen(rewardXP, rewardItems);
        }

        FindObjectOfType<Camera>().orthographicSize = 7f;

        AudioManager.instance.PlayMusic(FindObjectOfType<CameraController>().musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        FindObjectOfType<Camera>().orthographicSize = 7f;
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        SceneManager.LoadScene(gameOverScene);
    }
}
