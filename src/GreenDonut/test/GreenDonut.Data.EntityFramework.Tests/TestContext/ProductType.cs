// ReSharper disable CollectionNeverUpdated.Global

using System.ComponentModel.DataAnnotations;

namespace GreenDonut.Data.TestContext;

public class ProductType
{
    public int Id { get; set; }

    [Required] public string Name { get; set; } = default!;

    public ICollection<Product> Products { get; } = [];
}
