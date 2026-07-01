namespace Syslaps.Pdv.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ConfiguracaoCategoriaProduto")]
    public partial class ConfiguracaoCategoriaProduto : IEntidadeDaEmpresa
    {
        [Key]
        [StringLength(60)]
        public string Categoria { get; set; }

        [Key]
        [Required]
        [StringLength(32)]
        public string Empresa_CodigoEmpresa { get; set; }

        public bool TemProducao { get; set; }

        public bool DescontaInsumo { get; set; }

        [Required]
        public virtual bool Sincronizado { get; set; }
    }
}
