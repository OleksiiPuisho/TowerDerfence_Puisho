namespace Helpers.Events
{
    public class GameOverEvent
    {
    }
    public class GameWinEvent
    {

    }
    public class StartGameEvent
    {
        public int StartMoney;
    }
    public class MoneyUpdateEvent
    {
        public int MoneyCount;
    }
    public class MoneyUpdateUIEvent
    {
        public int Count;
    }
    public class LevelMaxEvent
    {
        public bool IsUpgreded;
    }
    public class NotEnoughMoneyEvent
    {
    }
    public class DeselectedAllEvent
    {
    }
    public class SelectedTowerEvent
    {
        public string Name;
        public string Level;
        public float MinDamage;
        public float MaxDamage;
        public string Radius;
        public string AttackType;
        public string PriceUpgrade;
        public int BulletSpawnCount;
    }
    public class SelectedMainBaseEvent
    {
        public string Level;
        public string MaxHealthBase;
        public string PriceUpgrade;
        public int IndexInMassive;
    }
    public class SelectedBuildPointEvent
    {
    }
    public class StartWaveEvent
    {
        public int CountEnemyInWaveStart;
        public int CurrentWaveCount;
        public int AllWave;
    }
    public class WaitWaweEvent
    {
        public int Timer;
        public int MaxTime;
    }
    public class UpdateInfoWaveEvent
    {
        public int EnemiesLeft;
    }
    public class UpdateInfoMainBaseEvent
    {
        public float MaxHealthBase;
        public float CurrentHealth;
        public float RepairPrice;
    }
    public class EnemyDeathEvent
    {
        
    }
    public class FastStartWaveEvent
    {

    }
}