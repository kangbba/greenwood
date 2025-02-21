using UnityEngine;
using Cysharp.Threading.Tasks;

public class SmallPlace : AnimationImage
{
    [Header("SmallPlace Settings")]
    [SerializeField] private ESmallPlaceName smallPlaceName;

    public ESmallPlaceName SmallPlaceName => smallPlaceName;


    public void Init(){
        FadeOut(0f);
    }
}
