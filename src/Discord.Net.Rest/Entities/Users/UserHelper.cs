using Discord.API.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model = Discord.API.User;
using ImageModel = Discord.API.Image;
using System.Linq;

namespace Discord.Rest
{
    internal static class UserHelper
    {
        public static async Task<Model> ModifyAsync(ISelfUser user, BaseDiscordClient client, Action<SelfUserProperties> func,
            RequestOptions options)
        {
            var args = new SelfUserProperties();
            func(args);
            var apiArgs = new API.Rest.ModifyCurrentUserParams
            {
                Avatar = args.Avatar.IsSpecified ? args.Avatar.Value?.ToModel() : Optional.Create<ImageModel?>(),
                Username = args.Username
            };

            if (!apiArgs.Avatar.IsSpecified && user.AvatarId != null)
                apiArgs.Avatar = new ImageModel(user.AvatarId);

            return await client.ApiClient.ModifySelfAsync(apiArgs, options).ConfigureAwait(false);
        }
        public static async Task<GuildUserProperties> ModifyAsync(IGuildUser user, BaseDiscordClient client, Action<GuildUserProperties> func,
            RequestOptions options)
        {
            var args = new GuildUserProperties();
            func(args);
            var apiArgs = new API.Rest.ModifyGuildMemberParams
            {
                Deaf = args.Deaf,
                Mute = args.Mute,
                Nickname = args.Nickname
            };

            if (args.Channel.IsSpecified)
                apiArgs.ChannelId = args.Channel.Value?.Id;
            else if (args.ChannelId.IsSpecified)
                apiArgs.ChannelId = args.ChannelId.Value;

            if (args.Roles.IsSpecified)
                apiArgs.RoleIds = args.Roles.Value.Select(x => x.Id).ToArray();
            else if (args.RoleIds.IsSpecified)
                apiArgs.RoleIds = args.RoleIds.Value.ToArray();

            /*
             * Ensure that the nick passed in the params of the request is not null.
             * string.Empty ("") is the only way to reset the user nick in the API,
             * a value of null does not. This is a workaround.
             */
            if (apiArgs.Nickname.IsSpecified && apiArgs.Nickname.Value == null)
                apiArgs.Nickname = new Optional<string>(string.Empty);

            await client.ApiClient.ModifyGuildMemberAsync(user.GuildId, user.Id, apiArgs, options).ConfigureAwait(false);
            return args;
        }

        public static async Task KickAsync(IGuildUser user, BaseDiscordClient client,
            string reason, RequestOptions options)
        {
            await client.ApiClient.RemoveGuildMemberAsync(user.GuildId, user.Id, reason, options).ConfigureAwait(false);
        }

        public static async Task KickAsync(ulong guildId, ulong userId, BaseDiscordClient client,
            string reason, RequestOptions options)
        {
            await client.ApiClient.RemoveGuildMemberAsync(guildId, userId, reason, options).ConfigureAwait(false);
        }

        public static async Task<RestDMChannel> CreateDMChannelAsync(IUser user, BaseDiscordClient client,
            RequestOptions options)
        {
            var args = new CreateDMChannelParams(user.Id);
            return RestDMChannel.Create(client, await client.ApiClient.CreateDMChannelAsync(args, options).ConfigureAwait(false));
        }

        public static async Task AddRolesAsync(IGuildUser user, BaseDiscordClient client, IEnumerable<ulong> roleIds, RequestOptions options)
        public static async Task<RestDMChannel> CreateDMChannelAsync(ulong userId, BaseDiscordClient client,
            RequestOptions options)
        {
            var args = new CreateDMChannelParams(userId);
            return RestDMChannel.Create(client, await client.ApiClient.CreateDMChannelAsync(args, options).ConfigureAwait(false));
        }

        public static async Task AddRolesAsync(IGuildUser user, BaseDiscordClient client, IEnumerable<IRole> roles, RequestOptions options)
        {
            foreach (var roleId in roleIds)
                await client.ApiClient.AddRoleAsync(user.Guild.Id, user.Id, roleId, options).ConfigureAwait(false);
        }

        public static async Task RemoveRolesAsync(IGuildUser user, BaseDiscordClient client, IEnumerable<ulong> roleIds, RequestOptions options)
        {
            foreach (var roleId in roleIds)
                await client.ApiClient.RemoveRoleAsync(user.Guild.Id, user.Id, roleId, options).ConfigureAwait(false);
        }

        public static async Task AddRolesAsync(ulong guildId, ulong userId, BaseDiscordClient client, IEnumerable<ulong> roles, RequestOptions options)
        {
            foreach (var role in roles)
                await client.ApiClient.AddRoleAsync(guildId, userId, role, options).ConfigureAwait(false);
        }

        public static async Task RemoveRolesAsync(ulong guildId, ulong userId, BaseDiscordClient client, IEnumerable<ulong> roles, RequestOptions options)
        {
            foreach (var role in roles)
                await client.ApiClient.RemoveRoleAsync(guildId, userId, role, options).ConfigureAwait(false);
        }
    }
}
