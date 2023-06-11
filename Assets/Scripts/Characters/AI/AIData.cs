using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters.AI
{
    [CreateAssetMenu(menuName = "BlobBoxer/AI/AISettings", fileName = "AISettings")]
    public class AIData : ScriptableObject
    {
        [FoldoutGroup("Basic Stats")] 
        [SerializeField] private float blockChancePercentage;
        public float BlockChancePercentage => blockChancePercentage;

        [FoldoutGroup("Basic Stats")] 
        [SerializeField] private int blockDuration;
        public int BlockDuration => blockDuration;

        [FoldoutGroup("Interval Values")]
        [SerializeField] private float normalAttackInterval;
        public float NormalAttackInterval => normalAttackInterval;

        [FoldoutGroup("Interval Values")]
        [SerializeField] private float hurryAttackInterval;
        public float HurryAttackInterval => hurryAttackInterval;

    }
}
