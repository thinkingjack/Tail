using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinCount = 20;
    public Vector2 areaSize = new Vector2(5f, 5f);

    void Start()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(-areaSize.x, areaSize.x),
                Random.Range(-areaSize.y, areaSize.y)
            );
            Instantiate(coinPrefab, pos, Quaternion.identity);
        }
    }
}