using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleEnemy : MonoBehaviour
{
    public int index;
    public int maxHealth;
    public int maxArmor;
    [SerializeField]private Text Log;
    [SerializeField]private GameObject Mark;
    [SerializeField]private GameObject Character;
    public int health;
    public int armor;
    public int blocks = 0;
    private GameObject HealthPrefab;
    private GameObject ArmorPrefab;
    private GameObject ShieldIconPrefab;
    private GameObject ParryIconPrefab;
    public Sprite StunIcon;
    public bool IsParry = false;
    public bool IsStunned = false;
    public string[] NextAttackList;
    public Sprite[] NextAttackIcon;
    public Sprite[] AttackSprite;
    public Sprite IdleAnimation;
    public Sprite BlockAnimation;
    public Sprite StanceAnimation;
    public int[] AttackMove;
    public GameObject NextAttackIndicator;
    private string Stance = "";
    private bool Animation = false;
    private int NextAttack;
    private float HealthPosition;
    private float ArmorPosition;
    private HealthSystem HealthSystemScript;
    private BattleKnight CharacterScript;

    void GetDamage(int damage, bool IgnoreArmor, bool Stun)
    {
        if (blocks > 0)
        {
            Log.text = "\n" + name + " blocks attack;" + Log.text;
            blocks -= damage;
        }
        else if (armor > 0 && !IgnoreArmor)
        {
            Log.text = "\n" + name + "'s armor gets " + damage + " damage;" + Log.text;
            armor -= damage;
        }
        else
        {
            Log.text = "\n" + name + "'s health gets " + damage + " damage;" + Log.text;
            health -= damage;
        }
        if (Stun)
        {
            Log.text = "\n" + name + " is stunned;" + Log.text;
            Stun = true;
        }
        SpriteManager();
    }

    IEnumerator AttackedAnimation()
    {
        transform.position += new Vector3(1, 0, 0);
        yield return new WaitForSeconds(1);
        if (health >= 1)
        {
            transform.position -= new Vector3(1, 0, 0);
        }
    }

    void SpriteManager()
    {
        GetComponent<SpriteRenderer>().sprite = IdleAnimation;
        if (blocks > 0)
        {
            GetComponent<SpriteRenderer>().sprite = BlockAnimation;
        }
        if (Stance != "")
        {
            GetComponent<SpriteRenderer>().sprite = StanceAnimation;
        }
    }

    IEnumerator EnemyAction()
    {
        Animation = true;
        if (!IsStunned)
        {
            yield return new WaitForSeconds(1);
            gameObject.GetComponent<SpriteRenderer>().sprite = AttackSprite[NextAttack];
            transform.position -= new Vector3(AttackMove[NextAttack], 0, 0);
            switch (NextAttackList[NextAttack])
            {
                case "Attack":
                    Log.text = "\n" + name + " attacks you;" + Log.text;
                    CharacterScript.GetDamage(1, false, false);
                    if (CharacterScript.IsParry)
                    {
                        IsStunned = true;
                    }
                    break;
                case "Defence":
                    Log.text = "\n" + name + " gets 1 block;" + Log.text;
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
                case "Spear Stance":
                    Log.text = "\n" + name + " lowers the spear;" + Log.text;
                    Stance = "Spear Stance";
                    break;
                case "Shieldbreaker":
                    Log.text = "\n" + name + " bashes your block with his shield, removing it;" + Log.text;
                    CharacterScript.blocks = 0;
                    break;
            }
            yield return new WaitForSeconds(1);
            transform.position += new Vector3(AttackMove[NextAttack], 0, 0);
            SpriteManager();
            CharacterScript.status = "New_turn";
        }
        else
        {
            Log.text += "\n" + name + " is no longer stunned;";
            yield return new WaitForSeconds(2);
            IsStunned = false;
            CharacterScript.status = "New_turn";
        }
        Animation = false;
    }

    void Start()
    {
        CharacterScript = Character.GetComponent<BattleKnight>();
        //generate health and armor indicator above character
        HealthPrefab = Resources.Load<GameObject>("health-full");
        ArmorPrefab = Resources.Load<GameObject>("armor-full");
        ShieldIconPrefab = Resources.Load<GameObject>("block-icon");
        ParryIconPrefab = Resources.Load<GameObject>("parry-icon");
        HealthSystemScript = HealthPrefab.GetComponent<HealthSystem>();

        for (int i = 0; i < maxHealth; i++)
        {
            HealthSystemScript.Unit = gameObject;
            HealthSystemScript.index = i + 1;
            Instantiate(HealthPrefab, new Vector3(transform.position.x - 1.1f + i * 0.7f, transform.position.y + 1.6f, 0), Quaternion.identity, gameObject.transform);
            HealthPosition = i;
        }
        HealthSystemScript = ArmorPrefab.GetComponent<HealthSystem>();
        for (int j = 0; j < maxArmor; j++)
        {
            HealthSystemScript.Unit = gameObject;
            HealthSystemScript.index = j + 1;
            Instantiate(ArmorPrefab, new Vector3(transform.position.x - 1.1f + ((HealthPosition + 1) * 0.7f) + (j * 0.7f), transform.position.y + 1.6f, 0), Quaternion.identity, gameObject.transform);
            ArmorPosition = j;
        }
    }

    void Update()
    {
        if (CharacterScript.status == "New_turn")
        {
            NextAttack = Random.Range(0, NextAttackList.Length);
            NextAttackIndicator.GetComponent<SpriteRenderer>().sprite = NextAttackIcon[NextAttack];
        }
        if (IsStunned)
        {
            NextAttackIndicator.GetComponent<SpriteRenderer>().sprite = StunIcon;
        }
        if (health <= 0)
        {
            Log.text = "\n" + name + " is defeated;" + Log.text;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
            NextAttackIndicator.SetActive(false);
            gameObject.tag = "Untagged";
            Destroy(this);
        }
        if (CharacterScript.status == "Enemy_turn")
        {
            if (Animation == false)
            {
                blocks = 0;
                Stance = "";
                SpriteManager();
                StartCoroutine(EnemyAction());
            }
        }
        if (CharacterScript.status == "Choose_target"|| CharacterScript.status == "Animation")
        {
            if (Input.GetMouseButtonDown(0)&&Stance == "Spear Stance")
            {
                Log.text = "\n" + "While attacking, you bump into " + name + "'s spear;" + Log.text;
                CharacterScript.GetDamage(1, false, false);
                SpriteManager();
            }
        }
    }

    private void OnMouseOver()
    {
        if (CharacterScript.status == "Choose_target")
        {
            Mark.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y - 1f, 0);
        }
    }

    private void OnMouseDown()
    {
        if (CharacterScript.status == "Choose_target")
        {
            Mark.transform.position = new Vector3(-11, 4, 0);
            CharacterScript.status = "Animation";
            switch (CharacterScript.attack)
            {
                case "slash(Clone)":
                    StartCoroutine(AttackedAnimation());
                    GetDamage(1, false, false);
                    break;
            }
        }
    }
}