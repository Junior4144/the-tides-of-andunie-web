using UnityEngine;

public interface IShieldable : IHealthController
{
    void ShieldActivationEventHandler();
    bool ShieldBlocks();
    void ShieldDeactivationEventHandler();
}
