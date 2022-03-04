using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class EventManagement : MonoBehaviour
{
    private int EventIndex = 1;
    [SerializeField] private Text Description;
    [SerializeField] private GameObject Image;
    [SerializeField] private Text Choice1;
    [SerializeField] private Text Choice2;
    [SerializeField] private Text Choice3;
    [SerializeField] private string[] Choices;
    [SerializeField] private Texture2D[] Images;
    [SerializeField] private string[] Scenes;
    [SerializeField] private string[] SpecialEvents;
    [SerializeField] private int[,] EventData = new int[100,13];
    private GameController script;

    public void EventChosen(int EventNumber)
    {
        switch (EventNumber)
        {
            case 1:
                EventIndex = EventData[EventIndex, 5];
                break;
            case 2:
                EventIndex = EventData[EventIndex, 6];
                break;
            case 3:
                EventIndex = EventData[EventIndex, 7];
                break;
        }
        Event();
        //move on to next event after making choice
    }

    void Event()
    {
        if (EventData[EventIndex, 0] != 0)
            Description.text = FindText(EventIndex);
        //set description for event
        if (EventData[EventIndex, 1] != 0)
            Image.GetComponent<RawImage>().texture = Images[EventData[EventIndex, 1]];
        //set image for event
        if (EventData[EventIndex, 2] != 0)
        {
            Choice1.text = Choices[EventData[EventIndex, 2]];
            Choice1.enabled = true;
        }
        else
            Choice1.enabled = false;
        //if there is first choice, set and show it
        if (EventData[EventIndex, 3] != 0)
        {
            Choice2.text = Choices[EventData[EventIndex, 3]];
            Choice2.enabled = true;
        }
        else
            Choice2.enabled = false;
        //if there is second choice, set and show it
        if (EventData[EventIndex, 4] != 0)
        {
            Choice3.text = Choices[EventData[EventIndex, 4]];
            Choice3.enabled = true;
        }
        else
            Choice3.enabled = false;
        //if there is third choice, set and show it
        script.supplies += EventData[EventIndex, 8];
        script.money += EventData[EventIndex, 9];
        //change player's resources
        SpecialEvent(SpecialEvents[EventData[EventIndex, 11]]);
        //initiate special event
        if (EventData[EventIndex, 10] != 0)
        {
            if (!SpecialEvents[EventData[EventIndex, 11]].Contains("event after fight"))
                script.InitialEventIndex = 3;
            SceneManager.LoadScene(Scenes[EventData[EventIndex, 10]]);
        }
        //load scene
    }

    string FindText(int EventIndex)
    {
        string path = "Assets/Resources/EventText.txt";
        string finalText = "";
        var startFound = false;
        StreamReader reader = new StreamReader(path);
        //set variables and start reading txt file
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;
            //skip  line if it's empty
            if (!startFound)
            {
                startFound = line.Contains("#" + EventData[EventIndex,0]);
                continue;
            }
            //scan file until text block with required index is found, than start saving it
            if (startFound)
            {
                var isEnd = line.Contains("-------------------------------------------------------------------");
                if (!isEnd)
                {
                    if (line.Contains(">"))
                        line = "\n";
                    //add new line symbol as current line
                    finalText += line;
                    //add another line to final result
                }
                else
                    break;
                //check if block of text for event ends
            }
        }
        reader.Close();
        return finalText;
        //return final block of text
    }

    void Start()
    {
        /*event values:
        0 - Description
        1 - Image
        2 - Choice 1 text
        3 - Choice 2 text
        4 - Choice 3 text
        5 - Choice 1 next event
        6 - Choice 2 next event
        7 - Choice 3 next event
        8 - Supplies change
        9 - Money change
        10 - Next scene
        11 - Special event*/

        //set whole archive of different events:

        // patrol encountered - 1
        EventData [1, 0] = 1;
        EventData[1, 1] = 1;
        EventData[1, 2] = 1;
        EventData[1, 3] = 2;
        EventData[1, 4] = 3;
        EventData[1, 5] = 2;
        EventData[1, 6] = 4;
        EventData[1, 7] = Random.Range(2,4);
        EventData[1, 11] = 1;

        //patrol fight - 2
        EventData[2, 10] = 2;
        EventData[2, 11] = 1;

        //return to map - 3
        EventData[3, 10] = 1;

        //patrol bribed - 4
        EventData[4, 0] = 2;
        EventData[4, 1] = 2;
        EventData[4, 2] = 4;
        EventData[4, 5] = 3;
        EventData[4, 9] = -10;

        //mill visited - 5
        EventData[5, 0] = 3;
        EventData[5, 1] = 3;
        EventData[5, 2] = 5;
        EventData[5, 3] = 6;
        EventData[5, 5] = 6;
        EventData[5, 6] = 7;

        //mill entered - 6
        EventData[6, 0] = 4;
        EventData[6, 1] = 4;
        EventData[6, 2] = 9;
        EventData[6, 3] = 10;
        EventData[6, 5] = 10;
        EventData[6, 6] = 11;

        //outside of the mill - 7
        EventData[7, 0] = 5;
        EventData[7, 1] = 5;
        EventData[7, 2] = 7;
        EventData[7, 3] = 8;
        EventData[7, 4] = 4;
        EventData[7, 5] = 8;
        EventData[7, 6] = 9;
        EventData[7, 7] = 3;

        //asked miller about food - 8
        EventData[8, 0] = 6;
        EventData[8, 1] = 6;
        EventData[8, 2] = 4;
        EventData[8, 5] = 3;

        //asked miller what happened - 9
        EventData[9, 0] = 7;
        EventData[9, 1] = 6;
        EventData[9, 2] = 4;
        EventData[9, 5] = 3;
        EventData[9, 11] = 2;

        //protecting miller - 10
        EventData[10, 10] = 3;
        EventData[10, 11] = 4;

        //not protecting miller - 11
        EventData[11, 0] = 8;
        EventData[11, 1] = 4;
        EventData[11, 2] = 4;
        EventData[11, 5] = 3;
        EventData[11, 11] = 3;

        //miller protected - 12
        EventData[12, 0] = 9;
        EventData[12, 1] = 6;
        EventData[12, 2] = 7;
        EventData[12, 3] = 8;
        EventData[12, 5] = 13;
        EventData[12, 6] = 14;

        //asked miller about food after fight - 13
        EventData[13, 0] = 10;
        EventData[13, 1] = 6;
        EventData[13, 2] = 4;
        EventData[13, 5] = 3;

        //asked miller what happened after fight - 14
        EventData[14, 0] = 11;
        EventData[14, 1] = 6;
        EventData[14, 2] = 4;
        EventData[14, 5] = 3;
        EventData[14, 11] = 2;

        script = GameObject.Find("Asset Holder").GetComponent<GameController>();
        EventIndex = script.InitialEventIndex;
        Event();
        //launch first event
    }

    private void SpecialEvent(string Event)
    {
        if (Event.Contains("destroy"))
            script.LocationStatus[script.CurrentLocationIndex] = "destroyed";
        if (Event.Contains("give map"))
            script.Map = true;
        if (Event.Contains("add rumor") && !script.knownRumors.Contains(Event.Replace("add rumor ", "")))
        {
            script.knownRumors.Add(Event.Replace("add rumor ",""));
        }
        if (Event.Contains("event after fight"))
        {
            script.InitialEventIndex = System.Int32.Parse(Event.Replace("event after fight ", ""));
        }
        //act depending on special event
    }
}
