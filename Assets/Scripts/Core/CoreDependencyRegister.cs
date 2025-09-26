using Core.Models;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core
{
    public class CoreDependencyRegister : SDependencyRegister
    {
        [SerializeField] private ColorPaletteConfig _colorPaletteConfig;
        
        protected override World World => SManager.World;

        public override void Register()
        {
            Container.Register(new SceneService());
            Container.Register(_colorPaletteConfig);
        }
    }
}