using Discord.Rest;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a generic WebSocket-based channel that can send and receive messages.
    /// </summary>
    public interface ISocketMessageChannel : IMessageChannel
    {
        /// <summary>
        ///     Sends a message to this message channel.
        /// </summary>
        /// <remarks>
        ///     This method follows the same behavior as described in <see cref="IMessageChannel.SendMessageAsync"/>.
        ///     Please visit its documentation for more details on this method.
        /// </remarks>
        /// <param name="text">The message to be sent.</param>
        /// <param name="isTTS">Determines whether the message should be read aloud by Discord or not.</param>
        /// <param name="embed">The <see cref="Discord.EmbedType.Rich"/> <see cref="Embed"/> to be sent.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <param name="allowedMentions">
        ///     Specifies if notifications are sent for mentioned users and roles in the message <paramref name="text"/>.
        ///     If <c>null</c>, all mentioned roles and users will be notified.
        /// </param>
        /// <param name="messageReference">The message references to be included. Used to reply to specific messages.</param>
        /// <returns>
        ///     A task that represents an asynchronous send operation for delivering the message. The task result
        ///     contains the sent message.
        /// </returns>
        new Task<RestUserMessage> SendMessageAsync(string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null);
        /// <summary>
        ///     Sends a file to this message channel with an optional caption.
        /// </summary>
        /// <remarks>
        ///     This method follows the same behavior as described in <see cref="IMessageChannel.SendFileAsync(string, string, bool, Embed, RequestOptions, bool, AllowedMentions, MessageReference)"/>.
        ///     Please visit its documentation for more details on this method.
        /// </remarks>
        /// <param name="filePath">The file path of the file.</param>
        /// <param name="text">The message to be sent.</param>
        /// <param name="isTTS">Whether the message should be read aloud by Discord or not.</param>
        /// <param name="embed">The <see cref="Discord.EmbedType.Rich" /> <see cref="Embed" /> to be sent.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <param name="isSpoiler">Whether the message attachment should be hidden as a spoiler.</param>
        /// <param name="allowedMentions">
        ///     Specifies if notifications are sent for mentioned users and roles in the message <paramref name="text"/>.
        ///     If <c>null</c>, all mentioned roles and users will be notified.
        /// </param>
        /// <param name="messageReference">The message references to be included. Used to reply to specific messages.</param>
        /// <returns>
        ///     A task that represents an asynchronous send operation for delivering the message. The task result
        ///     contains the sent message.
        /// </returns>
        new Task<RestUserMessage> SendFileAsync(string filePath, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null);
        /// <summary>
        ///     Sends a file to this message channel with an optional caption.
        /// </summary>
        /// <remarks>
        ///     This method follows the same behavior as described in <see cref="IMessageChannel.SendFileAsync(Stream, string, string, bool, Embed, RequestOptions, bool, AllowedMentions, MessageReference)"/>.
        ///     Please visit its documentation for more details on this method.
        /// </remarks>
        /// <param name="stream">The <see cref="Stream" /> of the file to be sent.</param>
        /// <param name="filename">The name of the attachment.</param>
        /// <param name="text">The message to be sent.</param>
        /// <param name="isTTS">Whether the message should be read aloud by Discord or not.</param>
        /// <param name="embed">The <see cref="Discord.EmbedType.Rich"/> <see cref="Embed"/> to be sent.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <param name="isSpoiler">Whether the message attachment should be hidden as a spoiler.</param>
        /// <param name="allowedMentions">
        ///     Specifies if notifications are sent for mentioned users and roles in the message <paramref name="text"/>.
        ///     If <c>null</c>, all mentioned roles and users will be notified.
        /// </param>
        /// <param name="messageReference">The message references to be included. Used to reply to specific messages.</param>
        /// <returns>
        ///     A task that represents an asynchronous send operation for delivering the message. The task result
        ///     contains the sent message.
        /// </returns>
        new Task<RestUserMessage> SendFileAsync(Stream stream, string filename, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, bool isSpoiler = false, AllowedMentions allowedMentions = null, MessageReference messageReference = null);
    }
}
