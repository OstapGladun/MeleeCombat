using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTooltip : MonoBehaviour
{
    public Text TooltipText;
    private RectTransform BackgroungImageTransform;
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private Image BackgroungImage;
    public bool ObservationMode = false;
    [SerializeField] private Camera Camera;
    Ray ray;
    RaycastHit hit;

    void Start()
    {
        BackgroungImageTransform = transform.Find("Tooltip Background").GetComponent<RectTransform>();
        TooltipText = transform.Find("Tooltip Text").GetComponent<Text>();
    }

    void Update()
    {
        if(Time.timeScale != 0)
        {
            if (Input.GetMouseButtonUp(1))
                ObservationMode = !ObservationMode;
            if (ObservationMode)
                ShowTooltip();
            else
                HideTooltip();
            //if not paused, player needs to press RMB to show/hide tooltip
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            HideTooltip();
        }
        //hide tooltip when pausing the game
        SetPosition();
        SetSize();
    }

    public void ShowTooltip()
    {
        TooltipText.GetComponent<Text>().enabled = true;
        BackgroungImage.GetComponent<Image>().enabled = true;
    }

    public void HideTooltip()
    {
        TooltipText.GetComponent<Text>().enabled = false;
        BackgroungImage.GetComponent<Image>().enabled = false;
    }

    private void SetSize()
    {
        float TextPaddingSize = 14f;
        Vector2 BackgroundSize = new Vector2(TooltipText.preferredWidth + TextPaddingSize, TooltipText.preferredHeight + TextPaddingSize);
        BackgroungImageTransform.sizeDelta = BackgroundSize;
    }
    private void SetPosition()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out localPoint);
        transform.position = parentCanvas.transform.TransformPoint(localPoint);
    }
}