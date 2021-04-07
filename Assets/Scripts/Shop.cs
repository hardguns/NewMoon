using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    //Static instance of the shop to use it in other scripts
    public static Shop instance;

    //ShopMenu: is all the shop menu
    //BuyMenu: is the buy panel in shop menu
    //SellMenu: is the sell panel in shop menu
    public GameObject ShopMenu;
    public GameObject BuyMenu;
    public GameObject SellMenu;

    //Shows the amount of gold
    public Text goldText;

    //Shows the items shop has for sale
    public string[] itemsForSale = new string[36];

    //Buy and sell button arrays to show the items to buy and current items to sell
    public ItemButton[] buyItemButton;
    public ItemButton[] sellItemButton;

    //Text objects to asign text values (buy & sell)
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    //Active item
    public Item selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !ShopMenu.activeInHierarchy)
        {
            OpenShop();
        }
    }

    public void OpenShop()
    {
        ShopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void CloseShop()
    {
        ShopMenu.SetActive(false);

        GameManager.instance.shopActive = false;
    }

    public void OpenBuyMenu()
    {
        selectedItem = null;
        buyItemName.text = "Select an item to buy";
        buyItemDescription.text = "Here you'll see details of the object";
        buyItemValue.text = "Value: ";

        BuyMenu.SetActive(true);
        SellMenu.SetActive(false);

        for (int i = 0; i < buyItemButton.Length; i++)
        {
            buyItemButton[i].buttonValue = i;

            if (itemsForSale[i] != "")
            {
                buyItemButton[i].buttonImage.gameObject.SetActive(true);
                buyItemButton[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButton[i].amountText.text = "";
            }
            else
            {
                buyItemButton[i].buttonImage.gameObject.SetActive(false);
                buyItemButton[i].amountText.text = "";
            }
        }
    }

    public void OpenSellenu()
    {
        selectedItem = null;
        sellItemName.text = "Select an item to sell";
        sellItemDescription.text = "Here you'll see details of the object";
        sellItemValue.text = "Value: ";

        BuyMenu.SetActive(false);
        SellMenu.SetActive(true);

        ShowSellItems();
    }

    public void ShowSellItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < sellItemButton.Length; i++)
        {
            sellItemButton[i].buttonValue = i;

            if (GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButton[i].buttonImage.gameObject.SetActive(true);
                sellItemButton[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButton[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                sellItemButton[i].buttonImage.gameObject.SetActive(false);
                sellItemButton[i].amountText.text = "";
            }
        }
    }

    public void selectedBuyItem(Item buyItem)
    {
        selectedItem = buyItem;

        if (selectedItem != null)
        {
            buyItemName.text = selectedItem.name;
            buyItemDescription.text = selectedItem.description;
            buyItemValue.text = "Value: " + selectedItem.value.ToString() + "g";
        }
    }

    public void selectedSellItem(Item sellItem)
    {
        selectedItem = sellItem;

        if (selectedItem != null)
        {
            sellItemName.text = selectedItem.name;
            sellItemDescription.text = selectedItem.description;
            sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f) + "g";
        }
    }

    public void BuyItem()
    {
        if (selectedItem != null)
        {
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;
                GameManager.instance.AddItem(selectedItem.itemName);
            }

            AudioManager.instance.PlaySFX(9);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void SellItem()
    {
        if (selectedItem != null)
        {
            int itemPosition = FindItemInHeld(selectedItem.itemName);

            if (itemPosition >= 0)
            {
                if (GameManager.instance.numberOfItems[itemPosition] == 1)
                {
                    sellItemName.text = "Select an item to sell";
                    sellItemDescription.text = "Here you'll see details of the object";
                    sellItemValue.text = "Value: ";
                }

                if (GameManager.instance.numberOfItems[itemPosition] > 0)
                {
                    GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);

                    GameManager.instance.RemoveItem(selectedItem.itemName);
                }
            }
            else
            {
                selectedItem = null;
            }

            AudioManager.instance.PlaySFX(8);
        }

        ShowSellItems();

        goldText.text = GameManager.instance.currentGold.ToString() + "g";

    }

    public int FindItemInHeld(string selectedItemName)
    {
        var position = 0;
        for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
        {
            if (GameManager.instance.itemsHeld[i] == selectedItemName)
            {
                position = i;
                break;
            }
            else
            {
                position = -1;
            }
        }

        return position;
    }

}
