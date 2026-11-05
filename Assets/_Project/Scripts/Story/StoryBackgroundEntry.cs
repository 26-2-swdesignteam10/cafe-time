
using System;
using UnityEngine;

namespace CafeTime.Story
{
    // SFR-007: 스토리 장면별 배경 이미지 데이터
    [Serializable]
    public class StoryBackgroundEntry
    {
        [SerializeField] private string id;
        [SerializeField] private Sprite sprite;

        public string Id => id;
        public Sprite Sprite => sprite;
    }
}
