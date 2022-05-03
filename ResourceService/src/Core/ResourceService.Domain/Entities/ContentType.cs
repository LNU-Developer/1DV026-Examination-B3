using System.Runtime.Serialization;

namespace ResourceService.Domain.Entities
{
    public enum ContentType
    {
        [EnumMember(Value = "image/gif")]
        GIF = 0,
        [EnumMember(Value = "image/jpeg")]
        JPEG,
        [EnumMember(Value = "image/png")]
        PNG
    }
}
