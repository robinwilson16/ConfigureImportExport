using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
[Keyless]
public class FTPConnectionTypeOption
{
    public string DisplayName { get; set; }
    public string Value { get; set; }
}
