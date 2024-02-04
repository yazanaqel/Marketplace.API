using System.ComponentModel.DataAnnotations;

namespace Marketplace.DAL.Dtos.AttributeDto;
public class AddAttributeDto
{
    [Required]
    public required int ProductId { get; set; }

    [Required,MaxLength(15)]
    public required List<string> AttributesList { get; set; }

}
