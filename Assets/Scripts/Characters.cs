using UnityEngine;

public class Characters : MonoBehaviour
{
    [SerializeField]
    private float _healthPoint = 100;

    public void DealingDamage(float damage)
    {
        if(damage >= 0)
        {
            _healthPoint -= damage;
        }

        if (_healthPoint <= 0)
        {
            Death();
        }
    }
        
    public void DealingDamage(float damage, Vector3 position, float radius)
    {
        var distance = (transform.position - position).magnitude;

        if ((damage >= 0) && (radius > 0) && (distance <= radius))
        {
            _healthPoint -= damage * ((radius - distance) / radius);
        }

        if (_healthPoint<=0)
        {
            Death();
        }
    }

    public float GetHP()
    {
        return _healthPoint;
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
