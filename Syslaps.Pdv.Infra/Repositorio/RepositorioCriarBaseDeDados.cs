using System.Linq;
using System.Text;
using Dapper;
using Syslaps.Pdv.Core.Dominio.Base;

namespace Syslaps.Pdv.Infra.Repositorio
{
    public class RepositorioCriarBaseDeDados : RepositorioBase, IRepositorioCriarBaseDeDados
    {
        public void ExcluirTabelas()
        {
            var sql = @"
DROP TABLE IF EXISTS VendaPagamento;
DROP TABLE IF EXISTS VendaProduto;
DROP TABLE IF EXISTS CupomFiscalSat;
DROP TABLE IF EXISTS Venda;
DROP TABLE IF EXISTS ResultadoOperacaoFechamento;
DROP TABLE IF EXISTS ComandaProduto;
DROP TABLE IF EXISTS PedidoProduto;
DROP TABLE IF EXISTS OperacaoCaixa;
DROP TABLE IF EXISTS ProdutoProducao;
DROP TABLE IF EXISTS ConfiguracaoCategoriaProduto;
DROP TABLE IF EXISTS Caixa;
DROP TABLE IF EXISTS ClienteCampanha;
DROP TABLE IF EXISTS Comanda;
DROP TABLE IF EXISTS TipoPagamento;
DROP TABLE IF EXISTS UsuarioEmpresa;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Produto;
DROP TABLE IF EXISTS Parametro;
DROP TABLE IF EXISTS Pedido;
DROP TABLE IF EXISTS Empresa;
";
            ExecutarBlocoSql(sql);
        }

        public void CriarDataBase()
        {
            var sql = @"
CREATE TABLE Empresa (
  CodigoEmpresa     VARCHAR(32)  NOT NULL,
  RazaoSocial       VARCHAR(120) NOT NULL,
  NomeFantasia      VARCHAR(120),
  CpfCnpj           VARCHAR(20)  NOT NULL,
  InscricaoEstadual VARCHAR(20),
  Endereco          VARCHAR(250),
  Numero            VARCHAR(20),
  Bairro            VARCHAR(80),
  Cidade            VARCHAR(80),
  Uf                VARCHAR(2),
  Cep               VARCHAR(10),
  Telefone          VARCHAR(20),
  Email             VARCHAR(120),
  CodigoAtivacaoSat VARCHAR(100),
  ModeloSat         VARCHAR(20),
  Ativa             BIT          NOT NULL DEFAULT 1,
  Sincronizado      BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoEmpresa)
);

CREATE TABLE Caixa (
  CodigoCaixa              VARCHAR(32)  NOT NULL,
  Empresa_CodigoEmpresa    VARCHAR(32)  NOT NULL,
  Nome                     VARCHAR(80)  NOT NULL,
  Machine                  VARCHAR(60)  NOT NULL,
  IP                       VARCHAR(20)  NOT NULL,
  Situacao                 VARCHAR(15)  NOT NULL DEFAULT Fechado,
  CodigoOperacaoDeAbertura VARCHAR(32),
  Sincronizado             BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoCaixa),
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX Caixa_FKIndex1 ON Caixa (Empresa_CodigoEmpresa);

CREATE TABLE ClienteCampanha (
  CodigoClienteCampanha VARCHAR(32)  NOT NULL,
  Empresa_CodigoEmpresa VARCHAR(32)  NOT NULL,
  NomeCampanha          VARCHAR(60)  NOT NULL,
  CpfCnpj               VARCHAR(20)  NOT NULL,
  Email                 VARCHAR(120) NOT NULL,
  Telefone              VARCHAR(20)  NOT NULL,
  NomeCliente           VARCHAR(120) NOT NULL,
  DataCadastro          DATETIME     NOT NULL,
  Observacao            VARCHAR(500),
  Sincronizado          BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoClienteCampanha),
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX ClienteCampanha_FKIndex1 ON ClienteCampanha (Empresa_CodigoEmpresa);

CREATE TABLE Comanda (
  CodigoComanda         VARCHAR(15)  NOT NULL,
  Empresa_CodigoEmpresa VARCHAR(32)  NOT NULL,
  Situacao              VARCHAR(20)  NOT NULL,
  Sincronizado          BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoComanda),
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX Comanda_FKIndex1 ON Comanda (Empresa_CodigoEmpresa);

CREATE TABLE Parametro (
  Nome         VARCHAR(120)  NOT NULL,
  Valor        VARCHAR(1000) NOT NULL,
  Sincronizado BIT           NOT NULL DEFAULT 0,
  PRIMARY KEY (Nome)
);

CREATE TABLE Pedido (
  CodigoPedido          VARCHAR(32)  NOT NULL,
  Empresa_CodigoEmpresa VARCHAR(32)  NOT NULL,
  DataPedido            DATETIME     NOT NULL,
  DataEntrega           DATETIME     NOT NULL,
  NomeCliente           VARCHAR(80)  NOT NULL,
  Telefone              VARCHAR(20),
  Situacao              VARCHAR(15)  NOT NULL,
  Observacao            VARCHAR(500),
  Valor                 DECIMAL      NOT NULL DEFAULT 0,
  Sincronizado          BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoPedido),
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX Pedido_FKIndex1 ON Pedido (Empresa_CodigoEmpresa);

CREATE TABLE Produto (
  CodigoDeBarra          VARCHAR(32)  NOT NULL,
  Empresa_CodigoEmpresa  VARCHAR(32)  NOT NULL,
  TipoProduto            VARCHAR(15)  NOT NULL,
  Modelo                 VARCHAR(60)  NOT NULL,
  Descricao              VARCHAR(250) NOT NULL,
  DescricaoBusca         VARCHAR(200) NOT NULL,
  PrecoCusto             DECIMAL      NOT NULL,
  PrecoVenda             DECIMAL      NOT NULL,
  PrecoVenda2            DECIMAL,
  TipoUnidade            VARCHAR(15)  NOT NULL,
  EstoqueMinimo          INTEGER,
  Marca                  VARCHAR(60)  NOT NULL,
  TipoFiscal             VARCHAR(80),
  CodigoNCM              VARCHAR(10),
  DigitoCST              VARCHAR(15),
  CEST                   VARCHAR(15),
  Categoria              VARCHAR(60)  NOT NULL,
  SubCategoria           VARCHAR(60)  NOT NULL,
  Ativo                  BIT          NOT NULL,
  ExibirNoPdv            BIT          NOT NULL,
  ControlarEstoque       BIT          NOT NULL,
  DescontarInsumoNaVenda BIT          NOT NULL,
  TemProducao            BIT          NOT NULL,
  CodigoParaCupom        VARCHAR(4)   NOT NULL,
  Sincronizado           BIT          NOT NULL DEFAULT 0,
  QtdeEstoque            DECIMAL      NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoDeBarra),
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX Produto_DescricaoBusca ON Produto (DescricaoBusca);
CREATE INDEX Produto_FKIndex1 ON Produto (Empresa_CodigoEmpresa);

CREATE TABLE Usuario (
  CodigoUsuario VARCHAR(32)  NOT NULL,
  Nome          VARCHAR(60)  NOT NULL,
  Senha         VARCHAR(200) NOT NULL,
  Tipo          VARCHAR(15)  NOT NULL,
  Sincronizado  BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoUsuario)
);

CREATE TABLE UsuarioEmpresa (
  Usuario_CodigoUsuario VARCHAR(32) NOT NULL,
  Empresa_CodigoEmpresa VARCHAR(32) NOT NULL,
  Sincronizado          BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (Usuario_CodigoUsuario, Empresa_CodigoEmpresa),
  FOREIGN KEY (Usuario_CodigoUsuario) REFERENCES Usuario (CodigoUsuario) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE INDEX UsuarioEmpresa_FKIndex1 ON UsuarioEmpresa (Usuario_CodigoUsuario);
CREATE INDEX UsuarioEmpresa_FKIndex2 ON UsuarioEmpresa (Empresa_CodigoEmpresa);

CREATE TABLE TipoPagamento (
  CodigoTipoPagamento   VARCHAR(32)  NOT NULL,
  Empresa_CodigoEmpresa VARCHAR(32)  NOT NULL,
  Nome                  VARCHAR(30)  NOT NULL,
  Categoria             VARCHAR(40)  NOT NULL,
  PercentualDesconto    DECIMAL      NOT NULL,
  DiasParaPagamento     INTEGER      NOT NULL,
  Habilitada            BIT          NOT NULL DEFAULT 1,
  Sincronizado          BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoTipoPagamento),
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX TipoPagamento_FKIndex1 ON TipoPagamento (Empresa_CodigoEmpresa);

CREATE TABLE ConfiguracaoCategoriaProduto (
  Categoria             VARCHAR(60) NOT NULL,
  Empresa_CodigoEmpresa VARCHAR(32) NOT NULL,
  TemProducao           BIT         NOT NULL,
  DescontaInsumo        BIT         NOT NULL,
  Sincronizado          BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (Categoria, Empresa_CodigoEmpresa),
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX ConfiguracaoCategoriaProduto_FKIndex1 ON ConfiguracaoCategoriaProduto (Empresa_CodigoEmpresa);

CREATE TABLE ProdutoProducao (
  CodigoProdutoProducao       VARCHAR(32) NOT NULL,
  Produto_CodigoDeBarra       VARCHAR(32) NOT NULL,
  DataProducao                DATE        NOT NULL,
  QuantidadeProduzida         INTEGER     NOT NULL,
  QuantidadeDescartadaInteira INTEGER     NOT NULL,
  QuantidadeDescartadaParcial INTEGER     NOT NULL,
  Sincronizado                BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoProdutoProducao),
  FOREIGN KEY (Produto_CodigoDeBarra) REFERENCES Produto (CodigoDeBarra) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX ProdutoProducao_FKIndex1 ON ProdutoProducao (Produto_CodigoDeBarra);
CREATE INDEX IFK_Rel_18 ON ProdutoProducao (Produto_CodigoDeBarra);

CREATE TABLE OperacaoCaixa (
  CodigoOperacaoCaixa         VARCHAR(32) NOT NULL,
  Empresa_CodigoEmpresa       VARCHAR(32) NOT NULL,
  Usuario_CodigoUsuario       VARCHAR(32) NOT NULL,
  Caixa_CodigoCaixa           VARCHAR(32) NOT NULL,
  DataOperacao                DATETIME    NOT NULL,
  TipoOperacao                VARCHAR(20) NOT NULL,
  ValorOperacao               DECIMAL     NOT NULL,
  CodigoOperacaoCaixaAbertura VARCHAR(32),
  Sincronizado                BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoOperacaoCaixa),
  FOREIGN KEY (Caixa_CodigoCaixa) REFERENCES Caixa (CodigoCaixa) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (Usuario_CodigoUsuario) REFERENCES Usuario (CodigoUsuario) ON DELETE NO ACTION ON UPDATE NO ACTION,
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX OperacaoCaixa_FKIndex1 ON OperacaoCaixa (Caixa_CodigoCaixa);
CREATE INDEX OperacaoCaixa_FKIndex2 ON OperacaoCaixa (Usuario_CodigoUsuario);
CREATE INDEX OperacaoCaixa_FKIndex3 ON OperacaoCaixa (Empresa_CodigoEmpresa);
CREATE INDEX IFK_Rel_15 ON OperacaoCaixa (Caixa_CodigoCaixa);
CREATE INDEX IFK_Rel_10 ON OperacaoCaixa (Usuario_CodigoUsuario);

CREATE TABLE PedidoProduto (
  CodigoPedidoProduto   VARCHAR(32) NOT NULL,
  Produto_CodigoDeBarra VARCHAR(32) NOT NULL,
  Pedido_CodigoPedido   VARCHAR(32) NOT NULL,
  Quantidade            INTEGER     NOT NULL,
  Sincronizado          BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoPedidoProduto),
  FOREIGN KEY (Pedido_CodigoPedido) REFERENCES Pedido (CodigoPedido) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (Produto_CodigoDeBarra) REFERENCES Produto (CodigoDeBarra) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX PedidoProduto_FKIndex1 ON PedidoProduto (Pedido_CodigoPedido);
CREATE INDEX PedidoProduto_FKIndex2 ON PedidoProduto (Produto_CodigoDeBarra);
CREATE INDEX IFK_Rel_23 ON PedidoProduto (Pedido_CodigoPedido);
CREATE INDEX IFK_Rel_22 ON PedidoProduto (Produto_CodigoDeBarra);

CREATE TABLE ComandaProduto (
  CodigoComandaProduto  VARCHAR(32) NOT NULL,
  Comanda_CodigoComanda VARCHAR(15) NOT NULL,
  Produto_CodigoDeBarra VARCHAR(32) NOT NULL,
  Quantidade            DECIMAL     NOT NULL,
  Sincronizado          BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoComandaProduto),
  FOREIGN KEY (Produto_CodigoDeBarra) REFERENCES Produto (CodigoDeBarra) ON DELETE NO ACTION ON UPDATE NO ACTION,
  FOREIGN KEY (Comanda_CodigoComanda) REFERENCES Comanda (CodigoComanda) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX ComandaProduto_FKIndex1 ON ComandaProduto (Produto_CodigoDeBarra);
CREATE INDEX ComandaProduto_FKIndex3 ON ComandaProduto (Comanda_CodigoComanda);
CREATE INDEX IFK_Rel_02 ON ComandaProduto (Produto_CodigoDeBarra);
CREATE INDEX IFK_Rel_17 ON ComandaProduto (Comanda_CodigoComanda);

CREATE TABLE ResultadoOperacaoFechamento (
  CodigoResultado                   VARCHAR(32) NOT NULL,
  OperacaoCaixa_CodigoOperacaoCaixa VARCHAR(32) NOT NULL,
  ValorAbertura                     DECIMAL     NOT NULL,
  ValorContabilizadoNoFechamento    DECIMAL     NOT NULL,
  ValorTotalSangria                 DECIMAL     NOT NULL,
  ValorTotalReforco                 DECIMAL     NOT NULL,
  ValorTotalPagamentoDinheiro       DECIMAL     NOT NULL,
  ValorTotalPagamentoDebito         DECIMAL     NOT NULL,
  ValorTotalPagamentoCredito        DECIMAL     NOT NULL,
  ValorTotalPagamentoOutros         DECIMAL     NOT NULL,
  ValorTotalPagamento               DECIMAL     NOT NULL,
  ValorTotalEstimadoEmEspecie       DECIMAL     NOT NULL,
  DiferencaNoCaixa                  DECIMAL     NOT NULL,
  ValorTotalDescontoVenda           DECIMAL     NOT NULL,
  ValorRecebimentoDebito            DECIMAL     NOT NULL,
  ValorRecebimentoCretito           DECIMAL     NOT NULL,
  Sincronizado                      BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoResultado, OperacaoCaixa_CodigoOperacaoCaixa),
  FOREIGN KEY (OperacaoCaixa_CodigoOperacaoCaixa) REFERENCES OperacaoCaixa (CodigoOperacaoCaixa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX OperacaoCaixaResultado_FKIndex1 ON ResultadoOperacaoFechamento (OperacaoCaixa_CodigoOperacaoCaixa);
CREATE INDEX IFK_Rel_12 ON ResultadoOperacaoFechamento (OperacaoCaixa_CodigoOperacaoCaixa);

CREATE TABLE Venda (
  CodigoVenda                       VARCHAR(40) NOT NULL,
  Empresa_CodigoEmpresa             VARCHAR(32) NOT NULL,
  OperacaoCaixa_CodigoOperacaoCaixa VARCHAR(32) NOT NULL,
  Usuario_CodigoUsuario             VARCHAR(32) NOT NULL,
  ValorTotalVenda                   DECIMAL     NOT NULL,
  ValorTotalDescontoVenda           DECIMAL     NOT NULL,
  ValorTotalRecebimento             DECIMAL     NOT NULL,
  DataRecebimento                   DATETIME    NOT NULL,
  DataVenda                         DATETIME    NOT NULL,
  CpfCnpjCliente                    VARCHAR(20),
  NomeCliente                       VARCHAR(60),
  ValorTroco                        DECIMAL     NOT NULL,
  Sincronizado                      BIT         NOT NULL DEFAULT 0,
  CFOP                              INTEGER     NOT NULL,
  PRIMARY KEY (CodigoVenda),
  FOREIGN KEY (Usuario_CodigoUsuario) REFERENCES Usuario (CodigoUsuario) ON DELETE NO ACTION ON UPDATE NO ACTION,
  FOREIGN KEY (OperacaoCaixa_CodigoOperacaoCaixa) REFERENCES OperacaoCaixa (CodigoOperacaoCaixa) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX Venda_FKIndex1 ON Venda (Usuario_CodigoUsuario);
CREATE INDEX Venda_FKIndex2 ON Venda (OperacaoCaixa_CodigoOperacaoCaixa);
CREATE INDEX Venda_FKIndex3 ON Venda (Empresa_CodigoEmpresa);
CREATE INDEX IFK_Rel_11 ON Venda (Usuario_CodigoUsuario);
CREATE INDEX IFK_Rel_14 ON Venda (OperacaoCaixa_CodigoOperacaoCaixa);

CREATE TABLE CupomFiscalSat (
  CodigoVenda           VARCHAR(32)   NOT NULL,
  Empresa_CodigoEmpresa VARCHAR(32)   NOT NULL,
  CpfCnpj               VARCHAR(20),
  ErrorCode             VARCHAR(200),
  ErrorCode2            VARCHAR(200),
  ErrorMessage          VARCHAR(200),
  InvoiceKey            VARCHAR(100),
  QrCodeSignature       VARCHAR(500),
  SessionCode           VARCHAR(15),
  TimeStamp             VARCHAR(15),
  Total                 VARCHAR(15),
  Xml                   VARCHAR(5000),
  DataOperacao          DATETIME,
  CodigoSat             VARCHAR(20),
  XmlEnvio              VARCHAR(5000),
  PRIMARY KEY (CodigoVenda),
  FOREIGN KEY (CodigoVenda) REFERENCES Venda (CodigoVenda) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (Empresa_CodigoEmpresa) REFERENCES Empresa (CodigoEmpresa) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX CupomFiscalSat_FKIndex1 ON CupomFiscalSat (CodigoVenda);
CREATE INDEX CupomFiscalSat_FKIndex2 ON CupomFiscalSat (Empresa_CodigoEmpresa);
CREATE INDEX IFK_Rel_1555 ON CupomFiscalSat (CodigoVenda);

CREATE TABLE VendaProduto (
  CodigoVendaProduto        VARCHAR(32)  NOT NULL,
  Produto_CodigoDeBarra     VARCHAR(32)  NOT NULL,
  Venda_CodigoVenda         VARCHAR(40)  NOT NULL,
  ValorDoProduto            DECIMAL      NOT NULL,
  ValorDoProdutoComDesconto DECIMAL      NOT NULL,
  ValorDoDesconto           DECIMAL      NOT NULL,
  Quantidade                DECIMAL      NOT NULL,
  ValorTotalVendaProduto    DECIMAL      NOT NULL,
  ValorTotalDoDesconto      DECIMAL      NOT NULL,
  CodigoParaCupom           VARCHAR(4)   NOT NULL,
  DescricaoProduto          VARCHAR(250) NOT NULL,
  Sincronizado              BIT          NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoVendaProduto),
  FOREIGN KEY (Venda_CodigoVenda) REFERENCES Venda (CodigoVenda) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (Produto_CodigoDeBarra) REFERENCES Produto (CodigoDeBarra) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX VendaProduto_FKIndex1 ON VendaProduto (Venda_CodigoVenda);
CREATE INDEX VendaProduto_FKIndex2 ON VendaProduto (Produto_CodigoDeBarra);
CREATE INDEX IFK_Rel_09 ON VendaProduto (Venda_CodigoVenda);
CREATE INDEX IFK_Rel_01 ON VendaProduto (Produto_CodigoDeBarra);

CREATE TABLE VendaPagamento (
  CodigoVendaPagamento              VARCHAR(32) NOT NULL,
  TipoPagamento_CodigoTipoPagamento VARCHAR(32) NOT NULL,
  Venda_CodigoVenda                 VARCHAR(40) NOT NULL,
  ValorPagamento                    DECIMAL     NOT NULL,
  Sincronizado                      BIT         NOT NULL DEFAULT 0,
  PRIMARY KEY (CodigoVendaPagamento),
  FOREIGN KEY (Venda_CodigoVenda) REFERENCES Venda (CodigoVenda) ON DELETE NO ACTION ON UPDATE NO ACTION,
  FOREIGN KEY (TipoPagamento_CodigoTipoPagamento) REFERENCES TipoPagamento (CodigoTipoPagamento) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE INDEX VendaPagamento_FKIndex1 ON VendaPagamento (Venda_CodigoVenda);
CREATE INDEX VendaPagamento_FKIndex2 ON VendaPagamento (TipoPagamento_CodigoTipoPagamento);
CREATE INDEX IFK_Rel_20 ON VendaPagamento (Venda_CodigoVenda);
CREATE INDEX IFK_Rel_19 ON VendaPagamento (TipoPagamento_CodigoTipoPagamento);
";
            ExecutarBlocoSql(sql);
        }

        public void Vacuum()
        {
            LimparLogDaBase();
        }

        private void ExecutarBlocoSql(string sql)
        {
            foreach (var statement in sql
                .Split(new[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                Db.Execute(statement);
            }
        }
    }
}
