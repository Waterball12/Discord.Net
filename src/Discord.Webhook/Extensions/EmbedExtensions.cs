using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Webhook.Extensions
{
    internal static class EmbedExtensions
    {
        public static Common.Embed ToModel(this Embed entity)
        {
            if (entity == null) return null;
            var model = new Common.Embed
            {
                Title = entity.Title,
                Description = entity.Description,
                Url = entity.Url,
                Timestamp = entity.Timestamp,
                Color = entity.Color?.RawValue
            };
            if (entity.Author != null)
                model.Author = entity.Author.Value.ToModel();
            model.Fields = entity.Fields.Select(x => x.ToModel()).ToArray();
            if (entity.Footer != null)
                model.Footer = entity.Footer.Value.ToModel();
            if (entity.Image != null)
                model.Image = entity.Image.Value.ToModel();
            if (entity.Thumbnail != null)
                model.Thumbnail = entity.Thumbnail.Value.ToModel();
            return model;
        }
        
        public static Common.EmbedAuthor ToModel(this EmbedAuthor entity)
        {
            return new Common.EmbedAuthor { Name = entity.Name, Url = entity.Url, IconUrl = entity.IconUrl };
        }
        public static Common.EmbedField ToModel(this EmbedField entity)
        {
            return new Common.EmbedField { Name = entity.Name, Value = entity.Value, Inline = entity.Inline };
        }
        public static Common.EmbedFooter ToModel(this EmbedFooter entity)
        {
            return new Common.EmbedFooter { Text = entity.Text, IconUrl = entity.IconUrl };
        }
        public static Common.EmbedImage ToModel(this EmbedImage entity)
        {
            return new Common.EmbedImage { Url = entity.Url };
        }
        public static Common.EmbedThumbnail ToModel(this EmbedThumbnail entity)
        {
            return new Common.EmbedThumbnail { Url = entity.Url };
        }
        
    }
}
