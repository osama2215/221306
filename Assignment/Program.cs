using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Assignment.Data;
using Microsoft.EntityFrameworkCore.Migrations;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AssignmentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AssignmentContext") ?? throw new InvalidOperationException("Connection string 'AssignmentContext' not found.")));
// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AssignmentContext>();
    // apply pending migrations at startup so table exists
    
    SeedData.Initialize(services, db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

public partial class UpdateStudentId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // 1) add new column with identity
        migrationBuilder.AddColumn<int>(
            name: "Id_new",
            table: "Students",
            type: "int",
            nullable: false,
            defaultValue: 0)
            .Annotation("SqlServer:Identity", "1, 1");

        // 2) copy existing ids
        migrationBuilder.Sql("SET IDENTITY_INSERT dbo.Students ON; UPDATE dbo.Students SET Id_new = Id; SET IDENTITY_INSERT dbo.Students OFF;");

        // 3) drop PK and old column, rename new column and re-create PK
        migrationBuilder.DropPrimaryKey("PK_Students", "Students");
        migrationBuilder.DropColumn(name: "Id", table: "Students");
        migrationBuilder.RenameColumn(name: "Id_new", table: "Students", newName: "Id");
        migrationBuilder.AddPrimaryKey("PK_Students", "Students", "Id");
        // Recreate FKs/indexes as needed
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Implement reverse migration logic if needed
    }
}