using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static BigPlaceNames;

public class BigPlace : AnimationImage
{
    [Header("BigPlace Settings")]
    [SerializeField] private Image _background;
    [SerializeField] private EBigPlaceName _bigPlaceName;
    private List<SmallPlaceDoor> _smallPlaceDoors = new List<SmallPlaceDoor>();

    public EBigPlaceName BigPlaceName => _bigPlaceName;
    public List<SmallPlaceDoor> SmallPlaceDoors => _smallPlaceDoors;

    private void Awake()
    {
        _smallPlaceDoors = GetComponentsInChildren<SmallPlaceDoor>().ToList();
    }

    public void Init(){
        FadeOut(0f);
    }
}
