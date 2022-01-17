using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionStimuli : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<PerceptionSystem>().RegisterStimuli(this);
    }
    private void OnDestroy()
    {
        PerceptionSystem perceptionSystemp = FindObjectOfType<PerceptionSystem>();
        if(perceptionSystemp)
        {
            perceptionSystemp.RemoveStimuli(this);
        }
    }
}
