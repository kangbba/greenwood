using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SmallPlaceDoor : MonoBehaviour
{
    [OnValueChanged("OVC_smallPlaceName")]
    [SerializeField] private ESmallPlaceName _smallPlaceName;

    public ESmallPlaceName SmallPlaceName { get => _smallPlaceName; }


    /// <summary>
    /// Odin Inspector의 OnValueChanged 사용하여 GameObject 이름 자동 변경
    /// </summary>
    private void OVC_smallPlaceName()
    {
        gameObject.name = $"Door of {_smallPlaceName}";
    }
}
