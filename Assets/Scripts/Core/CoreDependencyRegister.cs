using Core.Features.TilesFeature.Services;
using Core.Models;
using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core
{
    public class CoreDependencyRegister : SDependencyRegister
    {
        [SerializeField] private ColorPaletteConfigProvider _colorPaletteConfigProvider;
        [SerializeField] private GlobalConfigProvider _globalConfigProvider;
        
        protected override World World => SManager.World;

        public override void Register()
        {
            Container.Register(_colorPaletteConfigProvider);
            Container.Register(_globalConfigProvider);
            Container.Register<ISceneService>(new SceneService());
            Container.Register<IColorPaletteService>(new ColorPaletteService());
            Container.Register(new SimpleTileFactoryService());
        }
    }
}