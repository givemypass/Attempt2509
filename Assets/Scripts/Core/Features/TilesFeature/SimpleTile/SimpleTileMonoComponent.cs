using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.GameScreenFeature.Mono
{
    public class SimpleTileMonoComponent : MonoBehaviour
    {
        [Serializable]
        public struct Configuration
        {
            public Image[] Images;
        }

        [SerializeField] private Configuration[] _configurations;
        
        public Configuration SetConfiguration(int colorsCount)
        {
            foreach (var configuration in _configurations)
            {
                foreach (var image in configuration.Images)
                {
                    image.gameObject.SetActive(false);
                }
            }
            var result = _configurations[colorsCount - 1];
            foreach (var image in result.Images)
            {
                image.gameObject.SetActive(true);
            }

            return result;
        }
    }
}