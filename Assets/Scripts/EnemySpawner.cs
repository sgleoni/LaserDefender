using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfigSO> waveConfigs;
    [SerializeField] float timeBetweenWaves = 0f;
    [SerializeField] float waveTimeVariance = 0f;
    [SerializeField] float minimumWaveTime = 0.5f;
    [SerializeField] bool isLooping = false;

    WaveConfigSO currentWave;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    public WaveConfigSO GetCurrentWave()
    {
        return currentWave;
    }

    IEnumerator SpawnEnemies()
    {
        do
        {
            foreach (WaveConfigSO wave in waveConfigs)
            {
                currentWave = wave;

                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(
                        currentWave.GetEnemyPrefab(i),
                        currentWave.GetStartingWaypoint().position,
                        Quaternion.Euler(0, 0, 180),
                        transform
                    );

                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }

                yield return new WaitForSeconds(GetRandomTimeBetweenWaves());
            }
        } while (isLooping);
    }

    private float GetRandomTimeBetweenWaves()
    {
        float waveTime = Random.Range(
            timeBetweenWaves - waveTimeVariance,
            timeBetweenWaves + waveTimeVariance
        );

        return Mathf.Clamp(waveTime, minimumWaveTime, float.MaxValue);
    }
}
