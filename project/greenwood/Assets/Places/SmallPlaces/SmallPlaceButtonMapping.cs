using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SmallPlaceButtonMapping
{
    [Tooltip("이 버튼이 이동할 SmallPlace의 이름")]
    public ESmallPlaceName smallPlaceName;

    [Tooltip("클릭하면 장소를 이동할 버튼")]
    public Button button;
}
