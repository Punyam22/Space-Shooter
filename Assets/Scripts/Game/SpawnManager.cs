using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab; 
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _isSpawning = true; 
    [SerializeField]
    private GameObject[] _powerups;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnTripleShot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3.0f); 
        while(_isSpawning == true){
            Vector3 pos = new Vector3(Random.Range(-9.0f ,9.0f) ,7.5f , 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnTripleShot()
    {
        yield return new WaitForSeconds(3.0f);
        while(_isSpawning == true){
            Vector3 pos= new Vector3(Random.Range(-9.0f ,9.0f) ,7.5f , 0);
            Instantiate(_powerups[Random.Range(0, 3)], pos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));
        }
    }
    public void StopSpawning()
    {
        _isSpawning = false;
    }
}
