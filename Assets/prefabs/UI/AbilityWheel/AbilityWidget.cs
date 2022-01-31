using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWidget : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform icon;

    [SerializeField] float ExpandedScale = 2.0f;
    [SerializeField] float HighLighetedScale = 2.2f;
    [SerializeField] float ScaleSpeed = 20f;

    Vector3 GoalScale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        background.localScale = Vector3.Lerp(background.localScale,GoalScale,ScaleSpeed * Time.deltaTime);
    }

    internal void SetExpand(bool isExpanded)
    {
        
        if(isExpanded)
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }else
        {
            GoalScale = new Vector3(1, 1, 1);
        }
    }

    internal void SetHighlighted(bool isHighLighted)
    {
        if(isHighLighted)
        {
            GoalScale = new Vector3(1, 1, 1) * HighLighetedScale;
        }
        else
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
    }
}
