using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetWall : MonoBehaviour
{
    AreaEffector2D area;
    void Start()
    {
        area = GetComponent<AreaEffector2D>();
        //target = FindObjectOfType<PlayerController>().gameObject.transform;
        //initialD = Vector2.Distance(transform.position, target.position);
        //rigidbody2D.isKinematic = true;
    }

    void Update()
    {
        if (!hasMetalItem())
        {
            area.forceMagnitude = 0f;
        }
        else
        {
            area.forceMagnitude = -200f;
        }
    }

    public bool hasMetalItem()
    {
        for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
        {
            if (GameManager.instance.itemsHeld[i] != null && GameManager.instance.itemsHeld[i] != "")
            {
                var item = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]);

                if (item.itemMaterial == Item.ItemMaterial.Metal)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
