using UnityEngine;

public class CannonBallCutscene : CannonBall
{

    public float force;

    protected override void Shoot()
    {
        var target = GameObject.FindGameObjectWithTag("CannonTarget");
        rb.linearVelocity = (target.transform.position - transform.position).normalized * force;
    }
        
}
