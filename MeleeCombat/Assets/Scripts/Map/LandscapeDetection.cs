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

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "landscapecollision")
        {
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