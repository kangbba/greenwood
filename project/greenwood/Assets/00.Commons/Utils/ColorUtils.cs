using UnityEngine;

public static class ColorUtils
{   
    public static Color pm10 = Color.Lerp(Color.black, Color.blue, .5f);
    public static Color pm4 = CustomColor("F8C8AC");
    public static Color pm8 = Color.Lerp(pm6, pm10, .5f);
    public static Color pm6 = ColorUtils.CustomColor("5466A9");
    public static Color ModifiedAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    // 새 메서드: 16진수 색상 코드를 Color로 변환
    public static Color CustomColor(string hex)
    {
        // 만약 #이 없으면 자동으로 추가
        if (!hex.StartsWith("#"))
        {
            hex = "#" + hex;
        }

        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        else
        {
            Debug.LogError($"Invalid color code: {hex}");
            return Color.red; // 기본값으로 흰색 반환
        }
    }
}
