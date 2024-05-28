﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AccessManagmentAPI.Repos.Models
{
    [Table("tbl_pwdManger")]
    public partial class TblPwdManger
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Column("password")]
        [StringLength(200)]
        public string Password { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
    }
}
