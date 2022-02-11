using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopItem : MonoBehaviour
{
    ShopSystem _shopSystem;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] Image Icon;

    public WeaponInfo weaponInfo
    {
        get;
        private set;
    }
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CreditSystem>().onCreditChanged += OnCreditChanged;
    }

    private void OnCreditChanged(float newValue, float oldValue)
    {
        if(_shopSystem.CanPurchase(weaponInfo.cost))
        {
            gameObject.GetComponent<Button>().enabled = true;
        }
        else
        {
            
            gameObject.GetComponent<Button>().enabled = false; 
        }
        if(_shopSystem.PlayerOwnsItem(this))
        {
            gameObject.GetComponent<Button>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void Init(WeaponInfo weaponInfo, ShopSystem shopSystem)
    {
        _shopSystem = shopSystem;
        this.weaponInfo = weaponInfo;
        Icon.sprite = weaponInfo.Icon;
        Text.text = $"{weaponInfo.name}\n" +
            $"Rate: {weaponInfo.ShootSpeed}\n" +
            $"Damage: {weaponInfo.DmgPerBullet}\n" +
            $"Cost: {weaponInfo.cost}";
    }

    public void Purchase()
    {
        _shopSystem.PurchaseWeapon(weaponInfo.name);
    }
}
