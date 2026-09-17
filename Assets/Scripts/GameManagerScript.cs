using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// Gérer le gameplay global
public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject olivia;
    private OliviaScript oliviaScript;
    [SerializeField] GameObject monstre1;
    [SerializeField] GameObject monstre2;
    [SerializeField] GameObject monstre3;
    [SerializeField] Collider plancher;
    [SerializeField] private GameObject bouleBio;
    [SerializeField] private Material[] materialsBouleBio;
    private int kills = 0;
    private int bioresPoints = 0;
    private int bioPointsToUpgrade;
    public HashSet<GameObject> ennemis = new HashSet<GameObject>();
    private Vector3 min;
    private Vector3 max;

    void Start()
    {
        Time.timeScale = 1;

        if (DifficultyManager.instance.GetUpgradeRate() == 0)
        {
            bioPointsToUpgrade = 10;
        }
        else
        {
            bioPointsToUpgrade = DifficultyManager.instance.GetUpgradeRate();
        }

        oliviaScript = olivia.GetComponent<OliviaScript>();

        StartCoroutine(DelayBeforeFirstSpawn());
        StartCoroutine(DelayBeforeFirstWave());

        GameEvents.OnMonsterDied += HandleMonsterDied;
        GameEvents.OnUpgrade += ResetBioPoints;

        CheatEvents.OnSpawnBouleBio += SpawnBouleBioToOlivia;
        CheatEvents.OnGiveBioPoints += HandleBioPointsChange;
        CheatEvents.OnClearEnemies += ClearEnnemies;


        oliviaScript.OnOliviaDied += GameOver;
        oliviaScript.OnBioBallConsumed += HandleBioPointsChange;

        // Récupérer les limites du plancher
        min = plancher.bounds.min;
        max = plancher.bounds.max;
    }
    void OnDestroy()
    {
        GameEvents.OnMonsterDied -= HandleMonsterDied;
        GameEvents.OnUpgrade -= ResetBioPoints;

        CheatEvents.OnSpawnBouleBio -= SpawnBouleBioToOlivia;
        CheatEvents.OnGiveBioPoints -= HandleBioPointsChange;
        CheatEvents.OnClearEnemies -= ClearEnnemies;
    }
    // Gérer la fin de partie
    public void GameOver()
    {
        oliviaScript.OnBioBallConsumed -= HandleBioPointsChange;
        oliviaScript.OnOliviaDied -= GameOver;
        Time.timeScale = 0;
        GameEvents.OnGameOver?.Invoke(kills);
    }
    // Supprimer tous les ennemis
    private void ClearEnnemies()
    {
        foreach (var ennemi in ennemis)
        {
            Destroy(ennemi);
        }
        ennemis.Clear();
        GameEvents.OnMonstersAliveChanged.Invoke(ennemis.Count);
    }
    public void SpawnMonstre()
    {

        Vector3 spawnPos = Vector3.zero;

        GameObject[] typesMonstres = { monstre1, monstre2, monstre3 };
        int rng = Random.Range(0, typesMonstres.Length);

        float radius = typesMonstres[rng].GetComponent<SphereCollider>().radius;

        int attempts = 0;
        while (attempts < 10)
        {
            float x = Random.Range(min.x, max.x);
            float y = plancher.bounds.max.y + radius;  // On spawn les monstres au dessus du plancher
            float z = Random.Range(min.z, max.z);

            Vector3 testPos = new Vector3(x, y, z);
            if (!Physics.CheckSphere(testPos, radius * 0.8f)) // Check if spawn overlaps with other colliders, with reduced radius to avoid ground
            {
                spawnPos = testPos;
                break; ;
            }
            attempts++;
            if (attempts >= 10)
            {
                Debug.Log("Canceled spawn");
                return;
            }
        }




        GameObject ins = Instantiate(typesMonstres[rng], spawnPos, Quaternion.identity);
        EnnemiScript ennemiScript = ins.GetComponent<EnnemiScript>();
        ennemiScript.SetOlivia(olivia);
        ennemiScript.SetEstPeureux(rng == 1);
        ennemiScript.SetHasAttackAnimation(rng != 2);
        ennemis.Add(ins);
        GameEvents.OnMonstersAliveChanged?.Invoke(ennemis.Count);
    }
    // Spawn des monstres à intervalle régulier
    private IEnumerator SingleSpawn()
    {
        while (true)
        {
            //Debug.Log("SPAWNed MONSTER");
            SpawnMonstre();
            yield return new WaitForSeconds(3);
        }
    }
    // Spawn des vagues de monstres à intervalle régulier
    private IEnumerator WaveSpawn()
    {
        while (true)
        {
            for (int s = 0; s < DifficultyManager.instance.GetMonsterSpawnRate(); s++)
            {
                SpawnMonstre();
            }
            yield return new WaitForSeconds(15);
        }
    }
    // Délai avant les premiers spawns des monstres et des vagues
    private IEnumerator DelayBeforeFirstSpawn()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(SingleSpawn());
    }
    private IEnumerator DelayBeforeFirstWave()
    {
        yield return new WaitForSeconds(15);
        StartCoroutine(WaveSpawn());
    }
    // Gérer la mort d'un monstre
    private void HandleMonsterDied(GameObject monster)
    {
        ennemis.Remove(monster);
        SpawnBouleBio(monster.transform.position);
        kills++;
        GameEvents.OnMonsterKilled?.Invoke(kills);
        GameEvents.OnMonstersAliveChanged?.Invoke(ennemis.Count);
    }
    // Spawn boule bio à position donnée
    private void SpawnBouleBio(Vector3 position)
    {
        int rng = Random.Range(0, materialsBouleBio.Length);
        bouleBio.GetComponent<Renderer>().material = materialsBouleBio[rng];
        Instantiate(bouleBio, position, Quaternion.identity);
    }
    // Spawn boule bio à proximité d'Olivia
    private void SpawnBouleBioToOlivia()
    {

        float rngX = Random.Range(olivia.transform.position.x - 5, olivia.transform.position.x + 5);
        float rngZ = Random.Range(olivia.transform.position.z - 5, olivia.transform.position.z + 5);

        rngX = Mathf.Clamp(rngX, min.x + 1, max.x - 1);
        rngZ = Mathf.Clamp(rngZ, min.z + 1, max.z - 1);
        Vector3 spawnPosition = new Vector3(rngX, 0, rngZ);

        SpawnBouleBio(spawnPosition);
    }
    // Gérer les points de biores
    private void HandleBioPointsChange(int addedPoints)
    {
        bioresPoints += addedPoints;
        GameEvents.OnBioPointsChanged?.Invoke(bioresPoints);
        if (bioresPoints >= bioPointsToUpgrade)
        {
            GameEvents.OnUpgrade?.Invoke();
        }
    }
    // Réinitialiser les points de biores après une upgrade
    private void ResetBioPoints()
    {
        bioresPoints = 0;
        GameEvents.OnBioPointsChanged?.Invoke(bioresPoints);
    }
}
