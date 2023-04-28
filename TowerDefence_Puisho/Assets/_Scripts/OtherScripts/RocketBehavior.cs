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
        var colliders = Physics.SphereCastAll(transform.position, _radiusExplosion, transform.up);
        foreach (var collider in colliders)
        {
            if (gameObject.layer == LayerMask.NameToLayer(_enemyLayerBullet))
            {
                if (collider.collider.GetComponent<MainBase>() && collider.collider.TryGetComponent<IDamageble>(out var damageble))
                {
                    damageble.SetDamage(DamageBullet);
                }
            }
            else
            {
                if (collider.collider.TryGetComponent<Enemy>(out var enemy) && collider.collider.TryGetComponent<IDamageble>(out var damageble))
                {
                    float distanceInversivNormalize = Mathf.Abs((Vector3.Distance(transform.position, enemy.transform.position) / _radiusExplosion - 1f) * 100);
                    float distanceDamage = DamageBullet * distanceInversivNormalize / 100;
                    damageble.SetDamage(distanceDamage);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        Explosion();
        AutoPut();
        var hitParticle = SpawnController.GetObject(_hitParticle);
        hitParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        hitParticle.SetActive(true);
        _trile = null;
    }
    private void OnEnable()
    {
        if (_trile == null)
        {
            _trile = SpawnController.GetObject(_startParticle);
            _trile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _trile.SetActive(true);
        }
        Invoke(nameof(AutoPut), _timeToDeactivationBullet);
    }
}
