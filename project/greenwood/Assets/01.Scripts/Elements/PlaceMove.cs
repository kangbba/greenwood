using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlaceMove : Element
{
    private string _placeID;
    private float _duration;

    public PlaceMove(string placeID, float duration = 1f)
    {
        _placeID = placeID;
        _duration = duration;
    }

    public override async UniTask ExecuteAsync()
    {
        Debug.Log($"[PlaceEnter] Entering place: {_placeID} with duration: {_duration}");
        await PlaceService.MovePlace(_placeID, _duration);
    }

    public override void ExecuteInstantly()
    {
        Debug.Log($"[PlaceEnter] Instantly entering place: {_placeID}");
        PlaceManager.Instance.DestroyCurrentPlace(0);
        PlaceManager.Instance.CreatePlace(_placeID)?.Show(0);
    }
}
