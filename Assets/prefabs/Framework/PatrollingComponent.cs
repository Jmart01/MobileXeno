using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingComponent : MonoBehaviour
{
    [SerializeField] GameObject[] PatrollingPoint;
    int NextPatrolPointIndex = 0;

    public GameObject GetNextPatrolPoint()
    {
        if(PatrollingPoint.Length > NextPatrolPointIndex)
        {
            GameObject nextPoint = PatrollingPoint[NextPatrolPointIndex];
            NextPatrolPointIndex = (NextPatrolPointIndex + 1) % PatrollingPoint.Length;
            return nextPoint;
        }
        return null;
    }
}
