using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagement : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private Text NoteText;
    [SerializeField] private GameObject CharacterPage;
    [SerializeField] private GameObject NotesPage;
    [SerializeField] private GameObject MapPage;
    [SerializeField] private List<GameObject> mapMarks = new List<GameObject>();
    private GameController script;

    void Start()
    {
        script = GameObject.Find("Asset Holder").GetComponent<GameController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
                ShowMenu();
            else
                HideMenu();
        }
        //pause or resume game
    }

    private void ShowMenu()
    {   
        Time.timeScale = 0;
        Menu.SetActive(true);
        //pause game and show menu
        foreach(GameObject Mark in mapMarks)
        {
            Mark.SetActive(script.knownLocations.Contains(Mark.name.Replace("Mark","")));
            //show one of marks if list of known locations contains it
        }
        //generate marks on the map
    }

    private void HideMenu()
    {
        Time.timeScale = 1;
        Menu.SetActive(false);
        //hide menu
    }

    public void PageChange(string pageName)
    {
        switch (pageName)
        {
            case "Character":
                CharacterPage.SetActive(true);
                NotesPage.SetActive(false);
                MapPage.SetActive(false);
                break;
            case "Notes":
                CharacterPage.SetActive(false);
                NotesPage.SetActive(true);
                MapPage.SetActive(false);
                NoteText.text = "Choose an entry to read from the list on the left.";
                GameObject.Find("Content").GetComponent<NotesGenerator>().GenerateListOfNotes("Quests");
                break;
            case "Map":
                CharacterPage.SetActive(false);
                NotesPage.SetActive(false);
                MapPage.SetActive(true);
                break;
        }
        //change page of menu
    }
}
