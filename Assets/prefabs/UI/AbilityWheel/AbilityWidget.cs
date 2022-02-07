using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform icon;

    [SerializeField] RectTransform Cooldown;

    [SerializeField] float ExpandedScale = 2.0f;
    [SerializeField] float HighLighetedScale = 2.2f;
    [SerializeField] float ScaleSpeed = 20f;

    Vector3 GoalScale = new Vector3(1, 1, 1);

    AbilityBase ability;

    Material CooldownMat;
    // Start is called before the first frame update
    void Start()
    {
        CooldownMat = Instantiate(Cooldown.GetComponent<Image>().material);
        Cooldown.GetComponent<Image>().material = CooldownMat;
    }

    // Update is called once per frame
    void Update()
    {
        background.localScale = Vector3.Lerp(background.localScale,GoalScale,ScaleSpeed * Time.deltaTime);
        if(ability != null)
        {
            //Debug.Log(ability.CooldownPercent);
            SetCooldownProgress(ability.CooldownPercent);
        }
    }

    void SetCooldownProgress(float progress)
    {
        CooldownMat.SetFloat("_Progress", progress);
    }

    internal void SetExpand(bool isExpanded)
    {
        
        if(isExpanded)
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }else
        {
            if(IsHighlighted() && ability != null)
            {
                ability.ActivateAbility();
            }
            GoalScale = new Vector3(1, 1, 1);
        }
    }

    private bool IsHighlighted()
    {
        return GoalScale == new Vector3(1, 1, 1) * HighLighetedScale;
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

    internal void AssignAbility(AbilityBase newAbility)
    {
        ability = newAbility;
        icon.GetComponent<Image>().sprite = ability.GetIcon();
    }
}
