using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private MapTooltip script;
    [SerializeField]public float passability;
    [SerializeField]public float visibility;

    void Start()
    {
        script = GameObject.Find("Tooltip").GetComponent<MapTooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        script.ShowTooltip();
        switch (name)
        {
            case "Rumors":
                script.TooltipText.text = "Rumors and notes";
                break;
            case "Enemies":
                script.TooltipText.text = "Types of enemies";
                break;
            case "Locations":
                script.TooltipText.text = "Places and locations";
                break;
            case "Quests":
                script.TooltipText.text = "Current quests and tasks";
                break;
            case "Character Button":
                script.TooltipText.text = "Combat";
                break;
            case "Notes Button":
                script.TooltipText.text = "Journal";
                break;
            case "Map Button":
                script.TooltipText.text = "Map";
                break;
            case "VillageMark":
                script.TooltipText.text = "Village";
                break;
        }
        //for menu tooltips
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        script.HideTooltip();
        //hide menu tooltip
    }
    private void OnMouseOver()
    {
        if (script.ObservationMode && Time.timeScale != 0)
        {
            switch (name)
            {
                case "Crops Collision":
                    script.TooltipText.text = "Field of wheat \nvisibility is extremely low here";
                    break;
                case "Swamp Collision":
                    script.TooltipText.text = "Fetid swamp \nalmost impossible to traverse";
                    break;
                case "Light Forest Collision":
                    script.TooltipText.text = "Sparse forest \nvisibility and speed are reduced but still decent";
                    break;
                case "Forest Collision":
                    script.TooltipText.text = "Dense forest \nvisibility and speed are significantly limited";
                    break;
                case "Road Collision":
                    script.TooltipText.text = "Road \nthe fastest way to traverse the land";
                    break;
                case "Rock Collision":
                    script.TooltipText.text = "Cliff \nimpossible to pass, find the way around";
                    break;
                case "Water Collision":                                                       
                    script.TooltipText.text = "Water \nfairly deep water, unpassable";
                    break;
                case "Chopped Trees Collision":
                    script.TooltipText.text = "Chopped trees \nremains of a forest, felled for timber";
                    break;
                case "Battlefield Collision":
                    script.TooltipText.text = "Battlefield \naftermath of a brutal combat \ncovered with bodies and military equipment";
                    break;
                case "Grassland Collision":
                    script.TooltipText.text = "Open grassy area \ngreat visibility and easy to traverse";
                    break;
                case "Village Collision":
                    script.TooltipText.text = "Village \npart of the local settlement";
                    break;
                //lendscape tooltips
            }
            switch (tag)
            {
                case "patrol":
                    Debug.Log("patrol");
                    script.TooltipText.text = "Patrol of Iron Tusk \nfew moderately trained soldiers, \npatrolling the land";
                    break;
                //enemies and locations tooltips
            }
        }
        //for map tooltips
    }
}
