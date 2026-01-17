using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace visualAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixPessoaContaOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conta_Pessoas_PessoaId",
                table: "Conta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Conta",
                table: "Conta");

            migrationBuilder.DropColumn(
                name: "ContaId",
                table: "Pessoas");

            migrationBuilder.RenameTable(
                name: "Conta",
                newName: "Contas");

            migrationBuilder.RenameIndex(
                name: "IX_Conta_PessoaId",
                table: "Contas",
                newName: "IX_Contas_PessoaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contas",
                table: "Contas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_Pessoas_PessoaId",
                table: "Contas",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contas_Pessoas_PessoaId",
                table: "Contas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contas",
                table: "Contas");

            migrationBuilder.RenameTable(
                name: "Contas",
                newName: "Conta");

            migrationBuilder.RenameIndex(
                name: "IX_Contas_PessoaId",
                table: "Conta",
                newName: "IX_Conta_PessoaId");

            migrationBuilder.AddColumn<Guid>(
                name: "ContaId",
                table: "Pessoas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Conta",
                table: "Conta",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Conta_Pessoas_PessoaId",
                table: "Conta",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
