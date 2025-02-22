

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;
namespace Jin.TMProTracker
{
    public class TMProTracker : EditorWindow
    {
        private Vector2 _scrollPosition;
        private TMP_FontAsset _newFont;
        
        // 두 개의 검색 필터
        private string _nameFilter = "";
        private string _fontFilter = "";

        private HierarchyNode _rootNode;
        private Dictionary<string, bool> _foldoutStates = new Dictionary<string, bool>();

        // 토글: 에셋 포함 여부 (하이어라키는 항상 표시)
        private bool _includeAssets = true;  

        // 단순 목록 모드 토글: true이면 재귀적 폴드아웃, false이면 단순 목록(Flat) 모드
        private bool _showHierarchyMode = false; 

        // 이전 토글 값 (자동 갱신용)
        private bool _prevIncludeAssets = false;
        private bool _prevShowHierarchyMode = true;

        // 각 TMP 오브젝트의 선택 상태 (체크박스)
        private Dictionary<TextMeshProUGUI, bool> _selectedItems = new Dictionary<TextMeshProUGUI, bool>();

        [MenuItem("Jin/TMProTracker")]
        public static void ShowWindow()
        {
            TMProTracker window = GetWindow<TMProTracker>("TextMeshPro 폰트 관리자");
            window.minSize = new Vector2(800, 600);
            window.ScanAndBuildTree();
        }

        private void OnGUI()
        {
            GUILayout.Label("TextMeshPro 폰트 관리자", EditorStyles.boldLabel);
            GUILayout.Space(5);

            // 두 검색 필터 입력 (세로 배치)
            _nameFilter = EditorGUILayout.TextField("검색필터 (게임오브젝트 이름)", _nameFilter);
            _fontFilter = EditorGUILayout.TextField("검색필터 (폰트)", _fontFilter);

            _newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("새 폰트 선택", _newFont, typeof(TMP_FontAsset), false);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("✅ 전체 적용", GUILayout.Height(40))) { ApplyFontToAll(); }
            if (GUILayout.Button("✅ 선택 적용", GUILayout.Height(40))) { ApplySelectedFont(); }
            if (GUILayout.Button("🔍 다시 불러오기", GUILayout.Height(40))) { ScanAndBuildTree(); }
            EditorGUILayout.EndHorizontal();

            // 상단에 선택된 항목 수 표시
            int selectedCount = _selectedItems.Values.Count(v => v);
            GUILayout.Label($"선택된 항목: {selectedCount}개");

            GUILayout.Space(10);
            // 토글: 에셋 포함 여부
            _includeAssets = EditorGUILayout.Toggle("에셋 내 프리팹 포함", _includeAssets);
            // 토글: 재귀적 폴드 사용 여부
            _showHierarchyMode = EditorGUILayout.Toggle("재귀적 표시", _showHierarchyMode);

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
                "모든!!!!!!!!!!!!!!! TextMeshProUGUI의 폰트를 새 폰트로 변경하시겠습니까?",
                "예", "아니요");
            if (!confirm) return;

            Undo.RecordObject(this, "Change All TMP Fonts");
            int count = 0;
            ApplyFontRecursive(_rootNode, ref count);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("성공", $"{count}개의 텍스트에 폰트 변경이 적용되었습니다.", "확인");
            Debug.Log("[TMPFontManager] 모든 TMP 폰트 변경 완료");
        }

        private void ApplyFontRecursive(HierarchyNode node, ref int count)
        {
            foreach (TextMeshProUGUI tmp in node.TMPItems)
            {
                if (!MatchesFilter(tmp)) continue;
                ApplyFontToTMP(tmp);
                count++;
            }
            foreach (var child in node.Children)
            {
                ApplyFontRecursive(child, ref count);
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
                ApplyFontToTMP(tmp);
            }
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("성공", $"선택된 {selectedTMPs.Count}개의 텍스트에 폰트 변경이 적용되었습니다.", "확인");
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

            UpdateSelectionDictionary(_rootNode);

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

        // 단순 목록(Flat) 모드에서 그룹 헤더와 리스트타일 표시 (빈 그룹은 표시하지 않음)
        private void DrawFlatGroup(HierarchyNode node)
        {
            if (node.TMPItems.Count == 0)
            {
                foreach (var child in node.Children.OrderBy(c => c.FullPath))
                {
                    DrawFlatGroup(child);
                }
                return;
            }

            // 그룹 헤더 스타일 적용 (배경색 변경)
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { background = MakeTex(2, 2, new Color(0.15f, 0.15f, 0.15f, 1f)) }, // 어두운 회색 배경
                padding = new RectOffset(10, 10, 0, 0),
                fontSize = 15
            };

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            EditorGUILayout.LabelField(node.FullPath, headerStyle, GUILayout.ExpandWidth(true));
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

        // 배경색을 적용할 Texture 생성 함수
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }


        private bool NodeMatchesFilter(HierarchyNode node)
        {
            bool itemMatch = node.TMPItems.Any(tmp => MatchesFilter(tmp));
            bool childMatch = node.Children.Any(child => NodeMatchesFilter(child));
            return itemMatch || childMatch;
        }

        private bool MatchesFilter(TextMeshProUGUI tmp)
        {
            bool nameMatch = string.IsNullOrEmpty(_nameFilter) || tmp.gameObject.name.IndexOf(_nameFilter, StringComparison.OrdinalIgnoreCase) >= 0;
            bool fontMatch = string.IsNullOrEmpty(_fontFilter) || (tmp.font != null && tmp.font.name.IndexOf(_fontFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            return nameMatch && fontMatch;
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
                ApplyFontToTMP(tmp);
            }
        }
        private void ApplyFontToTMP(TextMeshProUGUI tmp)
        {
            if (_newFont == null)
            {
                EditorUtility.DisplayDialog("오류", "새 폰트가 지정되어 있지 않습니다.", "확인");
                return;
            }
            
            // TMP의 폰트 변경 및 변경사항 기록
            Undo.RecordObject(tmp, "Change TMP Font");
            tmp.font = _newFont;
            EditorUtility.SetDirty(tmp);

            // 프리팹 수정 처리
            if (PrefabUtility.IsPartOfPrefabInstance(tmp.gameObject))
            {
                // 프리팹 인스턴스인 경우 최상위 인스턴스 루트를 가져와서 적용
                GameObject prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(tmp.gameObject);
                if (prefabRoot != null)
                {
                    PrefabUtility.ApplyPrefabInstance(prefabRoot, InteractionMode.UserAction);
                }
            }
            else if (PrefabUtility.IsPartOfPrefabAsset(tmp.gameObject))
            {
                // 프리팹 에셋인 경우, 에셋 경로를 통해 프리팹 에셋을 로드한 뒤 저장
                string assetPath = AssetDatabase.GetAssetPath(tmp.gameObject);
                GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefabAsset != null)
                {
                    PrefabUtility.SavePrefabAsset(prefabAsset);
                }
            }
            Debug.Log($"성공 :: '{tmp.gameObject.name}'의 폰트 변경이 성공적으로 적용되었습니다.");
        }

        private void PingObject(TextMeshProUGUI tmp)
        {
            EditorGUIUtility.PingObject(tmp.gameObject);
        }
    }
}
#endif
