public interface IHealthController
{
    void TakeDamage(float amount);
    void AddHealth(float amount);
    void SetCurrentHealth(float currentHealth);
    float GetCurrentHealth();
    float GetPercentHealth();

}