using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleKnight : MonoBehaviour
{
    [SerializeField]private Sprite IdleAnimation;
    [SerializeField]private Sprite BlockAnimation;
    [SerializeField]private Sprite SlashAnimation;
    private GameObject HealthPrefab;
    private GameObject ArmorPrefab;
    private GameObject ShieldIconPrefab;
    private GameObject ParryIconPrefab;
    [SerializeField]private Text Log;
    [SerializeField]private Text Results;
    [SerializeField]private SpriteRenderer BattleBackground;
    public int[] Reward;
    public int health = 2;
    public int armor = 2;
    public int blocks = 0;
    public int stamina = 3;
    public string status = "New_turn";
    public string attack;
    public bool IsParry = false;
    public bool IsStunned = false;
    private bool CRrunning = false;
    private float HealthPosition;
    private float ArmorPosition;
    public bool AbilityHighlightened = false;
    [SerializeField]private GameObject VictoryUI;
    [SerializeField]private Canvas canvas;
    private HealthSystem HealthSystemScript;
    private GameController AssetHolderScript;

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

    void InstantiateAbility(string AbilityName, float n)
    {
        GameObject Ability = Instantiate(Resources.Load<GameObject>(AbilityName), Vector3.zero, Quaternion.identity);
        //create ability button

        Ability.GetComponent<Ability>().Character = GameObject.Find("Knight");
        Ability.GetComponent<Ability>().Stamina = GameObject.Find("Stamina").GetComponent<Image>();
        Ability.GetComponent<Ability>().hintText = GameObject.Find("Hint").GetComponent<Text>();
        //set variables 

        RectTransform rectTransform = Ability.GetComponent<RectTransform>();
        Ability.transform.SetParent(canvas.transform);
        //put ability button to canvas

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(350, 350);
        //set anchor in the left bottom corner for ability button

        rectTransform.position = new Vector2(80 + 125 * n, 70);
        //position ability button
    }

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
        Results.text = "You loot fallen enemies and get " + Reward[0] + " supplies and " + Reward[1] + " silver florins. \n";
        if (health < Movement.maxHealth)
        {
            Results.text += "You spend " + (Movement.maxHealth - health) * 2 + " supplies to heal your wounds. \n";
        }
        Results.text += "With experience you got on battlefield you are able to master new battle ability:";
    }

    void Start()
    {
        //set background according to landscape where battle occurs
        if (AssetHolderScript!=null && AssetHolderScript.BattleLandscape != null)
        {
            BattleBackground.sprite = Resources.Load<Sprite>(AssetHolderScript.BattleLandscape);
            AssetHolderScript.BattleLandscape = null;
        }
        //generate health and armor indicator above character
        HealthPrefab = Resources.Load<GameObject>("health-full");
        ArmorPrefab = Resources.Load<GameObject>("armor-full");
        ShieldIconPrefab = Resources.Load<GameObject>("block-icon");
        ParryIconPrefab= Resources.Load<GameObject>("parry-icon");
        HealthSystemScript = HealthPrefab.GetComponent<HealthSystem>();
        for (int i = 0; i < Movement.maxHealth; i++)
        {
            HealthSystemScript.Unit = gameObject;
            HealthSystemScript.index = i + 1;
            Instantiate(HealthPrefab, new Vector3(transform.position.x - 1.1f + i * 0.7f, transform.position.y + 1.6f, 0), Quaternion.identity, gameObject.transform);
            HealthPosition = i;
        }
        HealthSystemScript = ArmorPrefab.GetComponent<HealthSystem>();
        for (int j = 0; j < Movement.maxArmor; j++)
        {
            HealthSystemScript.Unit = gameObject;
            HealthSystemScript.index = j + 1;
            Instantiate(ArmorPrefab, new Vector3(transform.position.x - 1.1f + ((HealthPosition + 1) * 0.7f) + (j * 0.7f), transform.position.y + 1.6f, 0), Quaternion.identity, gameObject.transform);
            ArmorPosition = j;
        }
        //get abilities from asset holder or create 3 default abilities
        if (GameObject.Find("Asset Holder") != null)
        {
            AssetHolderScript = GameObject.Find("Asset Holder").GetComponent<GameController>();
            float x = 0;
            foreach (string Ability in AssetHolderScript.Abilities)
            {
                InstantiateAbility(Ability, x);
                x += 1;
            }
        }
        else
        {
            InstantiateAbility("block", 0);
            InstantiateAbility("slash", 1);
            InstantiateAbility("parry", 2);
        }
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
                //perform ability
                switch (attack)
                {
                    case "block(Clone)":
                        Log.text = "\n" + "You get 1 block;" + Log.text;
                        GetComponent<SpriteRenderer>().sprite = BlockAnimation;
                        status = "Ability_choice";
                        blocks++;
                        if (blocks == 1)
                        {
                            HealthSystemScript = ShieldIconPrefab.GetComponent<HealthSystem>();
                            HealthSystemScript.index = 101;
                            HealthSystemScript.Unit = gameObject;
                            Instantiate(ShieldIconPrefab, new Vector3(transform.position.x - 1.1f + ((HealthPosition + 1) * 0.7f) + ((ArmorPosition + 1) * 0.7f) + 0.4f, transform.position.y + 1.6f, 0), Quaternion.identity, gameObject.transform);
                            HealthSystemScript.index = 100;
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
                        HealthSystemScript = ParryIconPrefab.GetComponent<HealthSystem>();
                        HealthSystemScript.index = 201;
                        HealthSystemScript.Unit = gameObject;
                        Instantiate(ParryIconPrefab, new Vector3(transform.position.x - 1.1f + ((HealthPosition + 1) * 0.7f) + ((ArmorPosition + 1) * 0.7f) + 0.4f, transform.position.y + 1.6f, 0), Quaternion.identity, gameObject.transform);
                        HealthSystemScript.index = 200;
                        status = "Ability_choice";
                        break;
                }
                break;
        }
    }
}