//using UnityEngine;

//public class QuadrantHintSpawner : MonoBehaviour
//{
//    [Header("象限 Hint 物件（依照方向順序排列）")]
//    public QuadrantHint[] quadrantHints;

//    //private IDirectionProvider directionProvider;

//    // 外部注入敵人提供的方向資訊
//    //public void InjectDirectionProvider(IDirectionProvider provider)
//    //{
//    //    directionProvider = provider;
//    //}

//    /// 從 directionProvider 取得資料並生成象限提示
//    void Update()
//    {
//        if (directionProvider == null) return;

//        DirectionType[] types = directionProvider.GetAllDirectionTypes();

//        for (int i = 0; i < types.Length && i < quadrantHints.Length; i++)
//        {
//            if (types[i] != DirectionType.None && !quadrantHints[i].gameObject.activeSelf)
//            {
//                SpawnHint(i, types[i]);
//            }
//        }
//    }

//    /// 在指定方向生成一個象限提示
//    private void SpawnHint(int index, DirectionType dirType)
//    {
//        if (index < 0 || index >= quadrantHints.Length)
//            return;

//        var hint = quadrantHints[index];

//        if (!hint.gameObject.activeSelf)
//        {
//            hint.hintType = ConvertToHintType(dirType);
//            hint.gameObject.SetActive(true);
//        }
//    }

//    /// DirectionType 轉換為 HintType
//    private HintType ConvertToHintType(DirectionType type)
//    {
//        switch (type)
//        {
//            case DirectionType.WeakPoint: return HintType.Yellow;
//            case DirectionType.AttackPoint: return HintType.Red;
//            default: return HintType.None;
//        }
//    }
//}
