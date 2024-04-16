using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using System;
using UnityEditor.Callbacks;
using Unity.VisualScripting;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{

    //Waypoints
    private bool attackCooldown;
    private bool withinRange = false;
    [SerializeField] private GameObject hitboxEnemyPrefab;
    [SerializeField] private Transform hitboxSpawnPoint;
    public List<Transform> points;
    private bool isFacingRight = true;
    public EnemyCharacter enemyCharacter;
    private PlayerCharacter player = null;
    private ScoreBoardManager scoreBoard;
    private bool persuing = false;
    private float persueTime = 1f;
    public int nextWaypointID; 
    int idChangeValue = 1;
    public String type;


    void Start()
    {

        enemyCharacter = GetComponent<EnemyCharacter>();
        if (enemyCharacter != null)
        {
            type = enemyCharacter.BehaviorType;
        }
        if (enemyCharacter == null)
        {
            Debug.LogError("EnemyCharacter component not found on the GameObject.");
        }
        scoreBoard = FindObjectOfType<ScoreBoardManager>();
        if (scoreBoard == null)
        {
            Debug.LogError("ScoreBoardManager component not found in the scene.");
        }
        player = FindObjectOfType<PlayerCharacter>();
        if (player == null)
        {
            Debug.LogError("PlayerCharacter component not found in the scene.");
         }
        //if(type.Equals("Chaser"))
    //    {
      //  }
    }
    private void Reset()
    {
        Initialize();

    }

    void Initialize()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;

        //Creates the root object
        GameObject root = new GameObject(name + "_Root");

        //Reset position of Root to enemy object
        root.transform.position = transform.position;

        //Makes object a child of the root
        transform.SetParent(root.transform);
        switch(type)
        {
            case "Waypoint":
            {
                GameObject waypoints = new GameObject("Waypoints");
                waypoints.transform.position = root.transform.position;

                waypoints.transform.SetParent(root.transform);

                GameObject p1 = new GameObject("Point1"); p1.transform.SetParent(waypoints.transform);p1.transform.position = root.transform.position;
                GameObject p2 = new GameObject("Point2"); p2.transform.SetParent(waypoints.transform); p2.transform.position = root.transform.position;

                points = new List<Transform>();
                points.Add(p1.transform);
                points.Add(p2.transform);
                break;
            }
            case "Chaser":
            {
                break;
            }

        }
    }

    private void Update()
    {
        float direction;
        if(isFacingRight)
        {
            direction = Mathf.Sign(1);
        }
        else
        {
            direction = Mathf.Sign(-1);
        }
        Debug.DrawRay(transform.position, new Vector2(direction, 0) * 1, Color.blue);
        int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
        layerMask = ~layerMask;
        RaycastHit2D inPlayerRange = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 1, layerMask);
        if(inPlayerRange.collider != null && inPlayerRange.collider.name == "Player")
        {
            //Debug.Log(inPlayerRange.collider.name);
            withinRange = true;
        }
        else
        {
            withinRange = false;
        }
        if(withinRange)
        {
            Attack();
        }
        switch(type)
        {
            case "Waypoint":
            {
                MoveToNextPoint();
                break;
            }
            case "Chaser":
            {
                chasePlayer();
                break;
            }
        }
        

        if(enemyCharacter.health <= 0)
        {
            Die();
        }
    }

    void Attack()
    {
                if(attackCooldown == false)
                {
                    Vector3 attackOffset;
                    if(isFacingRight)
                    {
                        attackOffset = new Vector3(1,0,0);
                    }
                    else
                    {
                    attackOffset = new Vector3(-1,0,0);
                    }
                    attackCooldown = true;
                    Attack2(attackOffset, new Vector3(0.25f,0.25f,0.25f));
                }
    }

    void Attack2(Vector3 offset, Vector3 scale)
    {
        //animator.SetBool("IsPunching", true);
        Vector3 offsetPosition = hitboxSpawnPoint.position + offset;
        GameObject hitbox = Instantiate(hitboxEnemyPrefab, offsetPosition, hitboxSpawnPoint.rotation);
        hitbox.transform.localScale = scale;
        hitbox.transform.SetParent(this.transform, true);

        StartCoroutine(AttackWindow(hitbox, 1f));
    }

    IEnumerator AttackWindow(GameObject objectToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

        //animator.SetBool("IsPunching", false);
        // Then destroy the object
        attackCooldown = false;
        Destroy(objectToDestroy);
    }
    private void Die()
    {
        scoreBoard.addScore(100);
        Destroy(gameObject);
    }
    void chasePlayer()
    {
        if(withinRange)
        {
            return;
        }
        //Placeholder until I want to work out A* pathfinding for platforming (pain)
        if(player.transform != null)
        {

            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
            //Debug.DrawRay(transform.position, new Vector2(direction, 0) * 10, Color.red);
            int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            layerMask = ~layerMask;
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 10, layerMask);
            if(hitPlayer.collider != null && hitPlayer.collider.name == "Player")
            {
                persuing = true;
                persueTime = 1f;
                Vector2 xPos = new Vector2(player.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, xPos, enemyCharacter.speed*Time.deltaTime);
                            //logic for flipping enemy
                if (player.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFacingRight = true;
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFacingRight = false;
                }
            }
            else if (persuing)
            {
                persueTime -= Time.deltaTime;
                if(persueTime <= 0)
                {
                    persueTime = 1f;
                    persuing = false;
                }
                Vector2 xPos = new Vector2(player.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, xPos, enemyCharacter.speed*Time.deltaTime);
                            //logic for flipping enemy
                if (player.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    isFacingRight = true;
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    isFacingRight = false;
                }  
            }
        }
    }
    void MoveToNextPoint()
    {
        if(withinRange)
        {
            return;
        }
        Transform goalPoint = points[nextWaypointID];

        //logic for flipping enemy
        if (goalPoint.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = true;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }
        
        //Move towards goalpoint
        transform.position = Vector2.MoveTowards(transform.position,goalPoint.position,enemyCharacter.speed*Time.deltaTime);

        if (Vector2.Distance(transform.position, goalPoint.position) < 1f) 
        {
            if (nextWaypointID == points.Count - 1)
                idChangeValue = -1;

            if (nextWaypointID == 0)
                idChangeValue = 1;

            nextWaypointID += idChangeValue;
        }

    }
}

