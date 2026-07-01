namespace Syslaps.Pdv.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UsuarioEmpresa")]
    public partial class UsuarioEmpresa
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string Usuario_CodigoUsuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string Empresa_CodigoEmpresa { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Empresa Empresa { get; set; }

        [Required]
        public virtual bool Sincronizado { get; set; }
    }
}
