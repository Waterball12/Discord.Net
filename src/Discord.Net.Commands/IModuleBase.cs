using Discord.Commands.Builders;
using System.Threading.Tasks;

namespace Discord.Commands
{
    internal interface IModuleBase
    {
        ValueTask BeforeExecuteAsync(CommandInfo command);

        ValueTask AfterExecuteAsync(CommandInfo command);

        void SetContext(ICommandContext context);

        void OnModuleBuilding(CommandService commandService, ModuleBuilder builder);
    }
}
