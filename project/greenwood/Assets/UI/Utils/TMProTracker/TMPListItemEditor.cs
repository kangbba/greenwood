using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public static class TMPListItemEditor
{
    /// <summary>
    /// 카드 형태의 리스트타일 UI를 표시합니다.
    /// 체크박스는 리스트타일의 왼쪽 바깥에 표시됩니다.
    /// </summary>
    /// <param name="isSelected">체크박스 상태</param>
    /// <param name="onToggle">체크박스 상태 변경 콜백</param>
    public static void DrawListItem(TextMeshProUGUI tmp, bool isSelected, Action<bool> onToggle, Action<TextMeshProUGUI> onApplyFont, Action<TextMeshProUGUI> onPing, TMP_FontAsset newFont)
    {
        Color originalColor = GUI.backgroundColor;
        if (isSelected)
        {
            GUI.backgroundColor = Color.green;
        }

        // 체크박스를 별도 수평 그룹으로, 리스트타일 카드 외부에 위치시킵니다.
        EditorGUILayout.BeginHorizontal();
        bool newSelection = EditorGUILayout.Toggle(isSelected, GUILayout.Width(20));
        if (newSelection != isSelected)
        {
            onToggle(newSelection);
            
        }
        EditorGUILayout.EndHorizontal();

        // 리스트타일 카드 (도움말 상자 스타일)
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Space(5);
        EditorGUILayout.LabelField($"🎨 {tmp.gameObject.name} (TextMeshProUGUI)", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("📄 텍스트: " + tmp.text, EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("🔠 폰트: " + (tmp.font != null ? tmp.font.name : "없음"));
        EditorGUILayout.LabelField("🖌️ 머티리얼: " + (tmp.fontSharedMaterial != null ? tmp.fontSharedMaterial.name : "없음"));
        EditorGUILayout.LabelField("🔠 폰트 스타일: " + tmp.fontStyle);
        EditorGUILayout.LabelField("📏 정렬: " + tmp.alignment);
        EditorGUILayout.LabelField("🟩 아웃라인: " + (tmp.outlineWidth > 0 ? "사용" : "미사용"));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("🔄 개별 적용", GUILayout.Width(120), GUILayout.Height(30)))
        {
            onApplyFont?.Invoke(tmp);
        }
        if (GUILayout.Button("📍 핑", GUILayout.Width(120), GUILayout.Height(30)))
        {
            onPing?.Invoke(tmp);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        GUILayout.Space(20);

        GUI.backgroundColor = originalColor;
    }
}
