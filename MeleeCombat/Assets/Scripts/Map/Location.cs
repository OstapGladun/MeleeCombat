using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public int Index;
    private GameController script;

    void Start()
    {
        script = GameObject.Find("Asset Holder").GetComponent<GameController>();
        if (!script.NewGame)
        {
            switch (script.LocationStatus[Index])
            {
                case "destroyed":
                    Destroy(gameObject);
                    break;
            }
            //get movement properties and status from asset holder
        }
    }
}
