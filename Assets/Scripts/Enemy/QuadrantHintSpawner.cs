using UnityEngine;
using UnityEngine.UI;

public class QuadrantHintSpawner : MonoBehaviour
{
    [Header("Hint Settings")]
    public QuadrantHint[] quadrantHints;
    
    void Update()
    {
        // if (directionProvider == null) return
        // DirectionType[] types = directionProvider.GetAllDirectionTypes();
        // for (int i = 0; i < types.Length && i < quadrantHints.Length; i++)
        // {
        //     if (types[i] != DirectionType.None && !quadrantHints[i].gameObject.activeSelf)
        //     {
        //         SpawnHint(i, types[i]);
        //     }
        // }
    }
    /// Spawn Hint
    private void SpawnHint(int index, DirectionType dirType)
    {
        if (index < 0 || index >= quadrantHints.Length)
            return;
        var hint = quadrantHints[index];
        if (!hint.gameObject.activeSelf)
        {
            hint.hintType = ConvertToHintType(dirType);
            hint.gameObject.SetActive(true);
        }
    }
    /// Convert DirectionType to HintType
    private HintType ConvertToHintType(DirectionType type)
    {
        switch (type)
        {
            case DirectionType.WeakPoint: return HintType.Yellow;
            case DirectionType.AttackPoint: return HintType.Red;
            default: return HintType.None;
        }
    }

    public void UpdateAttackUI(DirectionType[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            if (directions[i] != DirectionType.None)
            {
                SpawnHint(i, directions[i]);
            }
        }
    }

    public void InteractWithDirection(int index)
    {
        if (index < 0 || index >= quadrantHints.Length)
            return;
        var hint = quadrantHints[index];
        SpawnHint(index, DirectionType.None);
    }
}
