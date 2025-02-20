using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public static class TMPListItemEditor
{
    /// <summary>
    /// 카드 형태의 리스트타일 UI를 표시합니다.
    /// </summary>
    public static void DrawListItem(TextMeshProUGUI tmp, Action<TextMeshProUGUI> onApplyFont, Action<TextMeshProUGUI> onPing, TMP_FontAsset newFont)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Space(5);
        EditorGUILayout.LabelField($"🎨 {tmp.gameObject.name} (TextMeshProUGUI)", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("📄 텍스트: " + tmp.text, EditorStyles.wordWrappedLabel);
        EditorGUILayout.LabelField("🔠 폰트: " + (tmp.font != null ? tmp.font.name : "없음"));
        EditorGUILayout.LabelField("🖌️ 머티리얼: " + (tmp.fontMaterial != null ? tmp.fontMaterial.name : "없음"));
        EditorGUILayout.LabelField("📏 정렬: " + tmp.alignment);
        EditorGUILayout.LabelField("🟩 아웃라인: " + (tmp.outlineWidth > 0 ? "사용" : "미사용"));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("🔄 개별 적용", GUILayout.Width(120)))
        {
            onApplyFont?.Invoke(tmp);
        }
        if (GUILayout.Button("📍 핑", GUILayout.Width(80)))
        {
            onPing?.Invoke(tmp);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}
