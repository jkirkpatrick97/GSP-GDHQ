using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private float enemyWaitTime = 5.0f;
    [SerializeField]
    private GameObject _enemyContainer;
    

    private bool _stopSpawning = false;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            GameObject newEnemy =  Instantiate(_enemyPrefab, new Vector3(Random.Range(-8f, 8f), 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(enemyWaitTime);
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
        
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2);
        while(_stopSpawning == false)
        {
            GameObject newPowerup = Instantiate(_powerups[Random.Range(0,4)], new Vector3(Random.Range(-8f, 8f), 8, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
        
    }

    public void StartSpawning()
    {
        StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine("SpawnPowerupRoutine");
    }

    
}
