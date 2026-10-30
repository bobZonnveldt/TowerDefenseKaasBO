using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class WaveData
{
    public float duration = 10f;
    public int easyenemies = 5;
    public int hardenemies = 3;
}
public class Wavemanager : MonoBehaviour
{
    public WaveData[] waves;
    public Button startWaveButton;
    public GameObject easyenemyprefab;

    public GameObject hardenemyprefab;

    public Transform[] Waypoints;

    public int Currentwaveindex = 0;
    public bool Waverunning = false;
   
   
    void Start()
    {
        startWaveButton.onClick.AddListener(StartWave);
    }

    // Update is called once per frame
    public void StartWave()
    {
        if(Waverunning) return;
        if(Currentwaveindex >= waves.Length) return;
        
        StartCoroutine(RunWave());
    }   

    IEnumerator RunWave()
    {
        Waverunning = true;
       startWaveButton.interactable = false;
       WaveData wava = waves[Currentwaveindex];
       for(int i = 0; i < wava.easyenemies; i++)
       {
            SpawnEnemy(easyenemyprefab);
            yield return new WaitForSeconds((wava.duration / 3) / wava.easyenemies);
       }

       for(int i = 0; i < wava.hardenemies; i++)
       {
            SpawnEnemy(hardenemyprefab);
            yield return new WaitForSeconds((wava.duration / 3) / wava.hardenemies);
       }
       yield return new WaitForSeconds(wava.duration / 3);

       Waverunning = false;
       Currentwaveindex++;
       startWaveButton.interactable = true;
    }

    void SpawnEnemy(GameObject enemyprefab)
    {
        GameObject enemy = Instantiate(enemyprefab, Waypoints[0].position, Quaternion.identity);
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.waypoints = Waypoints;
    }

    void Update()
    {
        
    }
}
