using ScriptCs.Contracts;

namespace ScriptCs.Engine.LolCode
{
    [Module("lolcode")]
    public class LolCodeModule : IModule
    {
        public void Initialize(IModuleConfiguration config)
        {
            config.ScriptEngine<LolCodeEngine>();
        }
    }
}