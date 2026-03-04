using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication6.Models;

[Table("clients")]
public partial class Client
{
    [Key]
    [Column("code")]
    [StringLength(15)]
    public string Code { get; set; } = null!;

    [Column("family")]
    [StringLength(50)]
    public string Family { get; set; } = null!;

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("patronymic")]
    [StringLength(50)]
    public string Patronymic { get; set; } = null!;

    [Column("pasportseria")]
    [StringLength(50)]
    public string Pasportseria { get; set; } = null!;

    [Column("pasportnumber")]
    [StringLength(50)]
    public string Pasportnumber { get; set; } = null!;

    [Column("datebirthday")]
    public DateOnly Datebirthday { get; set; }

    [Column("address")]
    [StringLength(150)]
    public string Address { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(50)]
    public string Password { get; set; } = null!;

    [InverseProperty("CodeclientNavigation")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [NotMapped]
    public string Fio => $"{Family} {Name} {Patronymic}";
}
