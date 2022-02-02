using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    bool isAbilityActive = false;
    Player player;
    HealthComponent healthComp;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        healthComp = player.GetComponent<HealthComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateAbility()
    {
        if(isAbilityActive == false)
        {
            Debug.Log("activate ability");
            StartCoroutine(StartAbility(5));
        }    
    }

    IEnumerator StartAbility(float coolDownTime)
    {
        Debug.Log("Start counting down");
        float timer = 0f;
        while (timer < coolDownTime && healthComp.GetHitpoints() < healthComp.GetMaxHitpoints())
        {
            timer += Time.deltaTime;
            
            Debug.Log($"Ability is active and will remain active for {coolDownTime - timer}");
            healthComp.TakeDamage(-1,gameObject);
            Debug.Log(healthComp.GetHitpoints());
        }
        isAbilityActive = true;
        yield return new WaitForEndOfFrame();
    }
}
