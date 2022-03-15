using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pathfinding;

public class Movement : MonoBehaviour
{
    public static int maxHealth = 2;
    public static int maxArmor = 2;
    private float supplies = 15;
    public int money = 15;
    public string Class = "Knight";
    [SerializeField] private Text Supplies;
    [SerializeField] private Text SuppliesMenu;
    [SerializeField] private Text MoneyMenu;
    [SerializeField] private GameObject TargetMarker;
    [SerializeField] private float speed;
    private Vector3 mousePosition;
    public Vector3 targetPosition;
    public bool isMoving = false;
    private GameController script;

    void MapLoad()
    {
        if (!script.NewGame)
        {
            supplies = script.supplies;
            money = script.money;
            Class = script.Class;
            transform.position = new Vector3(script.playerX, script.playerY-0.3f, 0);
            TargetMarker.transform.position = new Vector3(script.playerX, script.playerY-0.3f, 0);
            //get player's characteristics from asset holder
        }
    }
    void Start()
    {
        script = GameObject.Find("Asset Holder").GetComponent<GameController>();
        StartCoroutine(SupplyConsumption());
        MapLoad();
    }

    private IEnumerator SupplyConsumption()
    {
        if (!GetComponent<AIPath>().reachedEndOfPath)
        {
            supplies -= 0.015f;
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(SupplyConsumption());
        //if moving, consume x supplies every 0.1 seconds
    }

    void Update()
    {
        script.supplies = supplies;
        script.money = money;
        script.Class = Class;
        script.playerX = transform.position.x;
        script.playerY = transform.position.y;
        //save current player's characteristics to asset holder
        Supplies.text = "Supplies:" + Mathf.Round(supplies);
        SuppliesMenu.text = "Supplies:" + Mathf.Round(supplies);
        MoneyMenu.text = "Money:" + Mathf.Round(money);
        Supplies.color = Color.black;
        //display player's money and supplies on map and in menu
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            TargetMarker.SetActive(false);
            mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            TargetMarker.SetActive(true);
            TargetMarker.transform.position = targetPosition;
        }
        //set moving and pathfinding target with click
        if (isMoving)
        {
            Move();
        }
        //useless(replaced with A* pathfinder)
        if (supplies < 5)
        {
            Supplies.text = "Dangerously low food supplies:" + Mathf.Round(supplies);
            Supplies.color = Color.red;
        }
        if (supplies <= 0)
        {   
            SceneManager.LoadScene("Starvation");
        }
        //check player's supplies: warn him, or end game
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if(transform.position == targetPosition)
        {
            TargetMarker.SetActive(false);
            isMoving = false;
        }
        //useless(replaced with A* pathfinder)
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //save exact object, player interacted with
        if(collision.tag == "patrol")
        {
            script.CurrentLocationIndex = collision.GetComponent<MovementEnemy>().Index;
            script.LocationStatus[script.CurrentLocationIndex] = "visited";
            script.InitialEventIndex = 1;
            script.BattleLandscape = GetComponentInChildren<LandscapeDetection>().landscapeName;
            SceneManager.LoadScene("Event");
        }
        if (collision.name == "Mill")
        {
            script.CurrentLocationIndex = collision.GetComponent<Location>().Index;
            script.LocationStatus[script.CurrentLocationIndex] = "visited";
            script.InitialEventIndex = 5;
            SceneManager.LoadScene("Event");
        }
    }
}