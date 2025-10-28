using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDFUAWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedCertificatePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "AspNetUsers",
                newName: "CertificatePassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CertificatePassword",
                table: "AspNetUsers",
                newName: "Password");
        }
    }
}
