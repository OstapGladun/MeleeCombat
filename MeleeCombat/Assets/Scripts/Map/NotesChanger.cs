using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class NotesChanger : MonoBehaviour
{
    [SerializeField]private Text NoteText;

    public void ChangeNote(string noteName)
    {
        if(noteName == "self")
        {
            NoteText.text = FindText(GetComponent<Text>().text);
            //if activated by button
        }
        else
        {
            NoteText.text = FindText(noteName);
            //if activated by map mark
        }
    }

    string FindText(string noteName)
    {
        string path = "Assets/Resources/NoteText.txt";
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
                startFound = line.Contains("#" + noteName);
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
}
