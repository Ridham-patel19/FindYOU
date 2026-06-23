using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FindYOU.Migrations
{
    /// <inheritdoc />
    public partial class AddSearchVectorMain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"
        ALTER TABLE ""ChatEntries""
        ADD COLUMN search_vector tsvector;
    ");

    migrationBuilder.Sql(@"
        CREATE INDEX idx_chatentries_search_vector
        ON ""ChatEntries""
        USING GIN(search_vector);
    ");
}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
