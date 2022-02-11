using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Shop/ShopSystem")]
public class ShopSystem : ScriptableObject
{
    [SerializeField] CreditSystem creditSystem;
    [SerializeField] Weapon[] weaponsOnSale;

    // Start is called before the first frame update
    void Start()
    {

    }

    internal Weapon[] GetWeaponsOnSale()
    {
        return weaponsOnSale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PurchaseWeapon(string WeaponName)
    {
        if(!HasCreditSystem())
        {
            return;
        }
        foreach(Weapon weapon in weaponsOnSale)
        {
            if(weapon.GetWeaponInfo().name == WeaponName)
            {
                Player player = FindObjectOfType<Player>();
                if(player != null && CanPurchase(weapon.GetWeaponInfo().cost))
                {
                    player.AquireNewWeapon(weapon);
                    creditSystem.ChangeCredit(-weapon.GetWeaponInfo().cost);

                }
            }
        }
    }

    public bool CanPurchase(float cost)
    {
        if(!HasCreditSystem())
        {
            return false;
        }
        if(cost > creditSystem.GetCurrentCredit()) 
        {
            return false;
        }
        return true;
    }

    public bool PlayerOwnsItem(ShopItem item)
    {
        Player player = FindObjectOfType<Player>();
        foreach(Weapon weapon in player.GetOwnedWeapons())
        {
            if(weapon.GetWeaponInfo().name == item.weaponInfo.name)
            {
                Debug.Log("Player has weapon");
                return true;
            }
            else
            {
                Debug.Log("player does not have it");
                return false;
            }
        }
        return false;
    }

    bool HasCreditSystem()
    {
        if (creditSystem == null)
        {
            creditSystem = FindObjectOfType<CreditSystem>();
        }

        return creditSystem != null;
    }
}
