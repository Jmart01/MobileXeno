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
    public void SetPlayerHealth(float percent)
    {
        ProgressBar.material.SetFloat("_Progress", percent);
    }
    public void SwichToInGameMenu()
    {
        InGameMenu.SetActive(true);
        PauseMenu.SetActive(false);
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
