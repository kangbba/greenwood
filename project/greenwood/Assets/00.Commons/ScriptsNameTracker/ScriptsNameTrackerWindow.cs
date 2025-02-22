using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScriptsNameTrackerWindow : EditorWindow
{
    private Dictionary<string, List<ScriptData>> _scriptGroups = new Dictionary<string, List<ScriptData>>();
    private Vector2 _scrollPosition;

    // 상태별 필터 토글
    private bool _showNormal = true;
    private bool _showNoClass = true;
    private bool _showMultipleClasses = true;
    private bool _showMismatch = true;

    // 무시 폴더 목록 (Assets/로 시작)
    private List<string> _ignorePaths = new List<string>();

    // 선택된 스크립트의 AssetPath
    private string _selectedAssetPath = null;

    [MenuItem("Tools/Scripts Name Tracker")]
    public static void ShowWindow()
    {
        GetWindow<ScriptsNameTrackerWindow>("스크립트 이름 추적기");
    }

    private void OnGUI()
    {
        GUILayout.Space(5);
        EditorGUILayout.LabelField("필터 설정", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        _showNormal = EditorGUILayout.ToggleLeft("정상", _showNormal, GUILayout.Width(80));
        _showNoClass = EditorGUILayout.ToggleLeft("클래스 없음", _showNoClass, GUILayout.Width(100));
        _showMultipleClasses = EditorGUILayout.ToggleLeft("클래스 다중", _showMultipleClasses, GUILayout.Width(100));
        _showMismatch = EditorGUILayout.ToggleLeft("이름 불일치", _showMismatch, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.LabelField("무시 폴더 목록", EditorStyles.boldLabel);
        if (_ignorePaths.Count == 0)
        {
            EditorGUILayout.LabelField("없음", EditorStyles.miniLabel);
        }
        else
        {
            for (int i = 0; i < _ignorePaths.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{i + 1}.", GUILayout.Width(20));
                GUILayout.Label(_ignorePaths[i], GUILayout.ExpandWidth(true));
                if (GUILayout.Button("삭제", GUILayout.Width(50)))
                {
                    _ignorePaths.RemoveAt(i);
                    i--;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("🔍 스크립트 검사", GUILayout.Height(30)))
        {
            _scriptGroups = ScriptNameChecker.CheckScriptNames(_ignorePaths);
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("검사 결과", EditorStyles.boldLabel);
        if (_scriptGroups.Count == 0)
        {
            EditorGUILayout.LabelField("스크립트가 없습니다.", EditorStyles.miniLabel);
            return;
        }

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 150));
        int globalIndex = 1;
        foreach (var group in _scriptGroups)
        {
            // 그룹 내 스크립트 필터링
            List<ScriptData> groupScripts = group.Value.Where(s => ShouldDisplay(s.Type)).ToList();
            if (groupScripts.Count == 0)
                continue;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"📂 {group.Key}", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // 표 헤더 (한글 중앙 정렬)
            EditorGUILayout.BeginHorizontal();
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            GUILayout.Label("번호", headerStyle, GUILayout.Width(40));
            GUILayout.Label("상태", headerStyle, GUILayout.Width(60)); // 아이콘
            GUILayout.Label("상태 사유", headerStyle, GUILayout.Width(120));
            GUILayout.Label("스크립트 이름", headerStyle, GUILayout.Width(200));
            GUILayout.Label("발견된 클래스", headerStyle, GUILayout.Width(250));
            EditorGUILayout.EndHorizontal();

            foreach (var scriptData in groupScripts)
            {
                Rect rowRect = EditorGUILayout.BeginHorizontal();

                // 배경 처리: 선택된 행은 색상 적용
                if (_selectedAssetPath == scriptData.AssetPath)
                {
                    EditorGUI.DrawRect(rowRect, new Color(0.3f, 0.6f, 1f, 0.3f));
                }

                GUILayout.Label(globalIndex.ToString(), GUILayout.Width(40));
                GUILayout.Label(GetStatusIcon(scriptData.Type), GUILayout.Width(60));
                GUILayout.Label(scriptData.Type.ToString(), GUILayout.Width(120));
                GUIStyle centeredStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter };
                GUILayout.Label(scriptData.FileName, centeredStyle, GUILayout.Width(200));
                string classesText = scriptData.DetectedClasses.Any() ? string.Join("\n", scriptData.DetectedClasses) : "-";
                // 자동 높이: 각 줄 20 픽셀로 계산 (최소 20)
                GUILayout.Label(classesText, centeredStyle, GUILayout.Width(250), GUILayout.Height(20 * Mathf.Max(1, scriptData.DetectedClasses.Count)));
                EditorGUILayout.EndHorizontal();

                // 행 선택 이벤트: 단일 클릭 시 선택 및 핑, 더블 클릭 시 열기
                HandleRowClick(rowRect, scriptData.AssetPath);

                globalIndex++;
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        EditorGUILayout.EndScrollView();
    }

    private bool ShouldDisplay(ScriptErrorType type)
    {
        return (type == ScriptErrorType.Normal && _showNormal)
            || (type == ScriptErrorType.NoClass && _showNoClass)
            || (type == ScriptErrorType.MultipleClasses && _showMultipleClasses)
            || (type == ScriptErrorType.Mismatch && _showMismatch);
    }

    private void HandleRowClick(Rect rect, string assetPath)
    {
        Event e = Event.current;
        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            if (e.clickCount == 1)
            {
                _selectedAssetPath = assetPath;
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(assetPath));
                Repaint();
            }
            if (e.clickCount >= 2 && rect.Contains(e.mousePosition))
            {
                OpenScript(assetPath);
            }
        }
    }

    private void OpenScript(string assetPath)
    {
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        if (obj != null)
        {
            AssetDatabase.OpenAsset(obj);
        }
    }

    private string GetStatusIcon(ScriptErrorType type)
    {
        return type switch
        {
            ScriptErrorType.NoClass => "🚨",
            ScriptErrorType.MultipleClasses => "⚠️",
            ScriptErrorType.Mismatch => "❗",
            _ => "✅"
        };
    }
}
