using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] AIController AIC;
    [SerializeField] Rigidbody projectile;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float ballHeight = 3f;

    private void Start()
    {
        AIC = GetComponentInParent<AIController>();
    }
    public Vector3 ProjectileVelocity()
    {
        return CalculateProjectileVelocity();
    }
    public Rigidbody GetProjectileToLaunch()
    {
        return projectile;
    }

    public Vector3 CalculateProjectileVelocity()
    {
        GameObject Target = AIC.SetTarget();
        if(Target != null)
        {
            Vector3 TargetPos = Target.transform.position;
            Vector3 PLPos = transform.position;
            float displacementY = TargetPos.y - PLPos.y;
            Vector3 displacementXZ = new Vector3(TargetPos.x - PLPos.x, 0, TargetPos.z - PLPos.z);

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * ballHeight);
            Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * ballHeight / gravity) + Mathf.Sqrt(2 * (displacementY - ballHeight) / gravity));

            return velocityXZ + velocityY;
        }
        return Vector3.zero;
    }
}
