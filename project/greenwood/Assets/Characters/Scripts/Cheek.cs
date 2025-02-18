using UnityEngine;
using Cysharp.Threading.Tasks;

public class Cheek : MonoBehaviour
{
    [SerializeField] private GameObject _cheekObj;

    /// <summary>
    /// 뺨(볼터치 등)을 활성화
    /// </summary>
    public async UniTaskVoid Play()
    {
        _cheekObj.SetActive(true);
        await UniTask.Delay(2000);
        // 예시로 일정 시간 후 자동으로 꺼지고 싶다면 이렇게 (선택 사항)
        // Stop();
    }

    /// <summary>
    /// 뺨(볼터치 등) 비활성화
    /// </summary>
    public void Stop()
    {
        _cheekObj.SetActive(false);
    }
}
