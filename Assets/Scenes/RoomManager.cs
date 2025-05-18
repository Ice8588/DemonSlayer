using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private List<Room> rooms = new List<Room>();

    public float checkInterval = 1.0f;

    private void Start()
    {
        foreach (var room in rooms)
        {
            room.Initialize();
            StartCoroutine(MonitorRoom(room));
        }
    }

    private IEnumerator MonitorRoom(Room room)
    {
        while (true)
        {
            if (room.IsCleared())
            {
                Debug.Log($"Room {room.roomId} Cleared");
                room.RemoveWalls();
                yield break;
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }
}
