using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TeleportPlate : MonoBehaviour
{
    public string triggerTag = "Player";
    
    public Vector3 targetPosition;


    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            other.transform.position = targetPosition;
            Debug.Log($"Player ¶Ç°e¨ì {targetPosition}");
        }
    }
}