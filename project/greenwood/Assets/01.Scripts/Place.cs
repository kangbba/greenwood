using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening; // DoTween 사용

public class Place : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = gameObject.AddComponent<Image>();
        _image.color = new Color(1, 1, 1, 0); // 처음엔 투명
    }

    public void Init(Sprite sprite)
    {
        _image.sprite = sprite;
        _image.SetNativeSize();
    }

    public void Show(float duration)
    {
        Debug.Log($"[Place] Showing place over {duration}s");
        _image.DOFade(1f, duration).SetEase(Ease.Linear);
    }

    public void Hide(float duration)
    {
        Debug.Log($"[Place] Hiding place over {duration}s");
        _image.DOFade(0f, duration).SetEase(Ease.Linear);
    }
}
