using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;

public class EmotionHandler : MonoBehaviour
{

    [FoldoutGroup("🎭 Emotion List")]
    [SerializeField, ListDrawerSettings(ShowFoldout = true)]
    private List<Emotion> _emotions = new();


    private int _currentEmotionIndex = 0;
    private string _currentEmotionID; // 실제 감정 ID는 string으로 저장

    private void Awake()
    {
        FetchEmotions();
    }



    public void FetchEmotions()
    {
        _emotions = new List<Emotion>(GetComponentsInChildren<Emotion>(true));
    }

    public void SetEmotion(string emotionID, float duration, bool isRuntime = true)
    {
        if (string.IsNullOrEmpty(emotionID))
        {
            Debug.LogWarning("[EmotionHandler] 감정 ID가 null 또는 빈 값입니다.");
            return;
        }

        Emotion newEmotion = GetEmotion(emotionID);
        if (newEmotion == null)
        {
            Debug.LogWarning($"[EmotionHandler] 감정 `{emotionID}`이(가) 존재하지 않습니다.");
            return;
        }
        if (_currentEmotionID == emotionID)
        {
            Debug.LogWarning("[EmotionHandler] 이미 동일한 감정이 적용 중입니다.");
            return;
        }

        _currentEmotionID = emotionID;

        foreach (var emo in _emotions)
        {
            bool isTarget = emo == newEmotion;
            if(isTarget){
                emo.gameObject.SetActive(true);
                emo.FadeFrom(target : 1f, 0f, duration);
            }
            else{
                emo.FadeOut(duration);
                emo.gameObject.SetActive(false, duration);
            }
        }
        if(isRuntime){
            newEmotion.Init();
        }
        Debug.Log($"[EmotionHandler] 감정 변경: `{_currentEmotionID}`");
    }

    public Emotion GetEmotion(string emotionID)
    {
        return _emotions.Find(e => e.EmotionID == emotionID);
    }

    public void PlayMouthWithCurrentEmotion(bool b)
    {
        Emotion currentEmotion = GetEmotion(_currentEmotionID);
        if (currentEmotion != null) 
            currentEmotion.PlayMouth(b);
    }


   #if UNITY_EDITOR
     [Button("🎭 Export Emotion Enum")]
    private void ExportEmotionEnum()
    {
        Character character = GetComponentInParent<Character>();
        if (character == null)
        {
            Debug.LogError("[EmotionHandler] Character 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        string enumName = $"{character.CharacterName}EmotionType";
        string folderPath = "Assets/Scripts/Enums";
        string filePath = $"{folderPath}/{enumName}.cs";

        if (_emotions.Count == 0)
        {
            Debug.LogWarning("[EmotionHandler] 감정 리스트가 비어 있습니다.");
            return;
        }

        // ✅ 감정 리스트를 알파벳순 정렬 후, 예쁘게 들여쓰기 적용
        List<string> enumValues = new List<string>();
        foreach (var emotion in _emotions)
        {
            if (!string.IsNullOrEmpty(emotion.EmotionID))
            {
                enumValues.Add($"    {emotion.EmotionID},");
            }
        }
        enumValues.Sort(); // 알파벳순 정렬

        string enumContent = $@"public enum {enumName}
{{
{string.Join("\n", enumValues)}
}}";

        // ✅ 폴더가 존재하지 않으면 생성
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // ✅ 파일이 이미 존재하는 경우 덮어쓰기 여부 확인
        if (File.Exists(filePath))
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "파일 덮어쓰기 확인",
                $"파일 `{enumName}.cs`가 이미 존재합니다.\n덮어쓰시겠습니까?",
                "Yes", "No"
            );

            if (!overwrite)
            {
                Debug.Log($"[EmotionHandler] `{enumName}.cs` 생성이 취소되었습니다.");
                return;
            }
        }

        // ✅ 파일 저장 및 Unity 프로젝트 반영
        File.WriteAllText(filePath, enumContent + "\n"); // 마지막 줄바꿈 추가
        AssetDatabase.Refresh();
        Debug.Log($"[EmotionHandler] {enumName}.cs 생성 완료: {filePath}");

        // ✅ 파일 저장 후 자동으로 열기
        EditorUtility.RevealInFinder(filePath);
        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(filePath, 0);
    }




    /// <summary>
    /// ✅ **Emotion 순차 변경 버튼** (Odin Inspector 버튼)
    /// </summary>
    [Button("➡ Next Emotion", ButtonSizes.Gigantic)]
    private void CycleEmotion()
    {
        FetchEmotions();

        if (_emotions.Count == 0)
        {
            Debug.LogWarning("[EmotionHandler] No emotions found.");
            return;
        }

        // ✅ 순회 로직 (리스트 끝에 도달하면 처음으로 돌아감)
        _currentEmotionIndex = (_currentEmotionIndex + 1) % _emotions.Count;
        SetEmotion(_emotions[_currentEmotionIndex].EmotionID, 0f, false);

        // ✅ SceneView & 인스펙터 갱신
        EditorApplication.delayCall += () =>
        {
            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        };
    }
    /// <summary>
    /// ✅ **에디터에서 Transform (Local Position, Local Rotation) 고정**
    /// </summary>
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = Vector3.zero; // ✅ 항상 (0,0,0) 유지
            transform.localRotation = Quaternion.identity; // ✅ 항상 회전 없음
            transform.localScale = Vector3.one;
            
            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        }
    }

    
    #endif

}
