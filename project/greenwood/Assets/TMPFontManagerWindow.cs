using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public class TMPFontManagerWindow : EditorWindow
{
    private Vector2 _scrollPosition;
    private TMP_FontAsset _newFont;
    private string _searchFilter = "";

    private HierarchyNode _rootNode;
    private Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();

    // 토글: 에셋 포함 여부 (하이어라키는 항상 표시)
    private bool _includeAssets = false;  

    // 단순 목록 모드 토글: true이면 재귀적 폴드아웃, false이면 단순 목록(Flat) 모드
    private bool _showHierarchyMode = true; 

    // 이전 토글 값 (자동 갱신용)
    private bool _prevIncludeAssets = false;
    private bool _prevShowHierarchyMode = true;

    // 각 TMP 오브젝트의 선택 상태 (체크박스)
    private Dictionary<TextMeshProUGUI, bool> _selectedItems = new Dictionary<TextMeshProUGUI, bool>();

    [MenuItem("도구/TextMeshPro 폰트 관리자")]
    public static void ShowWindow()
    {
        TMPFontManagerWindow window = GetWindow<TMPFontManagerWindow>("TextMeshPro 폰트 관리자");
        window.minSize = new Vector2(800, 600);
        window.ScanAndBuildTree();
    }

    private void OnGUI()
    {
        GUILayout.Label("TextMeshPro 폰트 관리자", EditorStyles.boldLabel);
        GUILayout.Space(5);



        // 상단에 선택된 항목 수 표시
        int selectedCount = _selectedItems.Values.Count(v => v);

        GUILayout.Space(20);
        // 토글: 에셋 포함 여부
        _includeAssets = EditorGUILayout.Toggle("에셋 내 프리팹 포함 표시", _includeAssets);
        // 토글: 재귀적 폴드 사용 여부
        _showHierarchyMode = EditorGUILayout.Toggle("재귀적 폴드 사용", _showHierarchyMode);

        // 토글 값 변경 감지 -> 한 프레임 딜레이 후 갱신
        if (_prevIncludeAssets != _includeAssets || _prevShowHierarchyMode != _showHierarchyMode)
        {
            _prevIncludeAssets = _includeAssets;
            _prevShowHierarchyMode = _showHierarchyMode;
            EditorApplication.delayCall += () =>
            {
                ScanAndBuildTree();
                Repaint();
            };
        }
        // 검색 필터 입력창
        _newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("새 폰트 선택", _newFont, typeof(TMP_FontAsset), false);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("✅ 전체 적용", GUILayout.Height(40))) { ApplyFontToAll(); }
        if (GUILayout.Button("✅ 선택 적용", GUILayout.Height(40))) { ApplySelectedFont(); }
        if (GUILayout.Button("🔍 다시 불러오기", GUILayout.Height(40))) { ScanAndBuildTree(); }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(20);
        _searchFilter = EditorGUILayout.TextField("검색 필터", _searchFilter);
        GUILayout.Space(10);
        GUILayout.Label($"선택된 항목: {selectedCount}개");


        GUILayout.Space(10);
        // 스크롤뷰: 창 너비에 맞춰 확장, 가로 스크롤 없음
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        if (_rootNode != null)
        {
            if (_showHierarchyMode)
            {
                DrawNode(_rootNode, 0);
            }
            else
            {
                foreach (var group in _rootNode.Children.OrderBy(g => g.FullPath))
                {
                    DrawFlatGroup(group);
                }
            }
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Label($"현재 스크롤 위치: {_scrollPosition}");
    }

    private void ApplyFontToAll()
    {
        bool confirm = EditorUtility.DisplayDialog("⚠️ 전체 폰트 변경",
            "모든 TextMeshProUGUI의 폰트를 새 폰트로 변경하시겠습니까?",
            "예", "아니요");
        if (!confirm) return;

        Undo.RecordObject(this, "Change All TMP Fonts");
        ApplyFontRecursive(_rootNode);
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.Log("[TMPFontManager] 모든 TMP 폰트 변경 완료");
    }

    private void ApplyFontRecursive(HierarchyNode node)
    {
        foreach (TextMeshProUGUI tmp in node.TMPItems)
        {
            if (!MatchesFilter(tmp)) continue;
            Undo.RecordObject(tmp, "Change TMP Font");
            tmp.font = _newFont;
            EditorUtility.SetDirty(tmp);
        }
        foreach (var child in node.Children)
        {
            ApplyFontRecursive(child);
        }
    }

    // 선택된 항목에 대해서만 폰트 변경
    private void ApplySelectedFont()
    {
        var selectedTMPs = _selectedItems.Where(kv => kv.Value).Select(kv => kv.Key).ToList();
        if (selectedTMPs.Count == 0)
        {
            EditorUtility.DisplayDialog("알림", "선택된 항목이 없습니다.", "확인");
            return;
        }

        bool confirm = EditorUtility.DisplayDialog("⚠️ 선택 폰트 변경",
            $"선택된 {selectedTMPs.Count}개의 텍스트에 적용하시겠습니까?",
            "예", "아니요");
        if (!confirm) return;

        foreach (var tmp in selectedTMPs)
        {
            Undo.RecordObject(tmp, "Change TMP Font");
            tmp.font = _newFont;
            EditorUtility.SetDirty(tmp);
        }
        AssetDatabase.SaveAssets();
        Debug.Log($"[TMPFontManager] 선택된 {selectedTMPs.Count}개 텍스트의 폰트 변경 완료");
    }

    private void ScanAndBuildTree()
    {
        _rootNode = new HierarchyNode("루트", "");
        _foldoutStates.Clear();
        _selectedItems.Clear();

        // 하이어라키: 씬 내 TMP 검색
        List<TextMeshProUGUI> allTMPs = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None).ToList();
        foreach (var tmp in allTMPs)
        {
            if (!MatchesFilter(tmp))
                continue;
            string fullPath = GetHierarchyPath(tmp.transform);
            string groupPath = _showHierarchyMode ? fullPath : GetGroupPath(fullPath);
            InsertIntoTree(_rootNode, groupPath, tmp);
        }

        // 에셋 내 프리팹 표시 (토글이 켜져 있으면)
        if (_includeAssets)
        {
            HierarchyNode assetNode = new HierarchyNode("에셋 폴더", "Assets");
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null)
                {
                    string shortPath = assetPath;
                    if (shortPath.StartsWith("Assets/"))
                        shortPath = shortPath.Substring("Assets/".Length);
                    foreach (TextMeshProUGUI tmp in prefab.GetComponentsInChildren<TextMeshProUGUI>(true))
                    {
                        if (!MatchesFilter(tmp))
                            continue;
                        InsertIntoTree(assetNode, shortPath, tmp);
                    }
                }
            }
            _rootNode.Children.Add(assetNode);
        }

        // 트리 완성 후, _selectedItems 사전 업데이트 (모든 TMP는 기본적으로 선택되지 않음)
        UpdateSelectionDictionary(_rootNode);

        Debug.Log($"[TMPFontManager] 트리 생성 완료. 모드: {(_showHierarchyMode ? "재귀적 폴드" : "단순 목록")}{(_includeAssets ? " + 에셋 포함" : "")}");
    }

    private void UpdateSelectionDictionary(HierarchyNode node)
    {
        foreach (TextMeshProUGUI tmp in node.TMPItems)
        {
            if (!_selectedItems.ContainsKey(tmp))
                _selectedItems[tmp] = false;
        }
        foreach (var child in node.Children)
        {
            UpdateSelectionDictionary(child);
        }
    }

    private void InsertIntoTree(HierarchyNode node, string fullPath, TextMeshProUGUI tmp)
    {
        string[] segments = fullPath.Split('/');
        InsertSegments(node, segments, 0, tmp);
    }

    private void InsertSegments(HierarchyNode currentNode, string[] segments, int index, TextMeshProUGUI tmp)
    {
        if (index >= segments.Length)
        {
            currentNode.TMPItems.Add(tmp);
            return;
        }
        string segment = segments[index];
        HierarchyNode childNode = currentNode.Children.FirstOrDefault(n => n.Name == segment);
        if (childNode == null)
        {
            string newPath = string.IsNullOrEmpty(currentNode.FullPath) ? segment : currentNode.FullPath + "/" + segment;
            childNode = new HierarchyNode(segment, newPath);
            currentNode.Children.Add(childNode);
        }
        InsertSegments(childNode, segments, index + 1, tmp);
    }

    // 재귀적 폴드 모드에서 노드 표시
    private void DrawNode(HierarchyNode node, int indent)
    {
        if (!NodeMatchesFilter(node))
            return;
        EditorGUI.indentLevel = indent;
        bool fold = true;
        if (!string.IsNullOrEmpty(node.FullPath))
        {
            if (!_foldoutStates.ContainsKey(node.FullPath))
                _foldoutStates[node.FullPath] = true;
            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.normal.textColor = Color.blue;
            fold = EditorGUILayout.Foldout(_foldoutStates[node.FullPath], node.Name, true, foldoutStyle);
            _foldoutStates[node.FullPath] = fold;
        }
        else
        {
            GUILayout.Label("루트", EditorStyles.boldLabel);
        }
        if (fold)
        {
            foreach (TextMeshProUGUI item in node.TMPItems.Where(t => MatchesFilter(t)))
            {
                bool isSelected = _selectedItems.ContainsKey(item) ? _selectedItems[item] : false;
                TMPListItemEditor.DrawListItem(item, isSelected, (bool newVal) => { _selectedItems[item] = newVal; }, ApplySingleFontChange, PingObject, _newFont);
            }
            foreach (var child in node.Children)
            {
                DrawNode(child, indent + 1);
            }
        }
    }

    // 단순 목록(Flat) 모드에서, 빈 그룹은 표시하지 않고, 그룹 헤더와 리스트타일을 표시
    private void DrawFlatGroup(HierarchyNode node)
    {
        // 빈 그룹(직접 TMP가 없는 노드)은 표시하지 않음
        if (node.TMPItems.Count == 0)
        {
            foreach (var child in node.Children.OrderBy(c => c.FullPath))
            {
                DrawFlatGroup(child);
            }
            return;
        }

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField(node.FullPath, EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
        if (GUILayout.Button("경로 복사", GUILayout.Width(100)))
        {
            EditorGUIUtility.systemCopyBuffer = node.FullPath;
        }
        EditorGUILayout.EndHorizontal();

        var sortedItems = node.TMPItems.OrderBy(tmp => tmp.gameObject.name).ToList();
        foreach (var tmp in sortedItems.Where(t => MatchesFilter(t)))
        {
            EditorGUI.indentLevel = 0;
            bool isSelected = _selectedItems.ContainsKey(tmp) ? _selectedItems[tmp] : false;
            TMPListItemEditor.DrawListItem(tmp, isSelected, (bool newVal) => { _selectedItems[tmp] = newVal; }, ApplySingleFontChange, PingObject, _newFont);
        }

        foreach (var child in node.Children.OrderBy(c => c.FullPath))
        {
            DrawFlatGroup(child);
        }
    }

    private bool NodeMatchesFilter(HierarchyNode node)
    {
        bool itemMatch = node.TMPItems.Any(tmp => MatchesFilter(tmp));
        bool childMatch = node.Children.Any(child => NodeMatchesFilter(child));
        return itemMatch || childMatch;
    }

    private bool MatchesFilter(TextMeshProUGUI tmp)
    {
        if (string.IsNullOrEmpty(_searchFilter))
            return true;
        return tmp.gameObject.name.IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
               (tmp.text != null && tmp.text.IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0) ||
               (tmp.font != null && tmp.font.name.IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private string GetHierarchyPath(Transform transform)
    {
        List<string> segments = new List<string>();
        while (transform != null)
        {
            segments.Insert(0, transform.name);
            transform = transform.parent;
        }
        return string.Join("/", segments);
    }

    private string GetGroupPath(string fullPath)
    {
        string[] segments = fullPath.Split('/');
        if (segments.Length <= 1) return fullPath;
        return string.Join("/", segments.Take(segments.Length - 1));
    }

    private void ApplySingleFontChange(TextMeshProUGUI tmp)
    {
        if (_newFont == null) return;
        bool confirm = EditorUtility.DisplayDialog("폰트 변경", $"'{tmp.gameObject.name}'의 폰트를 변경하시겠습니까?", "예", "아니요");
        if (confirm)
        {
            Undo.RecordObject(tmp, "Change TMP Font");
            tmp.font = _newFont;
            EditorUtility.SetDirty(tmp);
        }
    }

    private void PingObject(TextMeshProUGUI tmp)
    {
        EditorGUIUtility.PingObject(tmp.gameObject);
    }
}
