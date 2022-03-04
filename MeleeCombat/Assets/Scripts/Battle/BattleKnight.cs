using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleKnight : MonoBehaviour
{
    public Sprite IdleAnimation;
    public Sprite BlockAnimation;
    public Sprite SlashAnimation;
    public GameObject HealthPrefab;
    public GameObject ArmorPrefab;
    public GameObject ShieldIconPrefab;
    public GameObject ShieldIcon;
    public GameObject ParryIconPrefab;
    public GameObject Block;
    public GameObject Slash;
    public GameObject Parry;
    public GameObject VictoryUI;
    public Text Log;
    public Text Results;
    public int[] Reward;
    public int health = 2;
    public int armor = 2;
    public int blocks = 0;
    public string status = "New_turn";
    public int stamina = 3;
    public string attack;
    public bool IsParry = false;
    public bool IsStunned = false;
    private bool CRrunning = false;
    private HealthSystem script;
    private float HealthPosition;
    private float ArmorPosition;
    public bool AbilityHighlightened = false;

    IEnumerator Defeat()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Defeat");
    }

    IEnumerator Victory()
    {
        VictoryUI.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Results.text = "You loot fallen enemies and get " + Reward[0] + " supplies. \n";
        if (health < Movement.maxHealth)
        {
            Results.text += "You spend " + (Movement.maxHealth - health) * 2 + " supplies to heal your wounds. \n";
        }
        Results.text += "With experience you got on battlefield you are able to master new battle ability:";
    }

    public void GetDamage(int damage, bool IgnoreArmor, bool Stun)
    {
        if (IsParry)
        {
            AttackAnimation("Block");
            Log.text = "\n" + "Attack parried, enemy is stunned;" + Log.text;
        }
        else if (blocks > 0)
        {
            blocks -= damage;
            Log.text = "\n" + "Attack blocked;" + Log.text;
        }
        else if (armor > 0 && !IgnoreArmor)
        {
            armor -= damage;
            Log.text = "\n" + "Your armor gets " + damage + " damage;" + Log.text;

        }
        else
        {
            health -= damage;
            Log.text = "\n" + "Your health gets " + damage + " damage;" + Log.text;
        }
        if (Stun)
        {
            Stun = true;
            Log.text = "\n" + "You are stunned;" + Log.text;
        }
    }

    IEnumerator AttackedAnimation()
    {
        transform.position += new Vector3(1, 0, 0);
        yield return new WaitForSeconds(1);
        if (health >= 1)
            transform.position -= new Vector3(1, 0, 0);
    }

    void SpriteManager()
    {
        GetComponent<SpriteRenderer>().sprite = IdleAnimation;
        if (IsParry == true)
            GetComponent<SpriteRenderer>().sprite = SlashAnimation;
        if (blocks > 0)
            GetComponent<SpriteRenderer>().sprite = BlockAnimation;
    }

    IEnumerator Renew()
    {
        Log.text = "\n" + "New round has began;" + Log.text;
        CRrunning = true;
        stamina = 3;
        blocks = 0;
        IsParry = false;
        SpriteManager();
        yield return new WaitForSeconds(0.001f);
        status = "Ability_choice";
        CRrunning = false;
    }

    IEnumerator AttackAnimation(string Sprite)
    {
        switch (Sprite)
        {
            case "Attack":
                GetComponent<SpriteRenderer>().sprite = SlashAnimation;
                break;
            case "Block":
                GetComponent<SpriteRenderer>().sprite = BlockAnimation;
                break;
        }
        transform.position += new Vector3(1, 0, 0);
        yield return new WaitForSeconds(1f);
        transform.position -= new Vector3(1, 0, 0);
        SpriteManager();
        status = "Ability_choice";
    }

    void Start()
    {
        script = HealthPrefab.GetComponent<HealthSystem>();
        for (int i = 0; i < Movement.maxHealth; i++)
        {
            script.Unit = gameObject;
            script.index = i + 1;
            Instantiate(HealthPrefab, new Vector3(transform.position.x - 1.1f + i * 0.7f, transform.position.y + 1.6f, 0), Quaternion.identity);
            HealthPosition = i;
        }
        script = ArmorPrefab.GetComponent<HealthSystem>();
        for (int j = 0; j < Movement.maxArmor; j++)
        {
            script.Unit = gameObject;
            script.index = j + 1;
            Instantiate(ArmorPrefab, new Vector3(transform.position.x - 1.1f + ((HealthPosition + 1) * 0.7f) + (j * 0.7f), transform.position.y + 1.6f, 0), Quaternion.identity);
            ArmorPosition = j;
        }
        //add ability system
        Instantiate(Block, new Vector3(-7.9f, -4, 0), Quaternion.identity);
        Instantiate(Slash, new Vector3(-5.9f, -4, 0), Quaternion.identity);
        Instantiate(Parry, new Vector3(-3.9f, -4, 0), Quaternion.identity);
        VictoryUI.SetActive(false);
    }

    void Update()
    {
        if (health<=0)
        {
            StartCoroutine(Defeat());
        }
        if (GameObject.FindGameObjectsWithTag("enemy").Length == 0)
        {
            StartCoroutine(Victory());
            status = "Victory_screen";
        }
        switch (status)
        {
            case "New_turn":
                if (CRrunning == false)
                    StartCoroutine(Renew());
                break;
            case "Ability_choice":
                if (stamina == 0)
                {
                    SpriteManager();
                    status = "Enemy_turn";
                }
                break;
            case "Animation":
                switch (attack)
                {
                    case "block(Clone)":
                        Log.text = "\n" + "You get 1 block;" + Log.text;
                        GetComponent<SpriteRenderer>().sprite = BlockAnimation;
                        status = "Ability_choice";
                        blocks++;
                        if (blocks == 1)
                        {
                            script = ShieldIconPrefab.GetComponent<HealthSystem>();
                            script.index = 101;
                            script.Unit = gameObject;
                            Instantiate(ShieldIconPrefab, new Vector3(transform.position.x - 1.1f + ((HealthPosition + 1) * 0.7f) + ((ArmorPosition + 1) * 0.7f) + 0.4f, transform.position.y + 1.6f, 0), Quaternion.identity);
                            script.index = 100;
                        }
                        break;
                    case "slash(Clone)":
                        if (GetComponent<SpriteRenderer>().sprite != SlashAnimation)
                        {
                            StartCoroutine(AttackAnimation("Attack"));
                        }
                        break;
                    case "parry(Clone)":
                        Log.text = "\n" + "You are now in parry stance;" + Log.text;
                        IsParry = true;
                        script = ParryIconPrefab.GetComponent<HealthSystem>();
                        script.index = 201;
                        script.Unit = gameObject;
                        Instantiate(ParryIconPrefab, new Vector3(transform.position.x - 1.1f + ((HealthPosition + 1) * 0.7f) + ((ArmorPosition + 1) * 0.7f) + 0.4f, transform.position.y + 1.6f, 0), Quaternion.identity);
                        script.index = 200;
                        status = "Ability_choice";
                        break;
                }
                break;
        }
    }
}