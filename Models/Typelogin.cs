using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication6.Models;

[Table("typelogin")]
public partial class Typelogin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("type")]
    [StringLength(50)]
    public string Type { get; set; } = null!;

    [InverseProperty("Typelogin")]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
