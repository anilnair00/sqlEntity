using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirCanada.Appx.AzAiIntegration.DataAccess.SQLTable
{
    [Table("AzAiIntegration", Schema = "Integration")]
    public class AzAiIntegration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTimeOffset CreatedDateTime { get; set; }

        public string? RequestMsg { get; set; }

        public Guid? ResponseId { get; set; }

        public string? ResponseMsg { get; set; }

        public long SsetDocumentId { get; set; }

        public DateTimeOffset UpdatedDateTime { get; set; }

        public string? MessageMetaData { get; set; }

        public string? Stage { get; set; }

        public string? State { get; set; }
    }
}
