using UnityEngine;

namespace CafeTime.Core
{
    // 고용할 수 있는 알바생 한 명 (ADR-0057). 하루 단위로 고용하고, 일당은 그날 정산 때 빠진다
    [System.Serializable]
    public class StaffDefinition
    {
        [Tooltip("저장 파일과 그림 파일 이름에 쓰는 id (Art/Sprites/Staff/Staff{id}Down.png). 바꾸지 않는다")]
        [SerializeField] string id;
        [SerializeField] string displayName;
        [SerializeField] StaffTask tasks;
        [Tooltip("업무 설명 (카드의 '맡는 일')")]
        [SerializeField] string taskText;
        [SerializeField] int wage;
        [Tooltip("이 DAY 부터 고용할 수 있다")]
        [SerializeField] int unlockDay = 1;

        public StaffDefinition(string id, string displayName, StaffTask tasks, string taskText, int wage, int unlockDay)
        {
            this.id = id;
            this.displayName = displayName;
            this.tasks = tasks;
            this.taskText = taskText;
            this.wage = wage;
            this.unlockDay = unlockDay;
        }

        public string Id => id;
        public string DisplayName => displayName;
        public StaffTask Tasks => tasks;
        public string TaskText => taskText;
        public int Wage => wage;
        public int UnlockDay => unlockDay;

        public bool Does(StaffTask task) => (tasks & task) != 0;
    }
}
