// singleton pour stocker les données de difficulté du jeu
public class DifficultyManager
{
    public static DifficultyManager instance { get; private set; } = new DifficultyManager();

    private int _OliviaHp;
    private int _upgradeRate;
    private int _monsterSpawnRate;

    public void SetDifficulty(int oliviaHp, int upgradeRate, int monsterSpawnRate)
    {
        _OliviaHp = oliviaHp;
        _upgradeRate = upgradeRate;
        _monsterSpawnRate = monsterSpawnRate;
    }
    public int GetOliviaHp()
    {
        return _OliviaHp;
    }
    public int GetUpgradeRate()
    {
        return _upgradeRate;
    }
    public int GetMonsterSpawnRate()
    {
        return _monsterSpawnRate;
    }
}