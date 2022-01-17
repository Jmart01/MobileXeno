using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform FirePoint;
    [SerializeField] ParticleSystem BulletEmitter;
    [SerializeField] float DamagePerBullet = 1;
    [SerializeField] Sprite WeaponIcon;

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

    public void UnEquip()
    {
        gameObject.SetActive(false);
    }

    public void Fire()
    {
        if(BulletEmitter)
        {
            BulletEmitter.Emit(BulletEmitter.emission.GetBurst(0).maxCount);
        }
    }
}
