
using System;
using UnityEngine;

namespace CafeTime.Story
{
    // SFR-008: 화자별 캐릭터 초상화 데이터
    [Serializable]
    public class StoryPortraitEntry
    {
        [SerializeField] private string speaker;
        [SerializeField] private Sprite sprite;

        public string Speaker => speaker;
        public Sprite Sprite => sprite;
    }
}
