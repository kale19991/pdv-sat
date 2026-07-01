namespace Syslaps.Pdv.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Empresa")]
    public partial class Empresa
    {
        [Key]
        [StringLength(32)]
        public string CodigoEmpresa { get; set; }

        [Required]
        [StringLength(120)]
        public string RazaoSocial { get; set; }

        [StringLength(120)]
        public string NomeFantasia { get; set; }

        [Required]
        [StringLength(20)]
        public string CpfCnpj { get; set; }

        [StringLength(20)]
        public string InscricaoEstadual { get; set; }

        [StringLength(250)]
        public string Endereco { get; set; }

        [StringLength(20)]
        public string Numero { get; set; }

        [StringLength(80)]
        public string Bairro { get; set; }

        [StringLength(80)]
        public string Cidade { get; set; }

        [StringLength(2)]
        public string Uf { get; set; }

        [StringLength(10)]
        public string Cep { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        [StringLength(120)]
        public string Email { get; set; }

        [StringLength(100)]
        public string CodigoAtivacaoSat { get; set; }

        [StringLength(20)]
        public string ModeloSat { get; set; }

        [Required]
        public virtual bool Ativa { get; set; }

        [Required]
        public virtual bool Sincronizado { get; set; }
    }
}
