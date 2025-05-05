using System.ComponentModel.DataAnnotations;

namespace CurrencyApp.Infra.Options;

public class JwtOptions
{
    [Required(AllowEmptyStrings = false)]
    public string Issuer { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Secret { get; set; }

    [Required]
    public int? AccessExpiryInMins { get; set; }

    [Required]
    public int? RefreshExpiryInDays { get; set; }
}