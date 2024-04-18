using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitboxCollisionEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerCharacter player;
    private PlayerMovement pmv;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        player = col.GetComponent<PlayerCharacter>();
        pmv = col.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.takeDamage(new System.Random().Next(1, 6));
            pmv.Knockback(10f);
           
            Destroy(gameObject);
        }
    }
}
