using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configs/GlobalConfigSO", fileName = "GlobalConfigSO", order = 0)]
    public class GlobalConfigSO : ScriptableObject
    {
        [SerializeField] private GlobalConfig _config;
        public GlobalConfig Get => _config;
    }
}