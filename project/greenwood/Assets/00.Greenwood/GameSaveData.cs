using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ✅ 게임 저장 데이터 (필수 데이터만 저장)
/// </summary>
[Serializable]
public class GameSaveData
{
    public List<string> OwnedItemIDs;  // ✅ 보유한 아이템 ID 목록
    public List<string> OwnedFlags;  // ✅ 활성화된 플래그 목록
    public Dictionary<string, int> Affinities;  // ✅ 캐릭터별 호감도
    public int CurrentChapter;  // ✅ 현재 진행 중인 챕터
    public string CurrentBigPlaceName;  // ✅ 현재 위치한 BigPlace 이름

    /// <summary>
    /// ✅ 생성자를 통한 저장 데이터 초기화
    /// </summary>
    public GameSaveData(
        List<string> ownedItemIDs,
        List<string> ownedFlags,
        Dictionary<string, int> affinities,
        int currentChapter,
        string currentBigPlaceName)
    {
        OwnedItemIDs = ownedItemIDs;
        OwnedFlags = ownedFlags;
        Affinities = affinities;
        CurrentChapter = currentChapter;
        CurrentBigPlaceName = currentBigPlaceName;
    }

    /// <summary>
    /// ✅ JSON 변환하여 저장
    /// </summary>
    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }

    /// <summary>
    /// ✅ JSON 데이터를 받아와 역직렬화 (불러오기)
    /// </summary>
    public static GameSaveData Deserialize(string jsonData)
    {
        return string.IsNullOrEmpty(jsonData) ? new GameSaveData(new List<string>(), new List<string>(), new Dictionary<string, int>(), 0, "") : JsonUtility.FromJson<GameSaveData>(jsonData);
    }
}
