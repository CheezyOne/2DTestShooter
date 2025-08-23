public class EnemyHealth : Health
{
    public override void Die()
    {
        Destroy(gameObject);
    }
}