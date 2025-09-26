using Core.Services;
using SelfishFramework.Src.Core;
using SelfishFramework.Src.Unity;

namespace Core
{
    public class CoreDependencyRegister : SDependencyRegister
    {
        protected override World World => SManager.World;

        public override void Register()
        {
            base.Register();
            Container.Register(new SceneService());
        }
    }
}