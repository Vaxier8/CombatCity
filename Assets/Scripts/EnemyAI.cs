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
    public List<Transform> points;
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
        if(type.Equals("Chaser"))
        {
                player = FindObjectOfType<PlayerCharacter>();
                if (player == null)
                {
                    Debug.LogError("PlayerCharacter component not found in the scene.");
                }
        }
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
    private void Die()
    {
        scoreBoard.addScore(100);
        Destroy(gameObject);
    }
    void chasePlayer()
    {
        //Placeholder until I want to work out A* pathfinding for platforming (pain)
        if(player.transform != null)
        {

            float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
            Debug.DrawRay(transform.position, new Vector2(direction, 0) * 10, Color.red);
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
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
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
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }  
            }
        }
    }
    void MoveToNextPoint()
    {
        Transform goalPoint = points[nextWaypointID];

        //logic for flipping enemy
        if (goalPoint.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
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

