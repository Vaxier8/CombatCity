using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    //Waypoints
    public List<Transform> points;
    public EnemyCharacter enemyCharacter;
    public int nextWaypointID; 
    int idChangeValue = 1;



    void Start()
    {
        enemyCharacter = GetComponent<EnemyCharacter>();
        if (enemyCharacter == null)
        {
            Debug.LogError("EnemyCharacter component not found on the GameObject.");
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

        GameObject waypoints = new GameObject("Waypoints");
        waypoints.transform.position = root.transform.position;

        waypoints.transform.SetParent(root.transform);

        GameObject p1 = new GameObject("Point1"); p1.transform.SetParent(waypoints.transform);p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("Point2"); p2.transform.SetParent(waypoints.transform); p2.transform.position = root.transform.position;

        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update()
    {
        MoveToNextPoint();
        //Attacking Logic
        //if (we are in attacking range)
        {
            enemyCharacter.takeDamage(new Random().Next(1, 6)); //RNG damage from [1,6] for now
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

