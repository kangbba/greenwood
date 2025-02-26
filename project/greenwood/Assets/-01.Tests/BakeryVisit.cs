using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class BakeryVisit : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Smile, KatePoseType.HandsFront, CharacterLocation.Center, 1f),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "맛있는게 많으니 잘 둘러보라구!",
        }),
    };
}
