using System.Diagnostics;
using Model = Discord.API.User;

namespace Discord.Rest
{
    /// <summary>
    ///     Represents a REST-based group user.
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class RestGroupUser : RestUser, IGroupUser
    {
        internal RestGroupUser(BaseDiscordClient discord, ulong id)
            : base(discord, id)
        {
        }
        internal new static RestGroupUser Create(BaseDiscordClient discord, Model model)
        {
            var entity = new RestGroupUser(discord, model.Id);
            entity.Update(model);
            return entity;
        }

    }
}
