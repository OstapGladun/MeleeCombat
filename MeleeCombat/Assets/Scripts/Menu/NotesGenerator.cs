using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesGenerator : MonoBehaviour
{
    [SerializeField] private Text NoteText;
    [SerializeField] private Text EntrySample;
    private Button btn;
    private Text LastEntry;
    private UnityEngine.Events.UnityAction buttonCallback;
    private GameController script1;
    private MenuManagement script2;

    private void Start()
    {
        btn = EntrySample.GetComponent<Button>();
        GenerateListOfNotes("Quests");
    }

    public void GenerateListOfNotes(string notePage)
    {
        script1 = GameObject.Find("Asset Holder").GetComponent<GameController>();
        script2 = GameObject.Find("Asset Holder").GetComponent<MenuManagement>();
        float entryY = 485;
        List<string> listOfEntries = new List<string> { };
        ClearList();
        switch (notePage)
        {
            case "Quests":
                listOfEntries = script1.knownQuests;
                break;
            case "Locations":
                listOfEntries = script1.knownLocations;
                break;
            case "Enemies":
                listOfEntries = script1.knownEnemies;
                break;
            case "Rumors":
                listOfEntries = script1.knownRumors;
                break;
        }
        //Get list of entries you need

        foreach (string Entry in listOfEntries)
        {
            EntrySample.text = Entry;
            LastEntry = Instantiate(EntrySample, new Vector3(240, entryY, 0), Quaternion.identity);
            LastEntry.transform.SetParent(transform);
            entryY -= 45;
            //place button
        }
    }
    private void ClearList()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
