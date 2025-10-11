using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.GameScreenFeature.Mono
{
    public class GameScreenMonoComponent : MonoBehaviour
    {
        public Image BackgroundImage; 
        public Image TransitionImage;
        public TextMeshProUGUI StepsText;
        public TextMeshProUGUI LevelText;
        public Button ResetButton;
        
        public GridMonoComponent[] Grids;
    }
}