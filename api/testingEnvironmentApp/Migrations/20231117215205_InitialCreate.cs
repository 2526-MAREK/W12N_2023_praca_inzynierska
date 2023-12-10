using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testingEnvironmentApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hubs",
                columns: table => new
                {
                    IdHub = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HubIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hubs", x => x.IdHub);
                });

            migrationBuilder.CreateTable(
                name: "MsrtPoints",
                columns: table => new
                {
                    IdPoint = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MsrtPointIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Factor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsrtPoints", x => x.IdPoint);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    IdDevice = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdHub = table.Column<int>(type: "int", nullable: false),
                    DeviceIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.IdDevice);
                    table.ForeignKey(
                        name: "FK_Devices_Hubs_IdHub",
                        column: x => x.IdHub,
                        principalTable: "Hubs",
                        principalColumn: "IdHub",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    IdChannel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDevice = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChannelIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Factor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.IdChannel);
                    table.ForeignKey(
                        name: "FK_Channels_Devices_IdDevice",
                        column: x => x.IdDevice,
                        principalTable: "Devices",
                        principalColumn: "IdDevice",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alarms",
                columns: table => new
                {
                    IdAlarm = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdChannel = table.Column<int>(type: "int", nullable: false),
                    ChannelIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurrenceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PossibleFault = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarms", x => x.IdAlarm);
                    table.ForeignKey(
                        name: "FK_Alarms_Channels_IdChannel",
                        column: x => x.IdChannel,
                        principalTable: "Channels",
                        principalColumn: "IdChannel",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Msrts",
                columns: table => new
                {
                    IdHubs = table.Column<int>(type: "int", nullable: false),
                    IdDevice = table.Column<int>(type: "int", nullable: false),
                    IdChannels = table.Column<int>(type: "int", nullable: false),
                    DataTimeMs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeZone = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    MsrtValue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Msrts", x => new { x.IdHubs, x.IdDevice, x.IdChannels, x.DataTimeMs, x.DateTimeZone });
                    table.ForeignKey(
                        name: "FK_Msrts_Channels_IdChannels",
                        column: x => x.IdChannels,
                        principalTable: "Channels",
                        principalColumn: "IdChannel",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Msrts_Devices_IdDevice",
                        column: x => x.IdDevice,
                        principalTable: "Devices",
                        principalColumn: "IdDevice",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Msrts_Hubs_IdHubs",
                        column: x => x.IdHubs,
                        principalTable: "Hubs",
                        principalColumn: "IdHub",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MsrtAssociations",
                columns: table => new
                {
                    IdHubs = table.Column<int>(type: "int", nullable: false),
                    IdDevice = table.Column<int>(type: "int", nullable: false),
                    IdChannels = table.Column<int>(type: "int", nullable: false),
                    DataTimeMs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeZone = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IdAssociation = table.Column<int>(type: "int", nullable: false),
                    IdPoint = table.Column<int>(type: "int", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MsrtAssociations", x => new { x.IdHubs, x.IdDevice, x.IdChannels, x.DataTimeMs, x.DateTimeZone });
                    table.ForeignKey(
                        name: "FK_MsrtAssociations_MsrtPoints_IdPoint",
                        column: x => x.IdPoint,
                        principalTable: "MsrtPoints",
                        principalColumn: "IdPoint",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MsrtAssociations_Msrts_IdHubs_IdDevice_IdChannels_DataTimeMs_DateTimeZone",
                        columns: x => new { x.IdHubs, x.IdDevice, x.IdChannels, x.DataTimeMs, x.DateTimeZone },
                        principalTable: "Msrts",
                        principalColumns: new[] { "IdHubs", "IdDevice", "IdChannels", "DataTimeMs", "DateTimeZone" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alarms_IdChannel",
                table: "Alarms",
                column: "IdChannel");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ChannelIdentifier",
                table: "Channels",
                column: "ChannelIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_IdDevice",
                table: "Channels",
                column: "IdDevice");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceIdentifier",
                table: "Devices",
                column: "DeviceIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_IdHub",
                table: "Devices",
                column: "IdHub");

            migrationBuilder.CreateIndex(
                name: "IX_Hubs_HubIdentifier",
                table: "Hubs",
                column: "HubIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MsrtAssociations_IdPoint",
                table: "MsrtAssociations",
                column: "IdPoint");

            migrationBuilder.CreateIndex(
                name: "IX_MsrtPoints_MsrtPointIdentifier",
                table: "MsrtPoints",
                column: "MsrtPointIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Msrts_IdChannels",
                table: "Msrts",
                column: "IdChannels");

            migrationBuilder.CreateIndex(
                name: "IX_Msrts_IdDevice",
                table: "Msrts",
                column: "IdDevice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.DropTable(
                name: "MsrtAssociations");

            migrationBuilder.DropTable(
                name: "MsrtPoints");

            migrationBuilder.DropTable(
                name: "Msrts");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Hubs");
        }
    }
}
