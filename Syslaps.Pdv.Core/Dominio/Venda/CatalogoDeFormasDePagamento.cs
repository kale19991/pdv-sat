using System.Collections.Generic;

namespace Syslaps.Pdv.Core.Dominio.Venda
{
    public static class CategoriaFormaPagamento
    {
        public const string PagamentoNaEntrega = "Pagamento na entrega";
        public const string CartaoDebito = "Cartões de débito";
        public const string CartaoCredito = "Cartões de crédito";
        public const string PagamentoOnline = "Pagamento online";

        public static readonly IReadOnlyList<string> Ordem = new List<string>
        {
            PagamentoNaEntrega,
            CartaoDebito,
            CartaoCredito,
            PagamentoOnline
        };
    }

    public class FormaDePagamentoDoCatalogo
    {
        public string Nome { get; }
        public string Categoria { get; }

        public FormaDePagamentoDoCatalogo(string nome, string categoria)
        {
            Nome = nome;
            Categoria = categoria;
        }
    }

    /// <summary>
    /// Catálogo de formas de pagamento espelhando o PaymentTypes.cs do ifome-back,
    /// usado para semear o TipoPagamento de cada empresa nova.
    /// </summary>
    public static class CatalogoDeFormasDePagamento
    {
        public static readonly IReadOnlyList<FormaDePagamentoDoCatalogo> Todas = new List<FormaDePagamentoDoCatalogo>
        {
            new FormaDePagamentoDoCatalogo("Dinheiro", CategoriaFormaPagamento.PagamentoNaEntrega),
            new FormaDePagamentoDoCatalogo("Pix", CategoriaFormaPagamento.PagamentoNaEntrega),

            new FormaDePagamentoDoCatalogo("Elo", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Mastercard", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Visa", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Tricard", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("VR Refeição", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("VR Alimentação", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Baneso", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Sodexo Alimentação", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Sodexo Refeição", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Ticket Alimentação", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Ticket Refeição", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Vale Gás", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Alelo Alimentação", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Alelo Refeição", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Greencard Alimentação", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Greencard Refeição", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Ben Alimentação", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Ben Refeição", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Union Pay", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Discover", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Banrisul", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Biq Benefícios", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Card br", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Lo Card", CategoriaFormaPagamento.CartaoDebito),
            new FormaDePagamentoDoCatalogo("Moeda Aratu", CategoriaFormaPagamento.CartaoDebito),

            new FormaDePagamentoDoCatalogo("Elo", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Mastercard", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Tricard", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Tricard Mais", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Baneso", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Visa", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Union Pay", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Discover", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Banrisul", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Diners Club", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Brasil Card", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("American Express", CategoriaFormaPagamento.CartaoCredito),
            new FormaDePagamentoDoCatalogo("Hipercard", CategoriaFormaPagamento.CartaoCredito),

            new FormaDePagamentoDoCatalogo("Pix Online", CategoriaFormaPagamento.PagamentoOnline),
            new FormaDePagamentoDoCatalogo("Cartão de Crédito Online", CategoriaFormaPagamento.PagamentoOnline),
        };
    }
}
