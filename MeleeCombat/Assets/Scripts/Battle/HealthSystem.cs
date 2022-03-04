using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int index;
    public Sprite HealthEmpty;
    public Sprite ArmorEmpty;
    public Sprite[] Shield;
    public GameObject Unit;

    void Start()
    {

    }

    void Update()
    {
        if (Unit == GameObject.Find("Knight"))
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "health-full" && index > Unit.GetComponent<BattleKnight>().health)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = HealthEmpty;
            }
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "armor-full" && index > Unit.GetComponent<BattleKnight>().armor)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = ArmorEmpty;
            }
            if (index == 101)
            {
                if (Unit.GetComponent<BattleKnight>().blocks > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = Shield[Unit.GetComponent<BattleKnight>().blocks - 1];
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            if (index == 201)
            {
                if (Unit.GetComponent<BattleKnight>().IsParry == false)
                {
                    Destroy(gameObject);
                }
            }
            if (Unit.GetComponent<BattleKnight>().health == 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "health-full" && index > Unit.GetComponent<BattleEnemy>().health)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = HealthEmpty;
            }
            if (gameObject.GetComponent<SpriteRenderer>().sprite.name == "armor-full" && index > Unit.GetComponent<BattleEnemy>().armor)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = ArmorEmpty;
            }
            if (index == 101)
            {
                if (Unit.GetComponent<BattleEnemy>().blocks > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = Shield[Unit.GetComponent<BattleEnemy>().blocks - 1];
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            if (index == 201)
            {
                if (Unit.GetComponent<BattleEnemy>().IsParry == false)
                {
                    Destroy(gameObject);
                }
            }
            if (Unit.GetComponent<BattleEnemy>().health == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        if(index == 101)
        {
            if (Unit == GameObject.Find("Knight"))
            {
                Unit.GetComponent<BattleKnight>().ShieldIcon = gameObject;
            }
            else
            {
                Unit.GetComponent<BattleEnemy>().ShieldIcon = gameObject;
            }
        }
    }
}