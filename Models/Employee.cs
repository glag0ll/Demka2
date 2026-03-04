using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaApplication6.Models;

[Table("employees")]
public partial class Employee
{
    [Key]
    [Column("id")]
    [StringLength(10)]
    public string Id { get; set; } = null!;

    [Column("postid")]
    public int Postid { get; set; }

    [Column("fio")]
    [StringLength(150)]
    public string Fio { get; set; } = null!;

    [Column("login")]
    [StringLength(50)]
    public string Login { get; set; } = null!;

    [Column("password")]
    [StringLength(50)]
    public string Password { get; set; } = null!;

    [Column("lastlogin", TypeName = "timestamp without time zone")]
    public DateTime Lastlogin { get; set; }

    [Column("typeloginid")]
    public int Typeloginid { get; set; }

    [InverseProperty("Employee")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("Postid")]
    [InverseProperty("Employees")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("Typeloginid")]
    [InverseProperty("Employees")]
    public virtual Typelogin Typelogin { get; set; } = null!;
}
