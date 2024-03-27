using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private Character character;
    public Image healthbar;
    public float maxHealth = 100f;

    void Start()
    {
        character = FindObjectOfType<PlayerCharacter>();
        if (character == null)
        {
            Debug.LogError("Character component not found on the GameObject.");
            return;
        }

        maxHealth = character.health;
    }
    void Update()
    {
         healthbar.fillAmount = character.health / 100f;

    }


}
