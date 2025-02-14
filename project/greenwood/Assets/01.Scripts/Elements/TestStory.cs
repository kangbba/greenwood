using System.Collections.Generic;
using UnityEngine;

public class TestStory : Story
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new Dialogue(CharacterName.Kate, 
            new List<string>
            {
                "야, 큰일 났어! 마을에서 이상한 일이 벌어졌어!",
                "오늘 광장에서 이상한 사람들이 모였거든.",
                "뭔가 느낌이 쎄해. 너도 알아둬야 할 것 같아."
            }
        ),

        new ChoiceSet(
            "어떻게 반응할까?",
            new List<ChoiceContent>
            {
                new ChoiceContent("무슨 일이었는데?", new List<Element>
                {
                    new Dialogue(CharacterName.Kate, 
                        new List<string>
                        {
                            "응! 그러니까 말이야, 광장에서...",
                            "이상한 검은 망토를 쓴 사람들이 뭔가를 속삭이고 있었어.",
                            "마을 사람들이 다 피하는 것 같았고, 뭔가 불길했어."
                        }
                    ),
                    new Dialogue(CharacterName.Joseph, 
                        new List<string>
                        {
                            "흠... 마을에 그런 일이?",
                            "그건 쉽게 넘길 문제가 아닐 수도 있겠군."
                        }
                    )
                }),
                
                new ChoiceContent("그냥 소문 아니야?", new List<Element>
                {
                    new Dialogue(CharacterName.Kate, 
                        new List<string>
                        {
                            "소문이라고 하기엔 분위기가 이상했어!",
                            "게다가 마을 사람들도 다들 눈을 피하는 것 같았다고."
                        }
                    ),
                    new Dialogue(CharacterName.Lisa, 
                        new List<string>
                        {
                            "흥, 괜한 걱정 아냐?",
                            "하지만... 네가 신경 쓰는 건 이유가 있겠지."
                        }
                    )
                })
            }
        ),

        new Dialogue(CharacterName.Mono, 
            new List<string>
            {
                "조용하다... 이상할 정도로.",
                "마을 사람들이 평소보다 말수가 줄었고, 분위기도 무겁다.",
                "내가 여기 온 이유는... 뭔가 놓치고 있는 게 있을지도 모르겠어."
            }
        ),

        new ChoiceSet(
            "이 분위기를 어떻게 받아들일까?",
            new List<ChoiceContent>
            {
                new ChoiceContent("뭔가 잘못된 것 같아.", new List<Element>
                {
                    new Dialogue(CharacterName.Ryan, 
                        new List<string>
                        {
                            "그래, 나도 같은 생각이야.",
                            "이 마을에 뭔가 이상한 일이 벌어지고 있어."
                        }
                    ),
                    new Dialogue(CharacterName.Eldra, 
                        new List<string>
                        {
                            "조심해라. 직감은 무시할 게 아니지.",
                            "특히 이런 상황에서는 말이다."
                        }
                    )
                }),
                
                new ChoiceContent("괜한 걱정이겠지.", new List<Element>
                {
                    new Dialogue(CharacterName.Mono, 
                        new List<string>
                        {
                            "그러길 바란다. 하지만 마음 한편이 찝찝하군.",
                            "뭐, 당장은 지켜보는 게 최선이겠지."
                        }
                    ),
                    new Dialogue(CharacterName.Lisa, 
                        new List<string>
                        {
                            "넌 걱정이 너무 많아.",
                            "하지만... 그래, 그럴 수도 있겠지."
                        }
                    )
                }),

                new ChoiceContent("여기서 벗어나자.", new List<Element>
                {
                    new Dialogue(CharacterName.Kate, 
                        new List<string>
                        {
                            "좋아! 더 이상 신경 쓰지 말자!",
                            "우리가 신경 쓴다고 해결되는 것도 아니잖아?"
                        }
                    ),
                    new Dialogue(CharacterName.Joseph, 
                        new List<string>
                        {
                            "하하, 젊은 혈기군.",
                            "하지만... 가끔은 단순한 해결책이 정답일 수도 있지."
                        }
                    )
                })
            }
        ),

        new Dialogue(CharacterName.Lisa, 
            new List<string>
            {
                "이 상황이 별로 마음에 안 들어.",
                "뭔가 이상한 기운이 감돌아...",
                "됐어. 신경 쓰지 않는 게 나을지도."
            }
        )
    };
}
