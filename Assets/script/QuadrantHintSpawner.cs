using UnityEngine;
using System.Collections;
using System;

public class QuadrantHintSpawner : MonoBehaviour
{
    public QuadrantHint[] quadrantHints;
    public float minDelay = 2f;
    public float maxDelay = 8f;
    public int maxActiveHints = 3;
    void Start()
    {
        foreach (QuadrantHint hint in quadrantHints)
        {
            hint.gameObject.SetActive(false);
            StartCoroutine(SpawnHintLoop(hint));
        }
    }

    IEnumerator SpawnHintLoop(QuadrantHint hint)
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            if (!hint.gameObject.activeSelf && GetActiveHintCount() < maxActiveHints)
            {
                // 隨機設定紅或黃事件
                hint.hintType = (UnityEngine.Random.value > 0.5f) ? HintType.Red : HintType.Yellow;
                hint.gameObject.SetActive(true);
            }
        }
    }

    int GetActiveHintCount()
    {
        int count = 0;
        foreach (QuadrantHint hint in quadrantHints)
        {
            if (hint.gameObject.activeSelf)
                count++;
        }
        return count;
    }
}
