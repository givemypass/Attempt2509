using UnityEngine;

namespace Core.Features.LevelsFeature.Models
{
    [CreateAssetMenu(menuName = "Configs/LevelsConfig", fileName = "LevelsConfig", order = 0)]
    public class LevelsConfig : ScriptableObject
    {
        public TextAsset[] LevelsJson;
    }
}