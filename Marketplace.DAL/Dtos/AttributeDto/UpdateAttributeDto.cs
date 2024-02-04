using System.ComponentModel.DataAnnotations;

namespace Marketplace.DAL.Dtos.AttributeDto;
public class UpdateAttributeDto
{
    [Required]
    public required int AttributeId { get; set; }

    [Required, MaxLength(15)]
    public required string AttributeName { get; set; }
}
