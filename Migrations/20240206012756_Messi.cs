using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjetoCafe.Migrations
{
    /// <inheritdoc />
    public partial class Messi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_adicionais",
                columns: table => new
                {
                    AdicionalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(30)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_adicional", x => x.AdicionalID);
                });

            migrationBuilder.CreateTable(
                name: "tb_clientes",
                columns: table => new
                {
                    ClienteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(70)", nullable: false),
                    Telefone = table.Column<string>(type: "char(11)", nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", nullable: true),
                    CPF = table.Column<string>(type: "char(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cliente", x => x.ClienteID);
                });

            migrationBuilder.CreateTable(
                name: "tb_comandas",
                columns: table => new
                {
                    ComandaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroComanda = table.Column<int>(type: "int", nullable: false),
                    Mesa = table.Column<int>(type: "int", nullable: true),
                    EstaAberta = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comanda", x => x.ComandaID);
                });

            migrationBuilder.CreateTable(
                name: "tb_formas_pagamento",
                columns: table => new
                {
                    FormaPagamentoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_forma_pagamento", x => x.FormaPagamentoID);
                });

            migrationBuilder.CreateTable(
                name: "tb_funcionarios",
                columns: table => new
                {
                    FuncionarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_funcionario", x => x.FuncionarioID);
                });

            migrationBuilder.CreateTable(
                name: "tb_itens",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(30)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(45)", nullable: true),
                    Tamanho = table.Column<string>(type: "char(1)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item", x => x.ItemID);
                });

            migrationBuilder.CreateTable(
                name: "tb_notas_fiscais",
                columns: table => new
                {
                    NotaFiscalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHoraCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    ValorTotal = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    TaxaServico = table.Column<bool>(type: "bit", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ValorFinal = table.Column<decimal>(type: "decimal(7,2)", nullable: false),
                    ComandaID = table.Column<int>(type: "int", nullable: false),
                    FormaPagamentoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_nota_fiscal", x => x.NotaFiscalID);
                    table.ForeignKey(
                        name: "FK_tb_notas_fiscais_tb_comandas_ComandaID",
                        column: x => x.ComandaID,
                        principalTable: "tb_comandas",
                        principalColumn: "ComandaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_notas_fiscais_tb_formas_pagamento_FormaPagamentoID",
                        column: x => x.FormaPagamentoID,
                        principalTable: "tb_formas_pagamento",
                        principalColumn: "FormaPagamentoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_pedidos",
                columns: table => new
                {
                    PedidoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoraFeito = table.Column<TimeOnly>(type: "time(0)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FuncionarioID = table.Column<int>(type: "int", nullable: false),
                    ComandaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pedido", x => x.PedidoID);
                    table.ForeignKey(
                        name: "FK_tb_pedidos_tb_comandas_ComandaID",
                        column: x => x.ComandaID,
                        principalTable: "tb_comandas",
                        principalColumn: "ComandaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_pedidos_tb_funcionarios_FuncionarioID",
                        column: x => x.FuncionarioID,
                        principalTable: "tb_funcionarios",
                        principalColumn: "FuncionarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_cupons_cliente",
                columns: table => new
                {
                    NotaFiscalID = table.Column<int>(type: "int", nullable: false),
                    ClienteID = table.Column<int>(type: "int", nullable: false),
                    DataValidade = table.Column<DateTime>(type: "date", nullable: true),
                    DataGeracao = table.Column<DateTime>(type: "date", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Valido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cupom_cliente", x => new { x.ClienteID, x.NotaFiscalID });
                    table.ForeignKey(
                        name: "FK_tb_cupons_cliente_tb_clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "tb_clientes",
                        principalColumn: "ClienteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_cupons_cliente_tb_notas_fiscais_NotaFiscalID",
                        column: x => x.NotaFiscalID,
                        principalTable: "tb_notas_fiscais",
                        principalColumn: "NotaFiscalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_pedido_adicionais",
                columns: table => new
                {
                    PedidoID = table.Column<int>(type: "int", nullable: false),
                    AdicionalID = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pedido_adicional", x => new { x.PedidoID, x.AdicionalID });
                    table.ForeignKey(
                        name: "FK_tb_pedido_adicionais_tb_adicionais_AdicionalID",
                        column: x => x.AdicionalID,
                        principalTable: "tb_adicionais",
                        principalColumn: "AdicionalID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_pedido_adicionais_tb_pedidos_PedidoID",
                        column: x => x.PedidoID,
                        principalTable: "tb_pedidos",
                        principalColumn: "PedidoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_pedido_itens",
                columns: table => new
                {
                    PedidoID = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(70)", nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pedido_item", x => new { x.PedidoID, x.ItemID });
                    table.ForeignKey(
                        name: "FK_tb_pedido_itens_tb_itens_ItemID",
                        column: x => x.ItemID,
                        principalTable: "tb_itens",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_pedido_itens_tb_pedidos_PedidoID",
                        column: x => x.PedidoID,
                        principalTable: "tb_pedidos",
                        principalColumn: "PedidoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tb_adicionais",
                columns: new[] { "AdicionalID", "Descricao", "Valor" },
                values: new object[,]
                {
                    { 1, "Chantilly", 3.9m },
                    { 2, "Ovo", 1m }
                });

            migrationBuilder.InsertData(
                table: "tb_clientes",
                columns: new[] { "ClienteID", "CPF", "Email", "Nome", "Telefone" },
                values: new object[,]
                {
                    { 1, "11122233345", "leonardo@example.com", "Leonardo", "41912332112" },
                    { 2, "33322211154", "guilherme@example.com", "Guilherme", "41998765432" }
                });

            migrationBuilder.InsertData(
                table: "tb_comandas",
                columns: new[] { "ComandaID", "EstaAberta", "Mesa", "NumeroComanda" },
                values: new object[,]
                {
                    { 1, false, null, 1 },
                    { 2, true, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "tb_formas_pagamento",
                columns: new[] { "FormaPagamentoID", "Descricao" },
                values: new object[,]
                {
                    { 1, "Pix" },
                    { 2, "Debito" }
                });

            migrationBuilder.InsertData(
                table: "tb_funcionarios",
                columns: new[] { "FuncionarioID", "Nome" },
                values: new object[,]
                {
                    { 1, "Pedro" },
                    { 2, "Andre" }
                });

            migrationBuilder.InsertData(
                table: "tb_itens",
                columns: new[] { "ItemID", "Descricao", "Nome", "Tamanho", "Valor" },
                values: new object[,]
                {
                    { 1, null, "Cappuccino", "M", 10.9m },
                    { 2, null, "Cafe Expresso", "P", 7.5m }
                });

            migrationBuilder.InsertData(
                table: "tb_notas_fiscais",
                columns: new[] { "NotaFiscalID", "ComandaID", "DataHoraCriacao", "Desconto", "FormaPagamentoID", "TaxaServico", "ValorFinal", "ValorTotal" },
                values: new object[] { 1, 2, new DateTime(2024, 2, 5, 22, 27, 55, 688, DateTimeKind.Local).AddTicks(9288), 0m, 2, false, 14.8m, 14.8m });

            migrationBuilder.InsertData(
                table: "tb_pedidos",
                columns: new[] { "PedidoID", "ComandaID", "FuncionarioID", "HoraFeito", "ValorTotal" },
                values: new object[] { 1, 2, 1, new TimeOnly(13, 37, 20), 14.8m });

            migrationBuilder.InsertData(
                table: "tb_cupons_cliente",
                columns: new[] { "ClienteID", "NotaFiscalID", "DataGeracao", "DataValidade", "Valido", "Valor" },
                values: new object[] { 1, 1, new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 10m });

            migrationBuilder.InsertData(
                table: "tb_pedido_adicionais",
                columns: new[] { "AdicionalID", "PedidoID", "Quantidade" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "tb_pedido_itens",
                columns: new[] { "ItemID", "PedidoID", "Observacao", "Quantidade" },
                values: new object[] { 1, 1, null, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_tb_cupons_cliente_NotaFiscalID",
                table: "tb_cupons_cliente",
                column: "NotaFiscalID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_notas_fiscais_ComandaID",
                table: "tb_notas_fiscais",
                column: "ComandaID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_notas_fiscais_FormaPagamentoID",
                table: "tb_notas_fiscais",
                column: "FormaPagamentoID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_adicionais_AdicionalID",
                table: "tb_pedido_adicionais",
                column: "AdicionalID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedido_itens_ItemID",
                table: "tb_pedido_itens",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedidos_ComandaID",
                table: "tb_pedidos",
                column: "ComandaID");

            migrationBuilder.CreateIndex(
                name: "IX_tb_pedidos_FuncionarioID",
                table: "tb_pedidos",
                column: "FuncionarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_cupons_cliente");

            migrationBuilder.DropTable(
                name: "tb_pedido_adicionais");

            migrationBuilder.DropTable(
                name: "tb_pedido_itens");

            migrationBuilder.DropTable(
                name: "tb_clientes");

            migrationBuilder.DropTable(
                name: "tb_notas_fiscais");

            migrationBuilder.DropTable(
                name: "tb_adicionais");

            migrationBuilder.DropTable(
                name: "tb_itens");

            migrationBuilder.DropTable(
                name: "tb_pedidos");

            migrationBuilder.DropTable(
                name: "tb_formas_pagamento");

            migrationBuilder.DropTable(
                name: "tb_comandas");

            migrationBuilder.DropTable(
                name: "tb_funcionarios");
        }
    }
}
