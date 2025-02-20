using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SmallPlaceLocation : MonoBehaviour
{
    [Tooltip("이 버튼이 이동할 SmallPlace")]
    [SerializeField, OnValueChanged("UpdateGameObjectName")] 
    private SmallPlace _smallPlacePrefab;

    public ESmallPlaceName SmallPlaceName => _smallPlacePrefab != null ? _smallPlacePrefab.SmallPlaceName : default;

    public SmallPlace SmallPlacePrefab => _smallPlacePrefab;

    /// <summary>
    /// Odin Inspector의 OnValueChanged 사용하여 GameObject 이름 자동 변경
    /// </summary>
    private void UpdateGameObjectName()
    {
        if (_smallPlacePrefab != null)
        {
            gameObject.name = $"{_smallPlacePrefab.SmallPlaceName}_{_smallPlacePrefab.gameObject.name}";
        }
        else
        {
            gameObject.name = "Unassigned_SmallPlace";
        }
    }

    /// <summary>
    /// Odin Inspector에서 수동으로 리프레시 버튼을 통해 이름 업데이트 가능
    /// </summary>
    [Button("Refresh Name")]
    private void RefreshName()
    {
        UpdateGameObjectName();
    }
}
