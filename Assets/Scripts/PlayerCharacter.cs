using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item { } //PLaceholder

public class PlayerCharacter : Character
{
    public List<Item> inventory = new List<Item>();
    public int experiencePoints = 0;

    public void CollectItem(Item item)
    {
        inventory.Add(item);
    }

    public void LevelUp()
    { 
        if (experiencePoints >= 10) //placeholder
        {
            level += 1;
            experiencePoints = 0; //reset
        }
    }

    public void UseItem(Item item)
    { 
        //needs implemented
    }
}
