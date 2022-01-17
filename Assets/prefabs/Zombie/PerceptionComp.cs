using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPerceptionUpdated(bool SuccessfullySensed, GameObject objectSensed);

public class PerceptionComp : MonoBehaviour
{
    public OnPerceptionUpdated onPerceptionUpdated;
    
    [SerializeField] float PeripheralVisionAngleDegrees = 120f;
    [SerializeField] float AlwaysAwareDistance = 20.0f;
    [SerializeField] float SightRaidus = 100f;
    [SerializeField] float LoseSightRaidus = 120f;
    [SerializeField] float EyeHeight = 2f;
    [SerializeField] LayerMask PerceptionLayerMask;
    bool DrawDebug = false;

    List<PerceptionStimuli> CurrentlySensingStimuli = new List<PerceptionStimuli>();
    public bool IsCurrentlySensing(GameObject gameObject)
    {
        foreach(var stimuli in CurrentlySensingStimuli)
        {
            if(stimuli != null && stimuli.gameObject == gameObject)
            {
                return true;
            }
        }
        return false;
    }
    private void Start()
    {
        FindObjectOfType<PerceptionSystem>().AddListener(this);
    }
    public void EvaluateStimuli(PerceptionStimuli perceptionStimuli)
    {
        if(IsPerceived(perceptionStimuli))
        {
            if (!CurrentlySensingStimuli.Contains(perceptionStimuli))
            {
                CurrentlySensingStimuli.Add(perceptionStimuli);
                if (onPerceptionUpdated!=null)
                {
                    onPerceptionUpdated.Invoke(true, perceptionStimuli.gameObject);
                }
            }
        }
        else
        {
            if (CurrentlySensingStimuli.Contains(perceptionStimuli))
            {
                CurrentlySensingStimuli.Remove(perceptionStimuli);
                if (onPerceptionUpdated != null)
                {
                    onPerceptionUpdated.Invoke(false, perceptionStimuli.gameObject);
                }
            }
        }
    }

    bool IsPerceived(PerceptionStimuli stimuli)
    {
        return CanSeeStimuli(stimuli) || IsInAwareDistance(stimuli);
    }

    private bool CanSeeStimuli(PerceptionStimuli stimuli)
    {
        Vector3 position = stimuli.transform.position;
        Vector3 ToTargetVector = position - transform.position;

        float checkRadius = SightRaidus;
        if (CurrentlySensingStimuli.Contains(stimuli))
        {
            checkRadius = LoseSightRaidus;
        }

        if (ToTargetVector.magnitude > checkRadius)
        {
            return false;
        }

        if (!IsStimuliInPeripheralAngle(stimuli))
        {
            return false;
        }

        if (IsStimuliBlocked(stimuli))
        {
            return false;
        }

        return true;
    }

    internal void StimuliUnRegistered(PerceptionStimuli stimuli)
    {
        onPerceptionUpdated.Invoke(false, stimuli.gameObject);
    }

    private bool IsStimuliInPeripheralAngle(PerceptionStimuli stimuli)
    {
        Vector3 position = stimuli.transform.position;
        Vector3 ToTargetVector = position - transform.position;
        Vector3 DirToTarget = ToTargetVector.normalized;
        Vector3 Dir = transform.forward;
        float angleDegrees = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(DirToTarget, Dir));
        return angleDegrees < PeripheralVisionAngleDegrees / 2;
    }

    bool IsStimuliBlocked(PerceptionStimuli Stimuli)
    {
        Ray testRay = new Ray(transform.position + Vector3.up * EyeHeight, (Stimuli.gameObject.GetComponent<Collider>().bounds.center - (transform.position + Vector3.up *EyeHeight)).normalized);
        if(Physics.Raycast(testRay, out RaycastHit hitInfo, LoseSightRaidus, PerceptionLayerMask))
        {
            //Debug.Log($"Found: {hitInfo.collider.gameObject}");
            return hitInfo.collider.gameObject != Stimuli.gameObject;
        }
        return true;
    }

    bool IsInAwareDistance(PerceptionStimuli stimuli)
    {
        return Vector3.Distance(stimuli.transform.position, transform.position) < AlwaysAwareDistance; 
    }

    private void OnDestroy()
    {
        PerceptionSystem perceptionSystemp =  FindObjectOfType<PerceptionSystem>();
        if (perceptionSystemp)
        {
            perceptionSystemp.RemoveListener(this);
        }
    }

    private void OnDrawGizmos()
    {
        if(DrawDebug)
        {
            Vector3 LeftDir = Quaternion.AngleAxis(PeripheralVisionAngleDegrees / 2, Vector3.up) * transform.forward;
            Vector3 RightDir = Quaternion.AngleAxis(-PeripheralVisionAngleDegrees / 2, Vector3.up) * transform.forward;
            Debug.DrawLine(transform.position + Vector3.up*EyeHeight, transform.position + LeftDir * LoseSightRaidus + Vector3.up * EyeHeight, Color.green);
            Debug.DrawLine(transform.position + Vector3.up * EyeHeight, transform.position + RightDir * LoseSightRaidus + Vector3.up * EyeHeight, Color.green);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * EyeHeight, LoseSightRaidus);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * EyeHeight, SightRaidus);
            Gizmos.DrawWireSphere(transform.position + Vector3.up * EyeHeight, AlwaysAwareDistance);
            for(int i = 0; i < CurrentlySensingStimuli.Count; i++)
            {
                if(CurrentlySensingStimuli[i]!=null)
                {
                    Debug.DrawLine(transform.position + Vector3.up * EyeHeight, CurrentlySensingStimuli[i].transform.position);
                }
            }
        }
    }

    public void SetDrawDebug(bool draw)
    {
        DrawDebug = draw;
    }
}
