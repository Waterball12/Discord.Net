using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Model = Discord.API.Channel;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a WebSocket-based direct-message channel.
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class SocketDMChannel : SocketChannel, IDMChannel, ISocketPrivateChannel, ISocketMessageChannel
    {
        /// <summary>
        ///     Gets the recipient of the channel.
        /// </summary>
        public SocketUser Recipient { get; }

        /// <summary>
        ///     Gets a collection that is the current logged-in user and the recipient.
        /// </summary>
        public new IReadOnlyCollection<SocketUser> Users => ImmutableArray.Create(Discord.CurrentUser, Recipient);

        internal SocketDMChannel(DiscordSocketClient discord, ulong id, SocketGlobalUser recipient)
            : base(discord, id)
        {
            Recipient = recipient;
            recipient.GlobalUser.AddRef();
        }
        internal static SocketDMChannel Create(DiscordSocketClient discord, ClientState state, Model model)
        {
            var entity = new SocketDMChannel(discord, model.Id, discord.GetOrCreateUser(state, model.Recipients.Value[0]));
            entity.Update(state, model);
            return entity;
        }
        internal override void Update(ClientState state, Model model)
        {
            Recipient.Update(state, model.Recipients.Value[0]);
        }

        /// <inheritdoc />
        public Task CloseAsync(RequestOptions options = null)
            => ChannelHelper.DeleteAsync(this, Discord, options);
        
        /// <summary>
        ///     Gets the message associated with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">TThe ID of the message.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns>
        ///     The message gotten from either the cache or the download, or <c>null</c> if none is found.
        /// </returns>
        public async Task<IMessage> GetMessageAsync(ulong id, RequestOptions options = null)
        {
            return await ChannelHelper.GetMessageAsync(this, Discord, id, options).ConfigureAwait(false);
        }


        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Message content is too long, length must be less or equal to <see cref="DiscordConfig.MaxMessageSize"/>.</exception>
        public Task<RestUserMessage> SendMessageAsync(string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
            => ChannelHelper.SendMessageAsync(this, Discord, text, isTTS, embed, allowedMentions, messageReference, options);

        /// <inheritdoc />
        public Task<RestUserMessage> SendFileAsync(string filePath, string text, bool isTTS = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
            => ChannelHelper.SendFileAsync(this, Discord, filePath, text, isTTS, embed, allowedMentions, messageReference, options, isSpoiler);
        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">Message content is too long, length must be less or equal to <see cref="DiscordConfig.MaxMessageSize"/>.</exception>
        public Task<RestUserMessage> SendFileAsync(Stream stream, string filename, string text, bool isTTS = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null)
            => ChannelHelper.SendFileAsync(this, Discord, stream, filename, text, isTTS, embed, allowedMentions, messageReference, options, isSpoiler);
        /// <inheritdoc />
        public Task DeleteMessageAsync(ulong messageId, RequestOptions options = null)
            => ChannelHelper.DeleteMessageAsync(this, messageId, Discord, options);
        /// <inheritdoc />
        public Task DeleteMessageAsync(IMessage message, RequestOptions options = null)
            => ChannelHelper.DeleteMessageAsync(this, message.Id, Discord, options);

        /// <inheritdoc />
        public Task TriggerTypingAsync(RequestOptions options = null)
            => ChannelHelper.TriggerTypingAsync(this, Discord, options);
        /// <inheritdoc />
        public IDisposable EnterTypingState(RequestOptions options = null)
            => ChannelHelper.EnterTypingState(this, Discord, options);

        //Users
        /// <summary>
        ///     Gets a user in this channel from the provided <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The snowflake identifier of the user.</param>
        /// <returns>
        ///     A <see cref="SocketUser"/> object that is a recipient of this channel; otherwise <c>null</c>.
        /// </returns>
        public new SocketUser GetUser(ulong id)
        {
            if (id == Recipient.Id)
                return Recipient;
            else if (id == Discord.CurrentUser.Id)
                return Discord.CurrentUser;
            else
                return null;
        }

        /// <summary>
        ///     Returns the recipient user.
        /// </summary>
        public override string ToString() => $"@{Recipient}";
        private string DebuggerDisplay => $"@{Recipient} ({Id}, DM)";
        internal new SocketDMChannel Clone() => MemberwiseClone() as SocketDMChannel;

        //SocketChannel
        /// <inheritdoc />
        internal override IReadOnlyCollection<SocketUser> GetUsersInternal() => Users;
        /// <inheritdoc />
        internal override SocketUser GetUserInternal(ulong id) => GetUser(id);

        //IDMChannel
        /// <inheritdoc />
        IUser IDMChannel.Recipient => Recipient;

        //ISocketPrivateChannel
        /// <inheritdoc />
        IReadOnlyCollection<SocketUser> ISocketPrivateChannel.Recipients => ImmutableArray.Create(Recipient);

        //IPrivateChannel
        /// <inheritdoc />
        IReadOnlyCollection<IUser> IPrivateChannel.Recipients => ImmutableArray.Create<IUser>(Recipient);

        //IMessageChannel
        /// <inheritdoc />
        async Task<IMessage> IMessageChannel.GetMessageAsync(ulong id, CacheMode mode, RequestOptions options)
        {
            return await GetMessageAsync(id, options).ConfigureAwait(false);
        }
        /// <inheritdoc />
        IAsyncEnumerable<IReadOnlyCollection<IMessage>> IMessageChannel.GetMessagesAsync(int limit, CacheMode mode, RequestOptions options)
            => SocketChannelHelper.GetMessagesAsync(this, Discord, new MessageCache(Discord), null, Direction.Before, limit, mode, options);
        /// <inheritdoc />
        IAsyncEnumerable<IReadOnlyCollection<IMessage>> IMessageChannel.GetMessagesAsync(ulong fromMessageId, Direction dir, int limit, CacheMode mode, RequestOptions options)
            => SocketChannelHelper.GetMessagesAsync(this, Discord, new MessageCache(Discord), fromMessageId, dir, limit, mode, options);

        IAsyncEnumerable<IReadOnlyCollection<IMessage>> IMessageChannel.GetMessagesAsync(ulong? fromMessageId, Direction dir, int limit, CacheMode mode, RequestOptions options)
            => SocketChannelHelper.GetMessagesAsync(this, Discord, new MessageCache(Discord), fromMessageId, dir, limit, mode, options);

        /// <inheritdoc />
        IAsyncEnumerable<IReadOnlyCollection<IMessage>> IMessageChannel.GetMessagesAsync(IMessage fromMessage, Direction dir, int limit, CacheMode mode, RequestOptions options)
            => SocketChannelHelper.GetMessagesAsync(this, Discord, new MessageCache(Discord), fromMessage.Id, dir, limit, mode, options);
        /// <inheritdoc />
        async Task<IUserMessage> IMessageChannel.SendFileAsync(string filePath, string text, bool isTTS, Embed embed, RequestOptions options, bool isSpoiler, AllowedMentions allowedMentions, MessageReference messageReference)
            => await SendFileAsync(filePath, text, isTTS, embed, options, isSpoiler, allowedMentions, messageReference).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IUserMessage> IMessageChannel.SendFileAsync(Stream stream, string filename, string text, bool isTTS, Embed embed, RequestOptions options, bool isSpoiler, AllowedMentions allowedMentions, MessageReference messageReference)
            => await SendFileAsync(stream, filename, text, isTTS, embed, options, isSpoiler, allowedMentions, messageReference).ConfigureAwait(false);
        /// <inheritdoc />
        async Task<IUserMessage> IMessageChannel.SendMessageAsync(string text, bool isTTS, Embed embed, RequestOptions options, AllowedMentions allowedMentions, MessageReference messageReference)
            => await SendMessageAsync(text, isTTS, embed, options, allowedMentions, messageReference).ConfigureAwait(false);

        //IChannel
        /// <inheritdoc />
        string IChannel.Name => $"@{Recipient}";

        /// <inheritdoc />
        Task<IUser> IChannel.GetUserAsync(ulong id, CacheMode mode, RequestOptions options)
            => Task.FromResult<IUser>(GetUser(id));
        /// <inheritdoc />
        IAsyncEnumerable<IReadOnlyCollection<IUser>> IChannel.GetUsersAsync(CacheMode mode, RequestOptions options)
            => ImmutableArray.Create<IReadOnlyCollection<IUser>>(Users).ToAsyncEnumerable();
    }
}
