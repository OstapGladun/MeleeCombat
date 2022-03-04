using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Pathfinding;

public class MovementEnemy : MonoBehaviour
{
    [SerializeField] private int CurrentDestination = 0;
    public int Index;
    [SerializeField] private GameObject DestinationObject;
    [SerializeField] private float[] DestinationX;
    [SerializeField] private float[] DestinationY;
    [SerializeField] public float aggroDistance;
    [SerializeField] private bool IsHostile = true;
    private bool IsAggro = false;
    private GameObject Player;
    private GameController script;

    void Start()
    {
        Player = GameObject.Find("Player");
        script = GameObject.Find("Asset Holder").GetComponent<GameController>();
        if (!script.NewGame)
        {
            switch (script.LocationStatus[Index])
            {
                case "destroyed":
                    Destroy(gameObject);
                    break;
                case "visited":
                    StartCoroutine(PeacefulMod(20));
                    break;
            }
            transform.position = script.EnemyPosition[Index];
            CurrentDestination = script.EnemyDestination[Index];
            //get movement properties and status from asset holder
        }
    }

    private void Update()
    {
        script.EnemyPosition[Index] = transform.position;
        script.EnemyDestination[Index] = CurrentDestination;
        IsAggro = false;
        //set variables
        if (GetComponent<AIPath>().reachedDestination)
        {
            CurrentDestination++;
            if (CurrentDestination > DestinationX.Length - 1)
                CurrentDestination = 0;
        }
        DestinationObject.transform.position = new Vector3(DestinationX[CurrentDestination], DestinationY[CurrentDestination], 0);
        //set next waypoint and move destination object to it
        if (Vector2.Distance(transform.position,Player.transform.position) <= aggroDistance && IsHostile)
            IsAggro = true;
        //find player if hostile
        if (!IsAggro)
            GetComponent<AIDestinationSetter>().target = DestinationObject.transform;
        else
            GetComponent<AIDestinationSetter>().target = Player.transform;
        //set target
    }

    IEnumerator PeacefulMod(float Duration)
    {
        IsHostile = false;
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(Duration);
        IsHostile = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }
}