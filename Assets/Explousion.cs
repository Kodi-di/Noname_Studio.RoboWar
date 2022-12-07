using UnityEngine;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class Explousion : MonoBehaviour
{
    [SerializeField]
    private float _radius = 5;
    [SerializeField]
    private float _force = 100;
    [SerializeField]
    private float _damage = 5000;
    [SerializeField]
    private GameObject _explosionEffect;
    [SerializeField]
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ExplodeAfterWait(float timeToWait)
    {
        Invoke("Explode", timeToWait);
    }

    public void Explode()
    {
        var colliders = Physics.OverlapSphere(transform.position, _radius);

        var rigidbodys = from collider in colliders
                         where collider.attachedRigidbody != null
                         select collider.attachedRigidbody;

        foreach (var rigidbody in rigidbodys)
        {
            rigidbody.AddExplosionForce(_force, transform.position, _radius);
        }
            
        var characters = from collider in colliders
                              where collider.GetComponent<Characters>() != null
                              select collider.GetComponent<Characters>();

        foreach (var character in characters)
        {
            character.DealingDamage(_damage, transform.position, _radius);
        }

        _audioSource.Play();
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
