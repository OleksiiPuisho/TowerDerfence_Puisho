using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField] internal float _timeToDeactivationBullet;
    [HideInInspector] public float SpeedBullet;
    [HideInInspector] public float DamageBullet;
    [SerializeField] internal GameObject _hitParticle;
    [SerializeField] internal GameObject _startParticle;
    [SerializeField] internal string _defaultLayerBullet;
    [SerializeField] internal string _enemyLayerBullet;
    public void AutoPutBullet()
    {
        Invoke(nameof(AutoPut), _timeToDeactivationBullet);
    }
    private void Update()
    {
        transform.Translate(transform.forward * (SpeedBullet * Time.deltaTime), Space.World);
    }
    internal void AutoPut()
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
            }
        }
        else
        {
            if (collider.GetComponent<Enemy>() && collider.TryGetComponent<IDamageble>(out var damageble))
            {
                damageble.SetDamage(DamageBullet);
            }
        }
        AutoPut();
        ParticleChange(_hitParticle);
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
    }
}
