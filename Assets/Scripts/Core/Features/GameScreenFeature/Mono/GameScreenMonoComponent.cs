using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.GameScreenFeature.Mono
{
    public class GameScreenMonoComponent : MonoBehaviour
    {
        public Image BackgroundImage; 
        public Image TransitionImage;
        public Image[] ColorSigns;
        public TextMeshProUGUI StepsText;
        
        public GridMonoComponent[] Grids;
    }
}