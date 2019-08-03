using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuimiOSHub.Migrations
{
    /// <inheritdoc />
    public partial class RenameSampleColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_samples_fecha_recep",
                table: "samples");

            migrationBuilder.DropIndex(
                name: "sample_unique_constraint",
                table: "samples");

            migrationBuilder.DropColumn(
                name: "fec_cap_res",
                table: "samples");

            migrationBuilder.RenameColumn(
                name: "suc_proc",
                table: "samples",
                newName: "priority");

            migrationBuilder.RenameColumn(
                name: "paciente_grd",
                table: "samples",
                newName: "patient_id");

            migrationBuilder.RenameColumn(
                name: "maquilador",
                table: "samples",
                newName: "outsourcer");

            migrationBuilder.RenameColumn(
                name: "label3",
                table: "samples",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "label1",
                table: "samples",
                newName: "exam_name");

            migrationBuilder.RenameColumn(
                name: "folio_grd",
                table: "samples",
                newName: "folio");

            migrationBuilder.RenameColumn(
                name: "fecha_recep",
                table: "samples",
                newName: "validated_at");

            migrationBuilder.RenameColumn(
                name: "fecha_grd",
                table: "samples",
                newName: "received_at");

            migrationBuilder.RenameColumn(
                name: "fec_nac",
                table: "samples",
                newName: "processed_at");

            migrationBuilder.RenameColumn(
                name: "fec_libera",
                table: "samples",
                newName: "birth_date");

            migrationBuilder.RenameColumn(
                name: "est_per_grd",
                table: "samples",
                newName: "exam_id");

            migrationBuilder.RenameColumn(
                name: "cliente_grd",
                table: "samples",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_samples_cliente_grd",
                table: "samples",
                newName: "IX_samples_client_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "samples",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_samples_received_at",
                table: "samples",
                column: "received_at");

            migrationBuilder.CreateIndex(
                name: "sample_unique_constraint",
                table: "samples",
                columns: new[] { "folio", "client_id", "received_at" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_samples_received_at",
                table: "samples");

            migrationBuilder.DropIndex(
                name: "sample_unique_constraint",
                table: "samples");

            migrationBuilder.RenameColumn(
                name: "validated_at",
                table: "samples",
                newName: "fecha_recep");

            migrationBuilder.RenameColumn(
                name: "received_at",
                table: "samples",
                newName: "fecha_grd");

            migrationBuilder.RenameColumn(
                name: "processed_at",
                table: "samples",
                newName: "fec_nac");

            migrationBuilder.RenameColumn(
                name: "priority",
                table: "samples",
                newName: "suc_proc");

            migrationBuilder.RenameColumn(
                name: "patient_id",
                table: "samples",
                newName: "paciente_grd");

            migrationBuilder.RenameColumn(
                name: "outsourcer",
                table: "samples",
                newName: "maquilador");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "samples",
                newName: "label3");

            migrationBuilder.RenameColumn(
                name: "folio",
                table: "samples",
                newName: "folio_grd");

            migrationBuilder.RenameColumn(
                name: "exam_name",
                table: "samples",
                newName: "label1");

            migrationBuilder.RenameColumn(
                name: "exam_id",
                table: "samples",
                newName: "est_per_grd");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "samples",
                newName: "cliente_grd");

            migrationBuilder.RenameColumn(
                name: "birth_date",
                table: "samples",
                newName: "fec_libera");

            migrationBuilder.RenameIndex(
                name: "IX_samples_client_id",
                table: "samples",
                newName: "IX_samples_cliente_grd");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "samples",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "fec_cap_res",
                table: "samples",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_samples_fecha_recep",
                table: "samples",
                column: "fecha_recep");

            migrationBuilder.CreateIndex(
                name: "sample_unique_constraint",
                table: "samples",
                columns: new[] { "folio_grd", "cliente_grd", "fecha_recep" },
                unique: true);
        }
    }
}
