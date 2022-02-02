using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Player player;
    [SerializeField] float blastRadius = 3f;
    [SerializeField] int directDmg;
    [SerializeField] int indirectDmg;
    [SerializeField] GameObject levelAssets;
    RangedZombie zombie;
    private void Start()
    {
        player = FindObjectOfType<Player>();
        zombie = FindObjectOfType<RangedZombie>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            player.GetComponent<HealthComponent>().ChangeHealth(-directDmg, gameObject);
            Debug.Log($"Should get destroyed, did {directDmg}");
            Debug.Log(other.gameObject.name);
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("LevelAssets"))
        {
            Debug.Log(other.gameObject.name);
            Explode();
        }
    }

    private void Explode()
    {
        float DistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(DistanceToPlayer < blastRadius)
        {
            player.GetComponent<HealthComponent>().ChangeHealth(-indirectDmg, gameObject);
            //there is a null exception error, this is because when the player dies it is still trying to find the player and shoot them.
            Debug.Log($"Should get destroyed, did {indirectDmg}");
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
