using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability {} //Placeholder

public class Character : MonoBehaviour
{
    public int health;
    public float speed;
    public List<Ability> abilities;
    public float jumpingPower;
    public bool isInvulnerable = false;

    public void TakeDamage(int damage)
    {
        if (damage >= health)
            health = 0;

        else
            health -= damage;
    }

    
}
