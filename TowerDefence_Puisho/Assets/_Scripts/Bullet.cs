using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private TypeBullet _typeBullet;
    private Rigidbody _rigidbodyBullet;
    public float SpeedBullet;
    public float DamageBullet;
    [SerializeField] private float _radiusExplosion;
    void Awake()
    {
        _rigidbodyBullet = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    void Start()
    {
        _rigidbodyBullet.AddForce(transform.forward * SpeedBullet, ForceMode.Impulse);
    }
    private void Explosion()
    {
        var colliders = Physics.SphereCastAll(transform.position, _radiusExplosion, transform.position);
        foreach(var collider in colliders)
        {
            if(collider.collider.TryGetComponent<MainBase>(out var mainBase))
            {                
                MainBase.CurrentHealthBase -= DamageBullet;
            }
            else if(collider.collider.TryGetComponent<Enemy>(out var enemy))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                float normalize = distance / DamageBullet;
                enemy.CurrentHealthEnemy -= normalize;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_typeBullet == TypeBullet.Bullet)
        {
            if (collision.gameObject.GetComponent<MainBase>())
            {
                MainBase.CurrentHealthBase -= DamageBullet;
            }
            if (collision.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.CurrentHealthEnemy -= DamageBullet;
            }
        }
        else if (_typeBullet == TypeBullet.Rocket)
        {
            Explosion();
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        
    }
}
public enum TypeBullet { Bullet, Rocket}
