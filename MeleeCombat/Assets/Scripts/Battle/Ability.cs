using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ability : MonoBehaviour
{
    public int StaminaCost;
    public bool IsActive;
    public GameObject Character;
    public Image Stamina;
    public Sprite[] StaminaSprites;
    private Vector3 ScaleChange;
    public string hint;
    [SerializeField] private Text hintText;

    void AbilityNotHighlightened()
    {
        Character.GetComponent<BattleKnight>().AbilityHighlightened = false;
        ScaleChange = new Vector3(0.3f, 0.3f, 1);
        gameObject.transform.localScale = ScaleChange;
        hintText.text = "Choose an ability to use";
    }

    void Start()
    {

    }

    void Update()
    {
        if (Character.GetComponent<BattleKnight>().status == "New_turn")
        {
            hintText.text = "Choose an ability to use";
            Stamina.GetComponent<Image>().sprite = StaminaSprites[3];
        }
        if (Character.GetComponent<BattleKnight>().status == "Ability_choice"&&!Character.GetComponent<BattleKnight>().AbilityHighlightened)
        {
            hintText.text = "Choose an ability to use";
        }
        if (Character.GetComponent<BattleKnight>().status == "Choose_target")
        {
            hintText.text = "Choose enemy to attack";
        }
        if (Character.GetComponent<BattleKnight>().status == "Animation" && Character.GetComponent<BattleKnight>().stamina > 0)
        {
            hintText.text = "Performing ability...";
        }
        if (Character.GetComponent<BattleKnight>().status == "Enemy_turn")
        {
            hintText.text = "Wait for enemy to attack";
        }
        if (Character.GetComponent<BattleKnight>().status == "Victory_screen")
        {
            hintText.text = "Choose an ability to unlock";
            //replace later
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene("Event");
        }
    }

    void OnMouseOver()
    {
        if (Character.GetComponent<BattleKnight>().status == "Ability_choice" && Character.GetComponent<BattleKnight>().stamina >= StaminaCost)
        {
            Character.GetComponent<BattleKnight>().AbilityHighlightened = true;
            ScaleChange = new Vector3(0.36f, 0.36f, 1);
            gameObject.transform.localScale = ScaleChange;
            hintText.text = hint;
        }
    }

    void OnMouseExit()
    {
        AbilityNotHighlightened();
    }

    private void OnMouseUpAsButton()
    {
        if (Character.GetComponent<BattleKnight>().status == "Ability_choice" && Character.GetComponent<BattleKnight>().stamina >= StaminaCost)
        {
            Character.GetComponent<BattleKnight>().stamina -= StaminaCost;
            Stamina.GetComponent<Image>().sprite = StaminaSprites[Character.GetComponent<BattleKnight>().stamina];
            switch (IsActive)
            {
                case true:
                    Character.GetComponent<BattleKnight>().attack = name;
                    Character.GetComponent<BattleKnight>().status = "Choose_target";
                    hintText.text = "Choose enemy to attack";
                    break;
                case false:
                    Character.GetComponent<BattleKnight>().attack = name;
                    Character.GetComponent<BattleKnight>().status = "Animation";
                    break;
            }
        }
    }
}