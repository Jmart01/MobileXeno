using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnBlackboardKeyUpdated(string name, object value);

public class BehaviorTree : MonoBehaviour
{
    Dictionary<string, object> Blackboard = new Dictionary<string, object>();
    public OnBlackboardKeyUpdated onBlackboardKeyUpdated;

    BTNode _CurrentRunningNode;
    public BTNode GetCurrentRunningNode() { return _CurrentRunningNode; }
    public void SetCurrentRunningNode(BTNode node) 
    { 
        _CurrentRunningNode = node;
    }
    /*
     * returns true if A is lower priority of B
     */
    public bool IsLowerPriority(BTNode A, BTNode B)
    {
         List<BTNode> AHierachy = GetHierachy(A);
        List<BTNode> BHierachy = GetHierachy(B);
        for (int i = 1; i < AHierachy.Count && i < BHierachy.Count; i++)
        {
            BTNode ASide = AHierachy[i];
            BTNode BSide = BHierachy[i];
            if (ASide != BSide)
            {
                return ASide.GetParentIndex() > BSide.GetParentIndex();
            }
        }
        return false;
    }

    List<BTNode> GetHierachy(BTNode node)
    {
        List<BTNode> hierachy = new List<BTNode>();
        hierachy.Add(node);

        if (node == _Root)
        {
            return hierachy;
        }

        BTNode parent = node.Parent;
        while(parent != _Root)
        {
            hierachy.Add(parent);
            parent = parent.Parent;
        }
        hierachy.Add(_Root);
        hierachy.Reverse();
        return hierachy;
    }

    internal bool IsRunningNode(BTNode node)
    {
        return node == GetCurrentRunningNode() || isChildOf(GetCurrentRunningNode(),node);
    }

    public bool isChildOf(BTNode A, BTNode B)
    {
        if(A==B)
        {
            return false;
        }
        List<BTNode> AHierachy = GetHierachy(A);
        return AHierachy.Contains(B);
    }

    public void RestartTree()
    {
        _Root.Finish();
    }

    public void SetBlackboardKey(string key, object value)
    {
        if (Blackboard.ContainsKey(key))
        {
            Blackboard[key] = value;
            if (onBlackboardKeyUpdated!=null)
            {
                onBlackboardKeyUpdated.Invoke(key, value);
            }
        }
    }

    public void AddBlackBoardKey(string key, object defaultValue = null)
    {
        if(!Blackboard.ContainsKey(key))
        {
            Blackboard.Add(key, defaultValue);
        }
    }

    public virtual void Init(AIController aiController) { AIC = aiController; }
    public void GetBlackboardValue(string key, out object value)
    {
        value = Blackboard[key];
    }

    public void Reset()
    {
        _Root.Finish();
    }
    public EBTTaskResult Run()
    {
        EBTTaskResult result = EBTTaskResult.Faliure;
        if(!_Root.HasStarted())
        {
            result = _Root.Start();
            if(result!=EBTTaskResult.Running)
            {
                _Root.Finish();
            }
            return result;
        }

        result = _Root.Update();
        if (result != EBTTaskResult.Running)
        {
            _Root.Finish();
        }
        return result;
    }

    BTNode _Root;
    public void SetRootNode(BTNode root)
    {
        _Root = root;
    }
    
    AIController _AIC;
    public AIController AIC { 
        set { _AIC = value; } 
        get { return _AIC; }  
    }
}
