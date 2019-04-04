using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuimiosHub.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "collection_routes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_routes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    current_stock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    min_stock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    max_stock = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "samples",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha_grd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_recep = table.Column<DateTime>(type: "datetime2", nullable: true),
                    folio_grd = table.Column<int>(type: "int", nullable: true),
                    cliente_grd = table.Column<int>(type: "int", nullable: true),
                    paciente_grd = table.Column<int>(type: "int", nullable: true),
                    est_per_grd = table.Column<int>(type: "int", nullable: true),
                    label1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fec_cap_res = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fec_libera = table.Column<DateTime>(type: "datetime2", nullable: true),
                    suc_proc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    maquilador = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    label3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fec_nac = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_samples", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shifts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shifts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "route_stops",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collection_route_id = table.Column<int>(type: "int", nullable: false),
                    sequence_order = table.Column<int>(type: "int", nullable: false),
                    location_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    contact_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    contact_phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_stops", x => x.id);
                    table.ForeignKey(
                        name: "FK_route_stops_collection_routes_collection_route_id",
                        column: x => x.collection_route_id,
                        principalTable: "collection_routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory_movements",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inventory_item_id = table.Column<int>(type: "int", nullable: false),
                    movement_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    reference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    movement_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory_movements", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_movements_inventory_items_inventory_item_id",
                        column: x => x.inventory_item_id,
                        principalTable: "inventory_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collection_route_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    scheduled_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_schedules_collection_routes_collection_route_id",
                        column: x => x.collection_route_id,
                        principalTable: "collection_routes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_schedules_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shift_handovers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shift_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    handover_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    pending_samples_count = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shift_handovers", x => x.id);
                    table.ForeignKey(
                        name: "FK_shift_handovers_shifts_shift_id",
                        column: x => x.shift_id,
                        principalTable: "shifts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shift_handovers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "check_ins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    route_stop_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    check_in_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    latitude = table.Column<double>(type: "float", nullable: false),
                    longitude = table.Column<double>(type: "float", nullable: false),
                    accuracy = table.Column<double>(type: "float", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_ins", x => x.id);
                    table.ForeignKey(
                        name: "FK_check_ins_route_stops_route_stop_id",
                        column: x => x.route_stop_id,
                        principalTable: "route_stops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_check_ins_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pending_samples",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shift_handover_id = table.Column<int>(type: "int", nullable: false),
                    sample_id = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pending_samples", x => x.id);
                    table.ForeignKey(
                        name: "FK_pending_samples_samples_sample_id",
                        column: x => x.sample_id,
                        principalTable: "samples",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pending_samples_shift_handovers_shift_handover_id",
                        column: x => x.shift_handover_id,
                        principalTable: "shift_handovers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_route_stop_id",
                table: "check_ins",
                column: "route_stop_id");

            migrationBuilder.CreateIndex(
                name: "IX_check_ins_user_id",
                table: "check_ins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_code",
                table: "inventory_items",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_movements_inventory_item_id",
                table: "inventory_movements",
                column: "inventory_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_pending_samples_sample_id",
                table: "pending_samples",
                column: "sample_id");

            migrationBuilder.CreateIndex(
                name: "IX_pending_samples_shift_handover_id",
                table: "pending_samples",
                column: "shift_handover_id");

            migrationBuilder.CreateIndex(
                name: "IX_route_stops_collection_route_id",
                table: "route_stops",
                column: "collection_route_id");

            migrationBuilder.CreateIndex(
                name: "sample_unique_constraint",
                table: "samples",
                columns: new[] { "folio_grd", "cliente_grd", "fecha_recep" },
                unique: true,
                filter: "[folio_grd] IS NOT NULL AND [cliente_grd] IS NOT NULL AND [fecha_recep] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_collection_route_id",
                table: "schedules",
                column: "collection_route_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_user_id",
                table: "schedules",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_shift_handovers_shift_id",
                table: "shift_handovers",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_shift_handovers_user_id",
                table: "shift_handovers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "check_ins");

            migrationBuilder.DropTable(
                name: "inventory_movements");

            migrationBuilder.DropTable(
                name: "pending_samples");

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "route_stops");

            migrationBuilder.DropTable(
                name: "inventory_items");

            migrationBuilder.DropTable(
                name: "samples");

            migrationBuilder.DropTable(
                name: "shift_handovers");

            migrationBuilder.DropTable(
                name: "collection_routes");

            migrationBuilder.DropTable(
                name: "shifts");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
