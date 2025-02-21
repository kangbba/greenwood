using UnityEngine;
using UnityEngine.UI;

public static class ImageUtils
{
    /// <summary>
    /// ✅ Image에서 AnimationImage 가져오거나 추가 (Image가 존재하지 않을 경우 예외 처리)
    /// </summary>
    public static AnimationImage GetAnim(this Image objImg)
    {
        if (objImg == null)
        {
            Debug.LogError("[ImageUtils] GetAnim failed: Image component is null!");
            return null;
        }

        return GetAnim(objImg.gameObject);
    }

    /// <summary>
    /// ✅ GameObject에서 AnimationImage 가져오거나 추가 (Image 여부 확인 후 동작)
    /// </summary>
    public static AnimationImage GetAnim(this GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("[ImageUtils] GetAnim failed: GameObject is null!");
            return null;
        }

        var image = obj.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError($"[ImageUtils] GetAnim failed: GameObject '{obj.name}' does not have an Image component!");
            return null;
        }

        var animationImage = obj.GetComponent<AnimationImage>();
        if (animationImage == null)
        {
            animationImage = obj.AddComponent<AnimationImage>();
        }
        return animationImage;
    }
}
