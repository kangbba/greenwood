using System;
using UnityEngine;
using static Character;

public class Emotion : MonoBehaviour
{
    [SerializeField] private KateEmotionType _emotionType; // Inspector에서 직접 설정
    public KateEmotionType EmotionType => _emotionType;

    [SerializeField] private Eyes _eyes;
    [SerializeField] private Mouth _mouth;
    [SerializeField] private Cheek _cheek;

    public void PlayMouth(bool b){
        if(b){
            _mouth.Play().Forget();
        }
        else{
            _mouth.Stop();
        }
    }

    public void PlayEyes(bool b){
        if(b){
            _eyes.Play().Forget();
        }
        else{
            _eyes.Stop();
        }
    }

}
