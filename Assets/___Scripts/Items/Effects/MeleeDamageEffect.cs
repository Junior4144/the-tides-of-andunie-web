using UnityEngine;

public class MeleeDamageEffect : ItemEffect
{
    [SerializeField] private float damageIncrease;
    [SerializeField] private bool isPercentage;
    
    private float _originalValue;
    private float _appliedValue;
    
    public override void Apply()
    {
        _originalValue = PlayerStatsManager.Instance.MeleeDamage;
        
        if (isPercentage)
            _appliedValue = _originalValue * (1 + damageIncrease / 100f);
        else
            _appliedValue = _originalValue + damageIncrease;
        
        PlayerStatsManager.Instance.SetMeleeDamage(_appliedValue);
    }
    
    public override string GetDescription() =>
        $"{(damageIncrease >= 0 ? "+" : "-")}{damageIncrease}{(isPercentage ? "%" : "")} Melee Damage";
}