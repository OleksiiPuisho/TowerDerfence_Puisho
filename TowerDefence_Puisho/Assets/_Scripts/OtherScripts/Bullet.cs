using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class Bullet : MonoBehaviour
{
    public TypeBullet TypeBullet;
    [SerializeField] private float _timeToDeactivationBullet;
    [HideInInspector] public float SpeedBullet;
    [HideInInspector] public float DamageBullet;
    [SerializeField] private float _radiusExplosion;
    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private GameObject _startParticle;
    private GameObject _trile;
    public void AutoPutBullet()
    {
        Invoke(nameof(AutoPut), _timeToDeactivationBullet);
    }
    private void Update()
    {
        transform.Translate(transform.forward * (SpeedBullet * Time.deltaTime), Space.World);
        if (TypeBullet == TypeBullet.Rocket)
        {
            _trile.transform.Translate(transform.forward * (SpeedBullet * Time.deltaTime), Space.World);
        }
    }
    private void Explosion()
    {
        var colliders = Physics.SphereCastAll(transform.position, _radiusExplosion, transform.forward);
        foreach (var collider in colliders)
        {
            if(collider.collider.GetComponent<MainBase>())
            {                
                MainBase.CurrentHealthBase -= DamageBullet;
            }
            else if(collider.collider.TryGetComponent<Enemy>(out var enemy))
            {
                if (enemy.EnemyScriptable.TypeEnemy == TypeEnemy.Air)
                {
                    float distanceNormalize = Mathf.Abs((Vector3.Distance(transform.position, enemy.transform.position) / _radiusExplosion - 1f) * 100);
                    float distanceDamage = DamageBullet * distanceNormalize / 100;
                    enemy.CurrentHealthEnemy -= distanceDamage;
                }
            }
        }
    }
    private void AutoPut()
    {
        SpawnController.PutObject(gameObject);
        gameObject.layer = LayerMask.NameToLayer("Bullet");
    }

    private void OnTriggerEnter(Collider collider)
    {       
        if (TypeBullet == TypeBullet.Bullet)
        {
            if (gameObject.layer == LayerMask.NameToLayer("BulletEnemy") && collider.gameObject.GetComponent<MainBase>())
            {
                MainBase.CurrentHealthBase -= DamageBullet;
                EventAggregator.Post(this, new UpdateInfoMainBaseEvent() 
                { 
                    CurrentHealth = MainBase.CurrentHealthBase - DamageBullet,
                    MaxHealthBase = MainBase.CurrentLevel.MaxHealth
                });
            }
            if (collider.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.CurrentHealthEnemy -= DamageBullet;
            }
        }
        else if (TypeBullet == TypeBullet.Rocket)
        {
            Explosion();
        }
        AutoPut();
    }
    private void OnEnable()
    {
        if(TypeBullet == TypeBullet.Rocket)
        {
            _trile = SpawnController.GetObject(_startParticle);
            _trile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _trile.SetActive(true);
            _trile.GetComponent<AutoPutObject>().AutoPut();
        }
    }
}
public enum TypeBullet { Bullet, Rocket}
