using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Helpers;
using Helpers.Events;

public enum EnemyState
{
    Movement,
    AttackMainBase
}
public class Enemy : MonoBehaviour, IDamageble
{
    private NavMeshAgent _agent;
    private AudioSource _audioSource;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform[] _spawnBullet;
    [SerializeField] private Transform _mainBaseTarget;
    [SerializeField] private string _layerBullet;
    private bool _hasAttack;
    public EnemyScriptable EnemyScriptable;
    private float _currentHealthEnemy;
    
    private EnemyState _currentEnemyState;
    public void SetDamage(float damageCount)
    {
        _currentHealthEnemy -= damageCount;

        if(_currentHealthEnemy <= 0)
        {
            Destroy(gameObject);
        }

        _healthSlider.value = _currentHealthEnemy;
        if (_currentHealthEnemy < EnemyScriptable.MaxHealthEnemy && _healthSlider.gameObject.activeSelf == false)
            _healthSlider.gameObject.SetActive(true);
    }
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _mainBaseTarget = FindObjectOfType<MainBase>().transform;
        _agent = GetComponent<NavMeshAgent>();
        StartCoroutine(ReloadCorutine());
        _currentEnemyState = EnemyState.Movement;
        _currentHealthEnemy = EnemyScriptable.MaxHealthEnemy;
        _healthSlider.maxValue = EnemyScriptable.MaxHealthEnemy;
        _healthSlider.value = EnemyScriptable.MaxHealthEnemy;
        _healthSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateStateAgent(_currentEnemyState);
        if (_currentHealthEnemy <= 0)
            Destroy(gameObject);
    }
    private void UpdateStateAgent(EnemyState enemyState)
    {
        _currentEnemyState = enemyState;       
        switch (enemyState)
        {
            case EnemyState.Movement:
                Movement();
                break;
            case EnemyState.AttackMainBase:
                AttackMainBase();
                break;            
        }
    }
    private void Movement()
    {
        _agent.destination = _mainBaseTarget.position;

        if (Vector3.Distance(_agent.transform.position, _mainBaseTarget.position) <= EnemyScriptable.RadiusShooting)
        {
            UpdateStateAgent(EnemyState.AttackMainBase);
        }
    }
    private void AttackMainBase()
    {
        if (_hasAttack)
        {
            _agent.destination = transform.position;
            Shooting(_mainBaseTarget);
        }
        if (Vector3.Distance(_agent.transform.position, _mainBaseTarget.position) > EnemyScriptable.RadiusShooting)
            UpdateStateAgent(EnemyState.Movement);
    }
    private void Shooting(Transform target)
    {
        if (_hasAttack)
        {
            for (int i = 0; i < _spawnBullet.Length; i++)
            {
                _spawnBullet[i].LookAt(target.transform);
                var bulletObject = SpawnController.GetObject(_bulletPrefab);
                bulletObject.layer = LayerMask.NameToLayer(_layerBullet);
                var bullet = bulletObject.GetComponent<Bullet>();
                bulletObject.transform.SetPositionAndRotation(_spawnBullet[i].position, _spawnBullet[i].rotation);

                bullet.DamageBullet = Random.Range(EnemyScriptable.MinDamage, EnemyScriptable.MaxDamage);
                bullet.SpeedBullet = EnemyScriptable.SpeedBulletEneny;

                bullet.gameObject.SetActive(true);
                bullet.AutoPutBullet();

                if (bullet.GetComponent<Bullet>())
                    AudioManager.InstanceAudio.PlaySfx(SfxType.Bullet, _audioSource);
                else if (bullet.GetComponent<RocketBehavior>())
                    AudioManager.InstanceAudio.PlaySfx(SfxType.RocketTrail, _audioSource);
            }

            _hasAttack = false;
            StartCoroutine(ReloadCorutine());
        }
    }
    IEnumerator ReloadCorutine()
    {
        yield return new WaitForSeconds(EnemyScriptable.ReloadGun);
        _hasAttack = true;
    }
    private void OnEnable()
    {
        if (EnemyScriptable.TypeEnemy == TypeEnemy.Ground)
            WaveController.EnemiesGroundList.Add(this);

        else if (EnemyScriptable.TypeEnemy == TypeEnemy.Air)
            WaveController.EnemiesAirList.Add(this);
    }
    private void OnDestroy()
    {
        EventAggregator.Post(this, new MoneyUpdateEvent() {MoneyCount = EnemyScriptable.Reward});
        EventAggregator.Post(this, new EnemyDeathEvent());
        if (EnemyScriptable.TypeEnemy == TypeEnemy.Ground)
            WaveController.EnemiesGroundList.Remove(this);
        else
            WaveController.EnemiesAirList.Remove(this);
    }
}