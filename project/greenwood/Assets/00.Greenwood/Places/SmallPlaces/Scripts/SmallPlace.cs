using UnityEngine;
using Cysharp.Threading.Tasks;
using static SmallPlaceNames;

public class SmallPlace : AnimationImage
{
    [Header("SmallPlace Settings")]
    [SerializeField] private ESmallPlaceName smallPlaceName;
    private Scenario _scenarioToPlay;
    public ESmallPlaceName SmallPlaceName => smallPlaceName;

    public Scenario ScenarioToPlay { get => _scenarioToPlay; }

    public void Init(){
        FadeOut(0f);
    }   
    
    public void SetScenario(Scenario scenarioToPlay)
    {
        _scenarioToPlay = scenarioToPlay;
    }

    public void ReadyForScenarioStart(){

        _scenarioToPlay.ReadyForScenarioStart();
    }

    public async UniTask PlayScenario()
    {
        await _scenarioToPlay.ExecuteAsync();
    }

}
