using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HitboxCollisionPlayer : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        EnemyCharacter enemy = col.GetComponent<EnemyCharacter>();
        if (enemy != null)
        {
            // Assuming takeDamage and the damage calculation are still required
            enemy.takeDamage(new System.Random().Next(1, 6));


            StartCoroutine(ChangeColorTemporarily(enemy));
        }
    }

    IEnumerator ChangeColorTemporarily(EnemyCharacter enemy)
    {
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            // Wait for 0.25 seconds
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
        }
        Destroy(gameObject);  
    }
}
