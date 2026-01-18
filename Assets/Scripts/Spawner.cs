using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;          // Ezt húzd be az inspectorban
    public float spawnDelay = 3f;      // 3 mp késleltetés
    public int maxObjects = 3;         // max 3 objektum

    private List<GameObject> spawnedObjects = new List<GameObject>();
    public bool spawnEnd = false;
    void Start()
    {
        // Kezdéskor feltöltjük a listát "helyekkel"
        for (int i = 0; i < maxObjects; i++)
        {
            spawnedObjects.Add(null);
        }

        // Indítjuk a spawn rutint
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (!spawnEnd)
        {
            yield return new WaitForSeconds(spawnDelay);

            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i] == null) // ha üres a hely
                {
                    GameObject newObj = Instantiate(prefab, transform.position, Quaternion.identity);
                    spawnedObjects[i] = newObj;
                    break; // csak egyet spawnolunk egyszerre
                }
            }
        }
    }
}
