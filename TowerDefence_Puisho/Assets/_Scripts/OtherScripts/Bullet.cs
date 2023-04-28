using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float _timeToDeactivationBullet;
    [HideInInspector] public float SpeedBullet;
    [HideInInspector] public float DamageBullet;
    [SerializeField] protected GameObject _hitParticle;
    [SerializeField] protected GameObject _startParticle;
    [SerializeField] protected string _defaultLayerBullet;
    [SerializeField] protected string _enemyLayerBullet;
    private void Update()
    {
        transform.Translate(transform.forward * (SpeedBullet * Time.deltaTime), Space.World);
    }
    protected void AutoPut()
    {
        SpawnController.PutObject(gameObject);
        gameObject.layer = LayerMask.NameToLayer(_defaultLayerBullet);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(gameObject.layer == LayerMask.NameToLayer(_enemyLayerBullet))
        {
            if (collider.GetComponent<MainBase>() && collider.TryGetComponent<IDamageble>(out var damageble))
            {
                damageble.SetDamage(DamageBullet);
                ParticleChange(_hitParticle);
                AutoPut();
            }
        }
        else
        {
            if (collider.GetComponent<Enemy>() && collider.TryGetComponent<IDamageble>(out var damageble))
            {
                damageble.SetDamage(DamageBullet);
                ParticleChange(_hitParticle);
                AutoPut();
            }
        }
        
    }
    private void ParticleChange(GameObject particleObject)
    {
        var particle = SpawnController.GetObject(particleObject);
        particle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        particle.SetActive(true);
    }
    private void OnEnable()
    {
        ParticleChange(_startParticle);
        Invoke(nameof(AutoPut), _timeToDeactivationBullet);
    }
}
