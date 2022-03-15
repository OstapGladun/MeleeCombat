using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int index;
    [SerializeField]private Sprite HealthEmpty;
    [SerializeField]private Sprite ArmorEmpty;
    [SerializeField]private Sprite[] Shield;
    public GameObject Unit;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Unit == GameObject.Find("Knight"))
        {
            if (spriteRenderer.sprite.name == "health-full" && index > Unit.GetComponent<BattleKnight>().health)
            {
                spriteRenderer.sprite = HealthEmpty;
            }
            if (spriteRenderer.sprite.name == "armor-full" && index > Unit.GetComponent<BattleKnight>().armor)
            {
                spriteRenderer.sprite = ArmorEmpty;
            }
            if (index == 101)
            {
                if (Unit.GetComponent<BattleKnight>().blocks > 0)
                {
                    spriteRenderer.sprite = Shield[Unit.GetComponent<BattleKnight>().blocks - 1];
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
            if (spriteRenderer.sprite.name == "health-full" && index > Unit.GetComponent<BattleEnemy>().health)
            {
                spriteRenderer.sprite = HealthEmpty;
            }
            if (spriteRenderer.sprite.name == "armor-full" && index > Unit.GetComponent<BattleEnemy>().armor)
            {
                spriteRenderer.sprite = ArmorEmpty;
            }
            if (index == 101)
            {
                if (Unit.GetComponent<BattleEnemy>().blocks > 0)
                {
                    spriteRenderer.sprite = Shield[Unit.GetComponent<BattleEnemy>().blocks - 1];
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

}