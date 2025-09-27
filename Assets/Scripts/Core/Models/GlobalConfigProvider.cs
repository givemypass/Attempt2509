using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configs/GlobalConfigProvider", fileName = "GlobalConfigProvider", order = 0)]
    public class GlobalConfigProvider : ScriptableObject
    {
        [SerializeField] private GlobalConfig _config;
        public GlobalConfig Get => _config;
    }
}