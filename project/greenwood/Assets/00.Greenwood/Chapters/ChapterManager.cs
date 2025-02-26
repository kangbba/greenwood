using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;
using static BigPlaceNames;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance { get; private set; }

    [SerializeField, ListDrawerSettings(ShowFoldout = true)]
    private List<Chapter> _chapters; // ✅ 모든 챕터 리스트

    private int _currentChapterIndex = -1; // ✅ 현재 챕터 인덱스
    private ReactiveProperty<Chapter> _currentChapter = new ReactiveProperty<Chapter>(); // ✅ 현재 챕터
    private CompositeDisposable _currentChapterSubscriptions = new CompositeDisposable(); // ✅ 현재 챕터 구독 관리

    public IReadOnlyReactiveProperty<Chapter> CurrentChapter => _currentChapter;

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
            return;
        }
    }

    private void Start()
    {
        if (_chapters == null || _chapters.Count == 0)
        {
            Debug.LogError("[ChapterManager] 챕터 리스트가 비어있습니다!");
            return;
        }

        // ✅ 게임 시작 시 첫 번째 챕터 시작
        StartChapter(0);
    }

    /// ✅ 챕터 시작 메서드 (이전 구독 해지 & 방문 기록 초기화 포함)
    /// </summary>
    private void StartChapter(int chapterIndex)
    {
        if (chapterIndex < 0 || chapterIndex >= _chapters.Count)
        {
            Debug.LogError($"[ChapterManager] 유효하지 않은 챕터 인덱스: {chapterIndex}");
            return;
        }

        // ✅ 이전 챕터 구독 해제
        _currentChapterSubscriptions.Clear();

        _currentChapterIndex = chapterIndex;
        _currentChapter.Value = _chapters[chapterIndex]; // ✅ 현재 챕터 업데이트

        Debug.LogWarning($"🔥🔥🔥 [ChapterManager] 챕터 {_currentChapter.Value.ChapterNumber} 시작! - {_currentChapter.Value.ChapterName} 🔥🔥🔥");

        // ✅ 이전 방문 기록 초기화
        ResetVisitedPlaces();

        // ✅ 장소 및 플레이어 위치 재설정
        PlaceManager.Instance.RecreateBigPlaces();
        PlayerManager.Instance.MoveBigPlace(_currentChapter.Value.BigPlaceToStart);

        // ✅ 챕터 클리어 조건 감지
        _currentChapter.Value.ClearConditions.IsSatisfiedAllStream()
            .Subscribe(isCleared =>
            {
                if (isCleared)
                {
                    Debug.LogWarning($"✅✅✅ [ChapterManager] 챕터 {_currentChapter.Value.ChapterNumber} 클리어! ✅✅✅");
                    MoveToNextChapter();
                }
            })
            .AddTo(_currentChapterSubscriptions);
    }


    /// <summary>
    /// ✅ 다음 챕터로 이동
    /// </summary>
    private void MoveToNextChapter()
    {
        if (_currentChapterIndex + 1 < _chapters.Count)
        {
            StartChapter(_currentChapterIndex + 1);
        }
        else
        {
            Debug.LogWarning("🎉🎉🎉 [ChapterManager] 모든 챕터 클리어! 🎉🎉🎉");
        }
    }

    /// <summary>
    /// ✅ 방문 기록 초기화
    /// </summary>
    private void ResetVisitedPlaces()
    {
        PlayerManager.Instance.ClearVisitedPlaces();
        Debug.Log("🧹 [ChapterManager] 이전 챕터의 방문 기록이 초기화됨!");
    }
}
