using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LandscapeDetection : MonoBehaviour
{
    [SerializeField] private float overallSpeed;
    [SerializeField] private float overallVisibility;
    private ChangeTooltip scriptLandscape;
    private MovementEnemy scriptEnemy;
    public string landscapeName;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "landscapecollision")
        {
            landscapeName = collision.name.Replace(" Collision","");
            scriptLandscape = collision.gameObject.GetComponent<ChangeTooltip>();
            scriptEnemy = GetComponentInParent<MovementEnemy>();
            GetComponentInParent<AIPath>().maxSpeed = scriptLandscape.passability * overallSpeed;
            if (scriptEnemy != null)
            {
                scriptEnemy.aggroDistance = scriptLandscape.visibility*overallVisibility;
            }
        }
    }
}