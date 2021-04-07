using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{
    private bool canOpen;

    public string[] itemsForSale;

    //If empty won't show dialog
    public string[] linesForDialog;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetButtonDown("Fire1") && PlayerController.player.canMove && !Shop.instance.ShopMenu.activeInHierarchy)
        {
           StartCoroutine(StartShopCO());
        }
    }

    public IEnumerator StartShopCO()
    {
        if (linesForDialog.Length > 0)
        {
            DialogManager.instance.ShowDialog(linesForDialog, true);
        }

        yield return new WaitUntil(() => !GameManager.instance.dialogActive);

        Shop.instance.itemsForSale = itemsForSale;

        Shop.instance.OpenShop();

        AudioManager.instance.PlaySFX(5);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }
    }

}
