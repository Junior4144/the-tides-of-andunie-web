using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSquadImpulseController : BaseSquadImpulseController
{
    protected override void Awake()
    {
        base.Awake();

        _heroRigidBody = GetComponent<Rigidbody2D>();
    }

    protected override float GetDashMultiplier(bool isDashing) => 1f;
}
