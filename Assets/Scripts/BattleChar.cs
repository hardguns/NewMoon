using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    //Checks if the gameObject is an enemy or a player
    public bool isPlayer;

    //Saves how many moves are available
    public string[] movesAvailable;

    //Stores the character name
    public string charName;

    //Player/Enemy Stats
    public int currentHP, maxHP, currentMP, maxMP, strength, defense, wpnPower, armPower;

    //Checks if player or the enemy has died in battle
    public bool hasDied;

    //Look for the SpriteRenderer component to be replaced
    public SpriteRenderer battlerSprite;

    //Sprite to be replaced
    public Sprite aliveSprite, deadSprite;

    //Adding fade effect to Enemies
    public bool shouldFade;
    public float fadeSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        battlerSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFade)
        {
            battlerSprite.color = new Color(Mathf.MoveTowards(battlerSprite.color.r, 1f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(battlerSprite.color.g, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(battlerSprite.color.b, 0f, fadeSpeed * Time.deltaTime), Mathf.MoveTowards(battlerSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (battlerSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        } 
    }

    public void EnemyFade()
    {
        shouldFade = true;
    }
}
