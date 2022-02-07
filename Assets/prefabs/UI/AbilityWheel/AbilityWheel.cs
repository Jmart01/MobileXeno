using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityWheel : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] AbilityWidget[] abilityWidgets;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 widgetPos = GetComponent<RectTransform>().position;
        Vector2 wheelCenter = new Vector2(widgetPos.x, widgetPos.y);
        Vector2 DragDir = (eventData.position - wheelCenter).normalized;

        float closestAngle = 360.0f;
        AbilityWidget closestWidget = null;

        foreach(var widget in abilityWidgets)
        {
            Vector3 widgetDir = -widget.transform.right;
            Vector2 widgetDir2D = new Vector2(widgetDir.x, widgetDir.y);

            float angle = Vector2.Angle(DragDir, widgetDir2D);
            if(angle < closestAngle)
            {
                closestAngle = angle;
                closestWidget = widget;
            }
            widget.SetHighlighted(false);
        }
        closestWidget.SetHighlighted(true);
    }

    internal void AddNewAbility(AbilityBase newAbility)
    {
        int level = newAbility.GetLevel();
        if(level -1 < abilityWidgets.Length && level - 1 >= 0)
        {
            abilityWidgets[level -1].AssignAbility(newAbility);
        }
    }

    internal void UpdateStamina(float newValue)
    {
        //loops throught the array of widgets
        for(int i = 0; i < abilityWidgets.Length; i++)
        {
            //checks  if newvalue if greater than 1
            if (newValue > 1)
            {
                //subtracts 1 from new value and sets it as the remainder
                newValue -= 1;
                //sets the stamina progress to 1 on the material
                abilityWidgets[i].SetStaminaProgress(1);
            }else if(newValue > 0)
            {
                
                abilityWidgets[i].SetStaminaProgress(newValue);
                newValue = 0;
                break;
            }
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach(AbilityWidget widget in abilityWidgets)
        {
            widget.SetExpand(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach(AbilityWidget widget in abilityWidgets)
        {
            widget.SetExpand(false);
        }
    }
}
