public interface IHealthController
{
    void TakeDamage(float amount);
    void AddHealth(float amount);
    float GetCurrentHealth();
    float GetPercentHealth();

}