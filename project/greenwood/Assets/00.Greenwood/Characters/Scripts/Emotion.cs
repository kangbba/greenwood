using UnityEngine;
using Sirenix.OdinInspector;

public class Emotion : AnimationImage
{
    [SerializeField] private string _emotionID; // ✅ Inspector에서 직접 설정 가능

    [SerializeField] private Eyes _eyes;
    [SerializeField] private Mouth _mouth;
    [SerializeField] private Cheek _cheek;

    public string EmotionID { get => _emotionID; }

    public void Init()
    {
        PlayMouth(false);
        PlayEyes(true);

        if (_cheek != null)
        {
            _cheek.SetFlush(false, 0f);
            _cheek.SetFlush(true, 2f);
        }
    }

    public void PlayMouth(bool isActive)
    {
        if (isActive)
        {
            _mouth.Stop();
            _mouth.Play().Forget();
        }
        else
        {
            _mouth.Stop();
        }
    }

    private void PlayEyes(bool isActive)
    {
        if (isActive)
        {
            _eyes.Stop();
            _eyes.Play().Forget();
        }
        else
        {
            _eyes.Stop();
        }
    }
#if UNITY_EDITOR

    [Button("➡ Random Eyes And Mouth")]
    public void RandomEyesAndMouth()
    {
        if(Random.Range(0,1f) < .3f){
        _eyes.PreviewBlink();
        }
        if(Random.Range(0,1f) < .7f){
        _mouth.PreviewMouth();
        }
    }
#endif
}
