using UnityEngine;

public class MaxHealthEffect : ItemEffect
{
    [SerializeField] private float healthIncrease;
    [SerializeField] private bool isPercentage;
    
    private float _originalValue;
    private float _appliedValue;
    
    public override void Apply()
    {
        _originalValue = PlayerStatsManager.Instance.MeleeDamage;
        
        if (isPercentage)
            _appliedValue = _originalValue * (1 + healthIncrease / 100f);
        else
            _appliedValue = _originalValue + healthIncrease;
        
        PlayerStatsManager.Instance.SetMeleeDamage(_appliedValue);
    }
    
    public override void Remove()
    {
        PlayerStatsManager.Instance.SetMeleeDamage(_originalValue);
    }
    
    public override string GetDescription() =>
        $"{(healthIncrease >= 0 ? "+" : "-")}{healthIncrease}{(isPercentage ? "%" : "")} Melee Damage";
}