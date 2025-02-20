using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class TMPFontManagerWindow : EditorWindow
{
    private Vector2 _scrollPosition;
    private List<TextMeshProUGUI> _allTextComponents = new List<TextMeshProUGUI>();
    private Dictionary<TextMeshProUGUI, bool> _selectedTexts = new Dictionary<TextMeshProUGUI, bool>();
    private TMP_FontAsset _newFont;
    private Dictionary<string, bool> _foldoutState = new Dictionary<string, bool>(); // ✅ 계층별 Foldout 상태 저장

    private const float IndentOffset = 20f; // ✅ 계층당 X축 오프셋 (밀려나게 함)

    [MenuItem("Tools/TextMeshPro Font Manager")]
    public static void ShowWindow()
    {
        TMPFontManagerWindow window = GetWindow<TMPFontManagerWindow>("TMP Font Manager");
        window.minSize = new Vector2(800, 600);
        window.ScanTextComponents();
    }

    private void OnGUI()
    {
        GUILayout.Label("TextMeshPro Font Manager", EditorStyles.boldLabel);
        GUILayout.Label($"Currently displaying {_allTextComponents.Count} TextMeshProUGUI components", EditorStyles.miniBoldLabel);
        GUILayout.Space(5);

        if (GUILayout.Button("🔍 Scan TMP Components"))
        {
            ScanTextComponents();
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Select New TMP_FontAsset", EditorStyles.boldLabel);
        _newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font", _newFont, typeof(TMP_FontAsset), false);

        GUILayout.Space(10);
        if (GUILayout.Button("✅ Apply Font to Selected"))
        {
            ApplyFontChange();
        }

        GUILayout.Space(10);
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        DrawHierarchyUI();

        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// ✅ 씬 내 모든 TextMeshProUGUI 검색
    /// </summary>
    private void ScanTextComponents()
    {
        _allTextComponents = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None).ToList();
        _selectedTexts.Clear();
        _foldoutState.Clear();

        foreach (var tmp in _allTextComponents)
        {
            _selectedTexts[tmp] = false;
            string rootPath = GetHierarchyPath(tmp.gameObject);
            if (!_foldoutState.ContainsKey(rootPath)) _foldoutState[rootPath] = true;
        }

        Debug.Log($"[TMPFontManager] Found {_allTextComponents.Count} TextMeshProUGUI components.");
    }

    /// <summary>
    /// ✅ 선택된 TMP 컴포넌트에 폰트 적용
    /// </summary>
    private void ApplyFontChange()
    {
        if (_newFont == null)
        {
            Debug.LogWarning("[TMPFontManager] No font selected!");
            return;
        }

        int changedCount = 0;
        foreach (var tmp in _allTextComponents)
        {
            if (_selectedTexts.ContainsKey(tmp) && _selectedTexts[tmp])
            {
                Undo.RecordObject(tmp, "Change TMP Font");
                tmp.font = _newFont;
                EditorUtility.SetDirty(tmp);
                changedCount++;
            }
        }

        Debug.Log($"[TMPFontManager] Applied new font to {changedCount} objects.");
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// ✅ TMP 리스트 아이템 표시 (계층 구조 지원)
    /// </summary>
    private void DrawHierarchyUI()
    {
        Dictionary<string, List<TextMeshProUGUI>> groupedByParent = new Dictionary<string, List<TextMeshProUGUI>>();

        foreach (var tmp in _allTextComponents)
        {
            string parentPath = GetClosestParentPath(tmp.gameObject);
            if (!groupedByParent.ContainsKey(parentPath))
                groupedByParent[parentPath] = new List<TextMeshProUGUI>();

            groupedByParent[parentPath].Add(tmp);
        }

        foreach (var kvp in groupedByParent)
        {
            DrawHierarchyFoldout(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    /// ✅ 부모별 `Foldout`을 지원하는 계층 구조 UI (X축 정렬 추가)
    /// </summary>
    private void DrawHierarchyFoldout(string parentPath, List<TextMeshProUGUI> components)
    {
        if (!_foldoutState.ContainsKey(parentPath))
            _foldoutState[parentPath] = true;

        EditorGUI.indentLevel = parentPath.Split('/').Length - 1;
        _foldoutState[parentPath] = EditorGUILayout.Foldout(_foldoutState[parentPath], parentPath, true);

        if (_foldoutState[parentPath])
        {
            foreach (var tmpComponent in components)
            {
                DrawListItem(tmpComponent);
            }
        }
    }

    /// <summary>
    /// ✅ 개별적인 TMP 리스트 아이템 표시 (그룹 내부)
    /// </summary>
    private void DrawListItem(TextMeshProUGUI tmpComponent)
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        _selectedTexts[tmpComponent] = EditorGUILayout.Toggle(_selectedTexts[tmpComponent], GUILayout.Width(20));

        Texture icon = tmpComponent.gameObject.scene.rootCount == 0 ? EditorGUIUtility.IconContent("Prefab Icon").image : EditorGUIUtility.IconContent("GameObject Icon").image;
        GUILayout.Label(icon, GUILayout.Width(20), GUILayout.Height(20));

        GUILayout.Label(tmpComponent.gameObject.name, EditorStyles.boldLabel, GUILayout.Width(150));
        GUILayout.Label("(TextMeshProUGUI)", EditorStyles.miniLabel, GUILayout.Width(120));

        EditorGUILayout.LabelField(tmpComponent.text, GUILayout.Width(200));

        tmpComponent.font = (TMP_FontAsset)EditorGUILayout.ObjectField(tmpComponent.font, typeof(TMP_FontAsset), false, GUILayout.Width(150));

        if (GUILayout.Button("🔄 Apply", GUILayout.Width(70)))
        {
            ApplySingleFontChange(tmpComponent);
        }

        if (GUILayout.Button("📍 Ping", GUILayout.Width(60)))
        {
            EditorGUIUtility.PingObject(tmpComponent.gameObject);
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// ✅ 개별적으로 TMP 폰트 변경 (경고 메시지 포함)
    /// </summary>
    private void ApplySingleFontChange(TextMeshProUGUI tmpComponent)
    {
        if (_newFont == null)
        {
            Debug.LogWarning("[TMPFontManager] No font selected!");
            return;
        }

        bool confirm = EditorUtility.DisplayDialog(
            "Change Font",
            $"Are you sure you want to change the font for {tmpComponent.gameObject.name}?",
            "Yes", "No");

        if (confirm)
        {
            Undo.RecordObject(tmpComponent, "Change TMP Font");
            tmpComponent.font = _newFont;
            EditorUtility.SetDirty(tmpComponent);
            Debug.Log($"[TMPFontManager] Changed font for {tmpComponent.gameObject.name}.");
        }
    }

    /// <summary>
    /// ✅ 가장 가까운 부모 오브젝트의 경로 반환
    /// </summary>
    private string GetClosestParentPath(GameObject obj)
    {
        Transform parent = obj.transform.parent;
        return parent != null ? GetHierarchyPath(parent.gameObject) : obj.name;
    }

    /// <summary>
    /// ✅ 오브젝트의 전체 `Hierarchy Path`를 반환
    /// </summary>
    private string GetHierarchyPath(GameObject obj)
    {
        List<string> path = new List<string>();
        Transform current = obj.transform;

        while (current != null)
        {
            path.Insert(0, current.name);
            current = current.parent;
        }

        return string.Join("/", path);
    }
}
