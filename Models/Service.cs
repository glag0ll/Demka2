using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication6.Models;

[Table("services")]
public partial class Service
{
    [Key]
    [Column("id")]
    [StringLength(5)]
    public string Id { get; set; } = null!;

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("code")]
    [StringLength(20)]
    public string Code { get; set; } = null!;

    [Column("costofhour")]
    public decimal Costofhour { get; set; }

    [ForeignKey("Serviceid")]
    [InverseProperty("Services")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
