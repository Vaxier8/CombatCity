using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitboxCollisionPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private EnemyCharacter enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        enemy = col.GetComponent<EnemyCharacter>();
        if (enemy != null)
        {
            enemy.takeDamage(new System.Random().Next(1, 6));
        }
    }
}
