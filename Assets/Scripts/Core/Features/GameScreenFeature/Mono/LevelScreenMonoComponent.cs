using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.GameScreenFeature.Mono
{
    public class LevelScreenMonoComponent : MonoBehaviour
    {
        public Image BackgroundImage; 
        public Image TransitionImage;
        public TextMeshProUGUI StepsText;
        public TextMeshProUGUI LevelText;
        
        public GridMonoComponent[] Grids;
    }
}