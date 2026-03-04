using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication6.Models;

[Table("orders")]
public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Column("datecreate")]
    public DateOnly Datecreate { get; set; }

    [Column("timeorder")]
    public TimeOnly Timeorder { get; set; }

    [Column("codeclient")]
    [StringLength(15)]
    public string Codeclient { get; set; } = null!;

    [Column("employeeid")]
    [StringLength(10)]
    public string Employeeid { get; set; } = null!;

    [Column("statusid")]
    public int Statusid { get; set; }

    [Column("dateclose")]
    public DateOnly? Dateclose { get; set; }

    [Column("rentaltime")]
    [StringLength(50)]
    public string Rentaltime { get; set; } = null!;

    [ForeignKey("Codeclient")]
    [InverseProperty("Orders")]
    public virtual Client CodeclientNavigation { get; set; } = null!;

    [ForeignKey("Employeeid")]
    [InverseProperty("Orders")]
    public virtual Employee Employee { get; set; } = null!;

    [ForeignKey("Statusid")]
    [InverseProperty("Orders")]
    public virtual Orderstatus Status { get; set; } = null!;

    [ForeignKey("Orderid")]
    [InverseProperty("Orders")]
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    [NotMapped]
    public string ServicesList => Services.Any() ? string.Join(", ", Services.Select(s => s.Name)) : "Нет услуг";
}
