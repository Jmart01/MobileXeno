using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WeaponInfo
{
    public string name;
    public Sprite Icon;
    public float DmgPerBullet;
    public float ShootSpeed;
    public float cost;
}

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform FirePoint;
    [SerializeField] ParticleSystem BulletEmitter;
    [SerializeField] float DamagePerBullet = 1;
    [SerializeField] Sprite WeaponIcon;
    [SerializeField] float ShootingSpeed = 1.0f;
    [SerializeField] string WeaponName;
    [SerializeField] float cost;

    public WeaponInfo GetWeaponInfo()
    {
        return new WeaponInfo()
        {
            Icon = WeaponIcon,
            name = WeaponName,
            DmgPerBullet = this.DamagePerBullet,
            ShootSpeed = this.ShootingSpeed,
            cost = this.cost
        };
    }

    public Sprite GetWeaponIcon() { return WeaponIcon; }
    public float GetDamagePerBullet() { return DamagePerBullet; }
    public GameObject Owner { set; get; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Equip()
    {
        gameObject.SetActive(true);        
    }

    public float GetShootingSpeed()
    {
        return ShootingSpeed;
    }

    public void UnEquip()
    {
        gameObject.SetActive(false);
    }

    public void Fire()
    {
        if(BulletEmitter)
        {
            GetComponent<AudioSource>().Play();

            BulletEmitter.Emit(BulletEmitter.emission.GetBurst(0).maxCount);
        }
    }
}
