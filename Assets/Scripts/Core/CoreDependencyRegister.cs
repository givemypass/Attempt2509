using Core.Features.LevelsFeature.Models;
using Core.Features.LevelsFeature.Services;
using Core.Features.TilesFeature.Services;
using Core.Models;
using Core.Services;
using Core.Utils;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;
using UnityEngine;

namespace Core
{
    public class CoreDependencyRegister : SDependencyRegister
    {
        [SerializeField] private ColorPaletteConfigProvider _colorPaletteConfigProvider;
        [SerializeField] private GlobalConfigProvider _globalConfigProvider;
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private LevelsConfig _editorLevelsConfig;
        
        protected override World World => SManager.World;

        public override void Register()
        {
            Container.Register(_colorPaletteConfigProvider);
            Container.Register(_globalConfigProvider);
            Container.Register<ISceneService>(new SceneService());
            Container.Register<IColorPaletteService>(new ColorPaletteService());
            Container.Register<ITileFactoryService>(new TileFactoryService());
            var levelsConfig = _levelsConfig;
#if UNITY_EDITOR
            if (EditorPlayArguments.IsTestMode())
            {
                levelsConfig = _editorLevelsConfig;
            }
#endif
            Container.Register<ILevelsService>(new LevelsService(levelsConfig));
            Container.Register(new TileModelsService());
            Container.Register(new TileModelsRandomService());
        }
    }
}