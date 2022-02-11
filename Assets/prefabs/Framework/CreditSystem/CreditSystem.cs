using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnCreditAmountChanged(float newValue, float oldValue);
public class CreditSystem : MonoBehaviour
{
    [SerializeField] float currentCredit = 10;

    public OnCreditAmountChanged onCreditChanged;

    public float GetCurrentCredit()
    {
        return currentCredit;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
     public void ChangeCredit(float changeAmount)
    {
        float oldValue = currentCredit;
        currentCredit += changeAmount;
        currentCredit = Mathf.Clamp(currentCredit, 0f, float.MaxValue);
        if(onCreditChanged != null)
        {
            onCreditChanged.Invoke(currentCredit, oldValue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BroadCastCreditAmount()
    {
        onCreditChanged.Invoke(currentCredit, currentCredit);
    }
}
