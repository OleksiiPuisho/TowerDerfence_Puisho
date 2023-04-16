using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using Helpers.Events;

public class Bullet : MonoBehaviour
{
    [SerializeField] private TypeBullet _typeBullet;
    [SerializeField] private float _timeToDeactivationBullet;
    [HideInInspector] public float SpeedBullet;
    [HideInInspector] public float DamageBullet;
    [SerializeField] private float _radiusExplosion;
    public void AutoPutBullet()
    {
        Invoke(nameof(AutoPut), _timeToDeactivationBullet);
    }
    private void Update()
    {
        transform.Translate(transform.forward * (SpeedBullet * Time.deltaTime), Space.World);
    }
    private void Explosion()
    {
        var colliders = Physics.SphereCastAll(transform.position, _radiusExplosion, transform.forward);
        Debug.Log(colliders.Length);
        foreach (var collider in colliders)
        {
            if(collider.collider.TryGetComponent<MainBase>(out var mainBase))
            {                
                MainBase.CurrentHealthBase -= DamageBullet;
            }
            else if(collider.collider.TryGetComponent<Enemy>(out var enemy))
            {
                if (enemy.EnemyScriptable.TypeEnemy == TypeEnemy.Air)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    float distanceDamage = distance * 100 / DamageBullet;
                    enemy.CurrentHealthEnemy -= distanceDamage;
                }
            }
        }
    }
    private void AutoPut()
    {
        SpawnController.PutObject(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {       
        if (_typeBullet == TypeBullet.Bullet)
        {
            if (gameObject.layer == LayerMask.NameToLayer("BulletEnemy") && collision.gameObject.GetComponent<MainBase>())
            {
                MainBase.CurrentHealthBase -= DamageBullet;
                EventAggregator.Post(this, new UpdateInfoMainBaseEvent() 
                { 
                    CurrentHealth = MainBase.CurrentHealthBase - DamageBullet,
                    MaxHealthBase = MainBase.CurrentLevel.MaxHealth
                });
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
        SpawnController.PutObject(gameObject);
    }
    private void OnEnable()
    {
        
    }
}
public enum TypeBullet { Bullet, Rocket}
