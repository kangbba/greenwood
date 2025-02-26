using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static BigPlaceNames;

[CreateAssetMenu(fileName = "NewChapter", menuName = "Greenwood/Chapter")]
public class Chapter : ScriptableObject
{
    public int ChapterNumber;
    public string ChapterName;
    public EBigPlaceName BigPlaceToStart; // ✅ 챕터 시작 장소
    public EventConditions ClearConditions; // ✅ 챕터 클리어 조건 유지

}
