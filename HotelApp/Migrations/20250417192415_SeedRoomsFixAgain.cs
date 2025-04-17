using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoomsFixAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 2, 151m, "01", "Double" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 1, 101m, "02", "Single" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 1, 101m, "03", "Single" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 4, 251m, "04", "Suite" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 1, 101m, "101", "Single" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 2, 151m, "102", "Double" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 4, 251m, "103", "Suite" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Capacity", "PricePerNight", "RoomNumber", "RoomType" },
                values: new object[] { 1, 101m, "104", "Single" });
        }
    }
}
