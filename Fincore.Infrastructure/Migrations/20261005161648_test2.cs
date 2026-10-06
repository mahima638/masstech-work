using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fincore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RFQs",
                columns: table => new
                {
                    RFQId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFQNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IndentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RFQLineNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqQty = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FactoryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDateofBid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFQs", x => x.RFQId);
                    table.ForeignKey(
                        name: "FK_RFQs_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "eid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RFQItems",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFQId = table.Column<int>(type: "int", nullable: false),
                    RFQLineNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReqQty = table.Column<int>(type: "int", nullable: false),
                    UOM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReqDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FactoryCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFQItems", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_RFQItems_RFQs_RFQId",
                        column: x => x.RFQId,
                        principalTable: "RFQs",
                        principalColumn: "RFQId");
                });

            migrationBuilder.CreateTable(
                name: "RFQQuotations",
                columns: table => new
                {
                    QuotationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotedAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RFQId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RFQQuotations", x => x.QuotationId);
                    table.ForeignKey(
                        name: "FK_RFQQuotations_RFQs_RFQId",
                        column: x => x.RFQId,
                        principalTable: "RFQs",
                        principalColumn: "RFQId");
                    table.ForeignKey(
                        name: "FK_RFQQuotations_user_VendorId",
                        column: x => x.VendorId,
                        principalTable: "user",
                        principalColumn: "eid");
                });

            migrationBuilder.CreateTable(
                name: "VendorRFQMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RFQId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorRFQMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorRFQMappings_RFQs_RFQId",
                        column: x => x.RFQId,
                        principalTable: "RFQs",
                        principalColumn: "RFQId");
                    table.ForeignKey(
                        name: "FK_VendorRFQMappings_user_VendorId",
                        column: x => x.VendorId,
                        principalTable: "user",
                        principalColumn: "eid");
                });

            migrationBuilder.CreateTable(
                name: "FinalizedQuotations",
                columns: table => new
                {
                    FinalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinalizedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RFQId = table.Column<int>(type: "int", nullable: false),
                    QuotationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalizedQuotations", x => x.FinalId);
                    table.ForeignKey(
                        name: "FK_FinalizedQuotations_RFQQuotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "RFQQuotations",
                        principalColumn: "QuotationId");
                    table.ForeignKey(
                        name: "FK_FinalizedQuotations_RFQs_RFQId",
                        column: x => x.RFQId,
                        principalTable: "RFQs",
                        principalColumn: "RFQId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalizedQuotations_QuotationId",
                table: "FinalizedQuotations",
                column: "QuotationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinalizedQuotations_RFQId",
                table: "FinalizedQuotations",
                column: "RFQId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RFQItems_RFQId",
                table: "RFQItems",
                column: "RFQId");

            migrationBuilder.CreateIndex(
                name: "IX_RFQQuotations_RFQId",
                table: "RFQQuotations",
                column: "RFQId");

            migrationBuilder.CreateIndex(
                name: "IX_RFQQuotations_VendorId",
                table: "RFQQuotations",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_RFQs_UserId",
                table: "RFQs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorRFQMappings_RFQId",
                table: "VendorRFQMappings",
                column: "RFQId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorRFQMappings_VendorId",
                table: "VendorRFQMappings",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalizedQuotations");

            migrationBuilder.DropTable(
                name: "RFQItems");

            migrationBuilder.DropTable(
                name: "VendorRFQMappings");

            migrationBuilder.DropTable(
                name: "RFQQuotations");

            migrationBuilder.DropTable(
                name: "RFQs");
        }
    }
}
