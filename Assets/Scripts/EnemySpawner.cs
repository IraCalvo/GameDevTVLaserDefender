using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List <WaveConfigSO> waveConfig;
    [SerializeField] float timeBetweenWaves;
    WaveConfigSO currentWave;
    [SerializeField] bool isLooping;

    void Start()
    {
        StartCoroutine(SpawnEnemyWaves());
    }

    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }

    IEnumerator SpawnEnemyWaves()
    { 
        do
        {
            foreach(WaveConfigSO wave in waveConfig)
            {
                currentWave = wave;
                for(int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    //getting object prefab to spawn, the position of where to spawn it which is at the starting point that was set up, 180 rotation on Z so sprite matches, setting the enemy to be a child of EnemySpawner
                    Instantiate(currentWave.GetEnemyPrefab(i), currentWave.GetStartingWayPoint().position, Quaternion.Euler(0,0, 180), transform);
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        } 
        while (isLooping);
    }
}
