using ResourceService.Domain.Common;

namespace ResourceService.Domain.Entities
{
    public class Image : AuditableEntity
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public ContentType ContentType { get; set; }
        public string ImageServiceId { get; set; }
        public Guid UserId { get; set; }
    }
}
