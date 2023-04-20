using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : Bullet
{
    [SerializeField] private float _radiusExplosion;
    private GameObject _trile;
    private void Update()
    {
        transform.Translate(transform.forward * (SpeedBullet * Time.deltaTime), Space.World);
        _trile.transform.Translate(transform.forward * (SpeedBullet * Time.deltaTime), Space.World);
    }
    private void Explosion()
    {
        var colliders = Physics.SphereCastAll(transform.position, _radiusExplosion, transform.forward);
        foreach (var collider in colliders)
        {
            if (collider.collider.TryGetComponent<Enemy>(out var enemy))
            {
                if (enemy.EnemyScriptable.TypeEnemy == TypeEnemy.Air && collider.collider.TryGetComponent<IDamageble>(out var damageble))
                {
                    float distanceNormalize = Mathf.Abs((Vector3.Distance(transform.position, enemy.transform.position) / _radiusExplosion - 1f) * 100);
                    float distanceDamage = DamageBullet * distanceNormalize / 100;
                    damageble.SetDamage(distanceDamage);
                }
            }
            else if (gameObject.layer == LayerMask.NameToLayer(_enemyLayerBullet) && collider.collider.TryGetComponent(out MainBase mainBase))
            {
                float distanceNormalize = Mathf.Abs((Vector3.Distance(transform.position, mainBase.transform.position) / _radiusExplosion - 1f) * 100);
                float distanceDamage = DamageBullet * distanceNormalize / 100;
                mainBase.SetDamage(distanceDamage);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Explosion();
        AutoPut();
        var hitParticle = SpawnController.GetObject(_hitParticle);
        hitParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        hitParticle.SetActive(true);
        hitParticle.GetComponent<AutoPutObject>().AutoPut();
    }
    private void OnEnable()
    {
        _trile = SpawnController.GetObject(_startParticle);
        _trile.transform.SetPositionAndRotation(transform.position, transform.rotation);
        _trile.SetActive(true);
        _trile.GetComponent<AutoPutObject>().AutoPut();
    }
}
