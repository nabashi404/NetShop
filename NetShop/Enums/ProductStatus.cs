using System.ComponentModel.DataAnnotations;

namespace NetShop.Enums;

public enum ProductStatus
{
    [Display(Name = "Active")]
    Active,

    [Display(Name = "Out of stock")]
    OutStocked,

    [Display(Name = "Hidden")]
    Hidden,
}
