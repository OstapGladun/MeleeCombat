using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool NewGame = true;
    public bool Map = false;
    public string Class;
    public float supplies;
    public int money;
    public float playerX = 0;
    public float playerY = 0;
    public Vector3[] EnemyPosition;
    public int[] EnemyDestination;
    public string[] LocationStatus;
    //actually locations and enemies, not only locations
    public int CurrentLocationIndex;
    //same here
    public int InitialEventIndex;
    public List<string> Abilities = new List<string>();
    public List<string> knownRumors = new List<string>();
    public List<string> knownEnemies = new List<string>();
    public List<string> knownLocations = new List<string>();
    public List<string> knownQuests = new List<string>();
    public static GameController Instance
    {
        get;
        set;
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
        //no idea how does it work
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Intro")
            SceneManager.LoadScene("Map");
        StartCoroutine(NewGameStarted());
        //remove when intro is done
    }

    IEnumerator NewGameStarted()
    {
        yield return new WaitForSeconds(0.1f);
        NewGame = false;
    }
}
