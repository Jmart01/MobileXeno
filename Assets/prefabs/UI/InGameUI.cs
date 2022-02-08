using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject InGameMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Image WeaponIcon;
    [SerializeField] Image ProgressBar;
    [SerializeField] Text CreditAmtText;
    public void SetPlayerHealth(float percent)
    {
        ProgressBar.material.SetFloat("_Progress", percent);
    }
    public void SwichToInGameMenu()
    {
        InGameMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void SetPlayerCreditAmt(float newValue)
    {
        CreditAmtText.text = newValue.ToString();
    }

    public void SWitchToPauseMenu()
    {
        PauseMenu.SetActive(true);
        InGameMenu.SetActive(false);
       
    }

    private void Start()
    {
        SwichToInGameMenu();
        HealthComponent PlayerHealthComp = FindObjectOfType<Player>().GetComponent<HealthComponent>();
        PlayerHealthComp.onHealthChanged += PlayerHealthChanged;
        PlayerHealthComp.BroadCastCurrentHealth();

        CreditSystem playerCreditSystem = FindObjectOfType<Player>().GetComponent<CreditSystem>();
        playerCreditSystem.onCreditChanged += PlayerCreditChanged;
        playerCreditSystem.BroadCastCreditAmount();
    }

    private void PlayerCreditChanged(float newValue, float oldValue)
    {
        SetPlayerCreditAmt(newValue);
    }

    private void PlayerHealthChanged(float newValue, float oldValue, float maxValue, GameObject Causer)
    {
        SetPlayerHealth((float)newValue/(float)maxValue);
    }

    private void Update()
    {
        
    }

    public void SwichedWeaponTo(Weapon EquipedWeapon)
    {
        WeaponIcon.sprite = EquipedWeapon.GetWeaponIcon();
    }
}
