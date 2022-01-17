using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionSystem : MonoBehaviour
{
    List<PerceptionStimuli> Stimulis = new List<PerceptionStimuli>();
    List<PerceptionComp> Listeners = new List<PerceptionComp>();
    [SerializeField] float AIUpdateInterval = 0.1f;
    public bool EnableDebug = false;

    private void Start()
    {
        StartCoroutine(StartUpdateAIPerception(AIUpdateInterval));
    }


    IEnumerator StartUpdateAIPerception(float Interval)
    {
        while(true)
        {
            yield return new WaitForSeconds(Interval);
            UpdateAIPerception();
        }
    }
    private void Update()
    {
        if (EnableDebug)
        {
            for (int i = 0; i < Listeners.Count; i++)
            {

                PerceptionComp perception = Listeners[i];
                if (Listeners[i] != null)
                {
                    perception.SetDrawDebug(true);
                }
            }
        }
    }
    private void UpdateAIPerception()
    {
        for(int i = 0; i < Listeners.Count; i++)
        {
            PerceptionComp perceptionComp = Listeners[i];
            if(perceptionComp!=null)
            {
                for(int j = 0; j < Stimulis.Count; j++)
                {
                    perceptionComp.EvaluateStimuli(Stimulis[j]);
                }
            }
        }
    }

    public void RegisterStimuli(PerceptionStimuli stimuli)
    {
        if (!Stimulis.Contains(stimuli))
        {
            Stimulis.Add(stimuli);
        }
    }

    public void AddListener(PerceptionComp listener)
    {
        if (!Listeners.Contains(listener))
        {
            Listeners.Add(listener);
        }
    }

    public void RemoveListener(PerceptionComp perceptionComp)
    {
        Listeners.Remove(perceptionComp);
    }

    public void RemoveStimuli(PerceptionStimuli stimuli)
    {
        Stimulis.Remove(stimuli);
        BroadCastStiumliRemoved(stimuli);
    }

    public void BroadCastStiumliRemoved(PerceptionStimuli stimuli)
    {
        for (int i = 0; i < Listeners.Count; i++)
        {
            PerceptionComp perceptionComp = Listeners[i];
            if (perceptionComp != null)
            {
                perceptionComp.StimuliUnRegistered(stimuli);
            }
        }
    }
}
