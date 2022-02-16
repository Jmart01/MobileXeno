using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject InGameMenu;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Canvas ShopMenu;
    [SerializeField] Image WeaponIcon;
    [SerializeField] Image ProgressBar;
    [SerializeField] Text CreditAmtText;

    public void SetPlayerHealth(float percent)
    {
        ProgressBar.material.SetFloat("_Progress", percent);
    }
    public void SwichToInGameMenu()
    {
        Time.timeScale = 1f;
        InGameMenu.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void ToggleShopMenu()
    {
        if(ShopMenu.enabled)
        {
            ShopMenu.enabled = false;

        }
        else
        {
            ShopMenu.enabled = true;
        }
    }

    public void SetPlayerCreditAmt(float newValue)
    {
        CreditAmtText.text = newValue.ToString();
    }

    public void SWitchToPauseMenu()
    {
        Time.timeScale = 0f;
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
        ShopMenu.enabled = false;
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

    public void RestartLevel()
    {
        GameplayStatic.StartNewGame();
    }
}
