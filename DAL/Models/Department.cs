﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Table("Department")]
public partial class Department
{
    [Key]
    public int Id { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("Head_id")]
    public int? HeadId { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();

    [ForeignKey("HeadId")]
    [InverseProperty("Departments")]
    public virtual Doctor? Head { get; set; }
}