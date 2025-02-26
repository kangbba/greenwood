using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static BigPlaceNames;
using static CharacterEnums;
using static SmallPlaceNames;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("🏠 Initializing Game...");
        
        // 최초 PlaceGroup 설정 (Town)

        // ✅ GameSaveDataProvider를 ItemManager에 주입
        ItemManager.Instance.SetItemProvider(new GameSaveDataProvider());
         // ✅ BigPlace 이동 시, 이 Dictionary를 넘김

        PlaceManager.Instance.Init();
        PlayerManager.Instance.MoveBigPlace(EBigPlaceName.Town);
    }
}
