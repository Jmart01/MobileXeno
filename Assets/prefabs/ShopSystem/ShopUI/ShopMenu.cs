using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] ShopItem shopItemPrefab;
    [SerializeField] GameObject ShopPanelObject;
    [SerializeField] ShopSystem shopSystem;
    // Start is called before the first frame update
    void Start()
    {
        PopulateItems(shopSystem.GetWeaponsOnSale());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateItems(Weapon[] weaponsOnSale)
    {
        foreach(Weapon weapon in weaponsOnSale)
        {
            ShopItem newItem = Instantiate(shopItemPrefab, ShopPanelObject.transform);
            newItem.Init(weapon.GetWeaponInfo(), shopSystem);
        }
    }
}