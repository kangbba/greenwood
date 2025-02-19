using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlaceButtonMapping
{
    [Tooltip("이 버튼이 이동할 Place의 이름")]
    public EPlaceName placeName;

    [Tooltip("클릭하면 장소를 이동할 버튼")]
    public Button button;
}
