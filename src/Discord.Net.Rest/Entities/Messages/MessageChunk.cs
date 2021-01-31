using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Discord.API.Message;

namespace Discord.Rest.Entities.Messages
{
    public class MessageChunk
    {
        internal MessageChunk(Model model)
        {

        }


        internal new static MessageChunk Create(Model model)
        {
            var entity = new MessageChunk(model);
            entity.Update(model);
            return entity;
        }

        internal void Update(Model model)
        {
            Id = model.Id;
            CreatedAt = SnowflakeUtils.FromSnowflake(Id);

            if (model.IsTextToSpeech.IsSpecified)
                IsTTS = model.IsTextToSpeech.Value;
            if (model.Pinned.IsSpecified)
                IsPinned = model.Pinned.Value;
            if (model.EditedTimestamp.IsSpecified)
                EditedTimestamp = model.EditedTimestamp.Value;
            if (model.MentionEveryone.IsSpecified)
                MentionedEveryone = model.MentionEveryone.Value;
            if (model.RoleMentions.IsSpecified)
                MentionedRoleIds = model.RoleMentions.Value.ToImmutableArray();

            MentionedUserIds = new List<ulong>();
            MentionedChannelIds = new List<ulong>();

            if (model.Attachments.IsSpecified)
            {
                var value = model.Attachments.Value;
                if (value.Length > 0)
                {
                    var attachments = ImmutableArray.CreateBuilder<Attachment>(value.Length);
                    for (int i = 0; i < value.Length; i++)
                        attachments.Add(Attachment.Create(value[i]));
                    Attachments= attachments.ToImmutable();
                }
                else
                    Attachments = ImmutableArray.Create<Attachment>();
            }

            if (model.Embeds.IsSpecified)
            {
                var value = model.Embeds.Value;
                if (value.Length > 0)
                {
                    var embeds = ImmutableArray.CreateBuilder<Embed>(value.Length);
                    for (int i = 0; i < value.Length; i++)
                        embeds.Add(value[i].ToEntity());
                    Embeds = embeds.ToImmutable();
                }
                else
                    Embeds = ImmutableArray.Create<Embed>();
            }

            if (model.UserMentions.IsSpecified)
            {
                var value = model.UserMentions.Value;

                if (value.Length > 0)
                {
                    var newMentions = ImmutableArray.CreateBuilder<ulong>(value.Length);
                    for (int i = 0; i < value.Length; i++)
                    {
                        var val = value[i];
                        if (val.Object != null)
                            newMentions.Add(val.Object.Id);
                    }
                    MentionedUserIds = newMentions.ToReadOnlyCollection();
                }
            }
            
            if (model.Content.IsSpecified)
            {
                var text = model.Content.Value;
                model.Content = text;
            }
        }

        public ulong Id { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public MessageType Type { get; private set; }
        public MessageSource Source { get; private set; }
        public bool IsTTS { get; private set; }
        public bool IsPinned { get; private set; }
        public bool IsSuppressed { get; private set; }
        public bool MentionedEveryone { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }
        public DateTimeOffset? EditedTimestamp { get; private set; }
        public IReadOnlyCollection<IAttachment> Attachments { get; private set; }
        public IReadOnlyCollection<IEmbed> Embeds { get; private set; }
        public IReadOnlyCollection<ITag> Tags { get; private set; }
        public IReadOnlyCollection<ulong> MentionedChannelIds { get; private set; }
        public IReadOnlyCollection<ulong> MentionedRoleIds { get; private set; }
        public IReadOnlyCollection<ulong> MentionedUserIds { get; private set; }
        public MessageActivity Activity { get; private set; }
        public MessageApplication Application { get; private set; }
        public MessageReference Reference { get; private set; }
    }
}
