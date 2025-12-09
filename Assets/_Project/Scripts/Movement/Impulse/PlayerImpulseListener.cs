using UnityEngine;

public class PlayerImpulseListener : BaseImpulseListener
{
    protected override bool ImpulseIsAllowed()
    {
        if (PlayerManager.Instance.IsInvincible) return false;

        return base.ImpulseIsAllowed();
    }
}
