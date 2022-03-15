using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Ability : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int StaminaCost;
    public bool IsActive;
    public GameObject Character;
    public Image Stamina;
    public Sprite[] StaminaSprites;
    private Vector3 ScaleChange;
    public string hint;
    [SerializeField] public Text hintText;
    private BattleKnight CharacterScript;

    private void Start()
    {
        CharacterScript = Character.GetComponent<BattleKnight>();
        AbilityNotHighlightened();
    }

    void AbilityNotHighlightened()
    {
        CharacterScript.AbilityHighlightened = false;
        ScaleChange = new Vector3(0.28f, 0.28f, 1);
        gameObject.transform.localScale = ScaleChange;
        hintText.text = "Choose an ability to use";
    }

    void Update()
    {
        //set hint depending on status
        if (CharacterScript.status == "New_turn")
        {
            hintText.text = "Choose an ability to use";
            Stamina.GetComponent<Image>().sprite = StaminaSprites[3];
        }
        if (CharacterScript.status == "Ability_choice"&&!CharacterScript.AbilityHighlightened)
        {
            hintText.text = "Choose an ability to use";
        }
        if (CharacterScript.status == "Choose_target")
        {
            hintText.text = "Choose enemy to attack";
        }
        if (CharacterScript.status == "Animation" && CharacterScript.stamina > 0)
        {
            hintText.text = "Performing ability...";
        }
        if (CharacterScript.status == "Enemy_turn")
        {
            hintText.text = "Wait for enemy to attack";
        }
        if (CharacterScript.status == "Victory_screen")
        {
            hintText.text = "Choose an ability to unlock";
            //replace later
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene("Event");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CharacterScript.status == "Ability_choice" && CharacterScript.stamina >= StaminaCost)
        {
            CharacterScript.AbilityHighlightened = true;
            ScaleChange = new Vector3(0.34f, 0.34f, 1);
            gameObject.transform.localScale = ScaleChange;
            hintText.text = hint;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AbilityNotHighlightened();
    }

    public void AbilityPressed()
    {
        if (CharacterScript.status == "Ability_choice" && CharacterScript.stamina >= StaminaCost)
        {
            CharacterScript.stamina -= StaminaCost;
            Stamina.GetComponent<Image>().sprite = StaminaSprites[CharacterScript.stamina];
            switch (IsActive)
            {
                case true:
                    CharacterScript.attack = name;
                    CharacterScript.status = "Choose_target";
                    hintText.text = "Choose enemy to attack";
                    break;
                case false:
                    CharacterScript.attack = name;
                    CharacterScript.status = "Animation";
                    break;
            }
        }
    }
}