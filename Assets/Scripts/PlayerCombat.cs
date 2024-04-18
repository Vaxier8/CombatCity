using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    private bool attackCooldown;
    private PlayerCharacter playerCharacter;
    private PlayerMovement playerMovement;
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private Transform hitboxSpawnPoint;
    public Animator animator;
    void Start()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        if (playerCharacter == null)
        {
            Debug.LogError("PlayerCharacter component not found on the GameObject.");
        }
        playerMovement= GetComponent<PlayerMovement>();
        if (playerCharacter == null)
        {
            Debug.LogError("PlayerMovement component not found on the GameObject.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(attackCooldown == false)
            {
                Vector3 attackOffset;
                if(playerMovement.getFacingRight())
                {
                    attackOffset = new Vector3(1,0,0);
                }
                else
                {
                attackOffset = new Vector3(-1,0,0);
                }
                attackCooldown = true;
                Attack(attackOffset, new Vector3(1f,.5f,0.25f));
            }
        }
        if(Input.GetButtonDown("Fire2"))
        {
            if(attackCooldown == false)
            {
                Vector3 attackOffset;
                if(playerMovement.getFacingRight())
                {
                    attackOffset = new Vector3(1,0.67f,0);
                }
                else
                {
                attackOffset = new Vector3(-1,0.67f,0);
                }
                attackCooldown = true;
                Attack(attackOffset, new Vector3(2f,2f,0.25f));
            }
        }
    }
    void Attack(Vector3 offset, Vector3 scale)
    {
        animator.SetBool("IsPunching", true);
        Vector3 offsetPosition = hitboxSpawnPoint.position + offset;
        GameObject hitbox = Instantiate(hitboxPrefab, offsetPosition, hitboxSpawnPoint.rotation);
        hitbox.transform.localScale = scale;
        hitbox.transform.SetParent(this.transform, true);

        StartCoroutine(AttackWindow(hitbox, .5f));
    }

    IEnumerator AttackWindow(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.SetBool("IsPunching", false);
        // Then destroy the object
        attackCooldown = false;
        Destroy(objectToDestroy);
    }
}
