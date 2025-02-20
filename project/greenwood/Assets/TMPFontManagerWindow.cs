using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class TMPFontManagerWindow : EditorWindow
{
    private Vector2 _scrollPosition;
    private TMP_FontAsset _newFont;

    private HierarchyNode _rootNode;
    private Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();

    // 토글: 하이어라키와 에셋 내 프리팹 표시
    private bool _showHierarchy = true;
    private bool _showAssetPrefabs = false;
    // 이전 토글 값 저장 (자동 갱신용)
    private bool _prevShowHierarchy = true;
    private bool _prevShowAssetPrefabs = false;

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

        _newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("새 폰트 선택", _newFont, typeof(TMP_FontAsset), false);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("✅ 전체 적용")) { ApplyFontToAll(); }
        if (GUILayout.Button("🔍 다시 스캔")) { ScanAndBuildTree(); }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        // 토글: 하이어라키 표시, 에셋 폴더 표시
        EditorGUILayout.BeginHorizontal();
        _showHierarchy = EditorGUILayout.Toggle("하이어라키 표시", _showHierarchy);
        _showAssetPrefabs = EditorGUILayout.Toggle("에셋 내 프리팹 표시", _showAssetPrefabs);
        EditorGUILayout.EndHorizontal();

        // 토글 값이 변경되면 한 프레임 딜레이 후 갱신
        if (_prevShowHierarchy != _showHierarchy || _prevShowAssetPrefabs != _showAssetPrefabs)
        {
            _prevShowHierarchy = _showHierarchy;
            _prevShowAssetPrefabs = _showAssetPrefabs;
            EditorApplication.delayCall += () =>
            {
                ScanAndBuildTree();
                Repaint();
            };
        }

        GUILayout.Space(10);
        // 스크롤뷰: 창 너비에 맞춰 확장, 가로 스크롤 없음
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        if (_rootNode != null)
        {
            DrawNode(_rootNode, 0);
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
            Undo.RecordObject(tmp, "Change TMP Font");
            tmp.font = _newFont;
            EditorUtility.SetDirty(tmp);
        }
        foreach (var child in node.Children)
        {
            ApplyFontRecursive(child);
        }
    }

    private void ScanAndBuildTree()
    {
        _rootNode = new HierarchyNode("루트", "");
        _foldoutStates.Clear();

        if (_showHierarchy)
        {
            List<TextMeshProUGUI> allTMPs = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None).ToList();
            foreach (var tmp in allTMPs)
            {
                string fullPath = GetHierarchyPath(tmp.transform);
                // 그룹화: 마지막 세그먼트를 제거하여 공통 경로로 묶음
                string groupPath = GetGroupPath(fullPath);
                InsertIntoTree(_rootNode, groupPath, tmp);
            }
        }
        if (_showAssetPrefabs)
        {
            HierarchyNode assetNode = new HierarchyNode("에셋 폴더", "Assets");
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null)
                {
                    foreach (TextMeshProUGUI tmp in prefab.GetComponentsInChildren<TextMeshProUGUI>(true))
                    {
                        InsertIntoTree(assetNode, assetPath, tmp);
                    }
                }
            }
            _rootNode.Children.Add(assetNode);
        }

        Debug.Log($"[TMPFontManager] 트리 생성 완료. 모드: {(_showHierarchy ? "하이어라키" : "")}{(_showAssetPrefabs ? " + 에셋" : "")}");
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

    private void DrawNode(HierarchyNode node, int indent)
    {
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
            // 먼저 부모 노드에 속한 TMP 항목들을 표시 (예: 텍스트 A)
            foreach (TextMeshProUGUI item in node.TMPItems)
            {
                TMPListItemEditor.DrawListItem(item, ApplySingleFontChange, PingObject, _newFont);
            }
            // 그 다음 자식 노드들을 재귀적으로 표시 (예: 폴더 2 등)
            foreach (var child in node.Children)
            {
                DrawNode(child, indent + 1);
            }
        }

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
