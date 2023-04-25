using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerSpace
{
    public class RocketTower : Tower
    {
        private void OnEnable()
        {
            StartCoroutine(TowerReloadCorutine());
            AudioManager.InstanceAudio.PlaySfx(SfxType.BuildTower, _audioSource);
        }
        void Update()
        {
            UpdateState(CurrentState);
        }
    }
}
