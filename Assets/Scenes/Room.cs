using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public string roomId;
    public List<GameObject> walls = new List<GameObject>();
    public string enemyTag = "Enemy";

    private List<GameObject> initialEnemies = new List<GameObject>();

    public void Initialize()
    {
        RecordInitialEnemies();
        GetComponent<BoxCollider>().enabled = false;
    }

    private void RecordInitialEnemies()
    {
        var box = GetComponent<BoxCollider>();
        Collider[] colliders = Physics.OverlapBox(
            transform.position + box.center,
            box.size / 2,
            transform.rotation
        );

        foreach (var col in colliders)
        {
            if(col.CompareTag("Untagged"))
                continue;
            if (col.CompareTag(enemyTag))
            {
                var enemy = col.gameObject;
                if (!initialEnemies.Contains(enemy))
                    initialEnemies.Add(enemy);
            }
        }

        Debug.Log($"Room{roomId} Enemy Count: {initialEnemies.Count}");
    }

    public bool IsCleared()
    {
        initialEnemies.RemoveAll(e => e == null);
        return initialEnemies.Count == 0;
    }

    public void RemoveWalls()
    {
        foreach (var wall in walls)
        {
            if (wall != null)
                Destroy(wall);
        }
        walls.Clear();
    }
}
