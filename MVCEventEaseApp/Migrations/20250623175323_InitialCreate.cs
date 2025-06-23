using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCEventEaseApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Locations = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueID);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VenueID = table.Column<int>(type: "int", nullable: false),
                    EventtypeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_Events_EventTypes_EventtypeID",
                        column: x => x.EventtypeID,
                        principalTable: "EventTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Venues_VenueID",
                        column: x => x.VenueID,
                        principalTable: "Venues",
                        principalColumn: "VenueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    VenueID = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Bookings_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Venues_VenueID",
                        column: x => x.VenueID,
                        principalTable: "Venues",
                        principalColumn: "VenueID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Conference" },
                    { 2, "Wedding" },
                    { 3, "Expo" },
                    { 4, "Workshop" }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "VenueID", "Capacity", "ImageURL", "IsAvailable", "Locations", "VenueName" },
                values: new object[,]
                {
                    { 1, "500", "https://example.com/images/grandhall.jpg", false, "123 Main St, Cityville", "Grand Hall" },
                    { 2, "300", "https://example.com/images/cityconference.jpg", false, "456 Elm St, Cityville", "City Conference Center" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventID", "Description", "EventDate", "EventName", "EventtypeID", "VenueID" },
                values: new object[,]
                {
                    { 1, "A conference for tech enthusiasts.", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tech Conference 2023", null, 1 },
                    { 2, "A summit for business leaders.", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Summit 2023", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingID", "BookingDate", "EventID", "VenueID" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EventID",
                table: "Bookings",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_VenueID",
                table: "Bookings",
                column: "VenueID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventtypeID",
                table: "Events",
                column: "EventtypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueID",
                table: "Events",
                column: "VenueID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
