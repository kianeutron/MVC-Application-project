using Someren_Case.Interfaces;
using Someren_Case.Repositories;

var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;


builder.Services.AddControllersWithViews();


var connectionString = configuration.GetConnectionString("dbproject242504");


builder.Services.AddScoped<IStudentRepository>(provider => new DbStudentRepository(connectionString));

builder.Services.AddScoped<IOrderRepository>(provider => new DbOrderRepository(connectionString));

builder.Services.AddScoped<IDrinkRepository, DbDrinkRepository>();
builder.Services.AddScoped<ILecturerRepository>(sp => new DbLecturerRepository(connectionString));

builder.Services.AddScoped<IRoomRepository, DbRoomRepository>();


builder.Services.AddScoped<IActivityParticipantRepository>(provider =>
    new DbActivityParticipantRepository(connectionString));


builder.Services.AddScoped<IActivityRepository>(provider => new DbActivityRepository(connectionString));

var app = builder.Build();


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
    "default",
    "{controller=Home}/{action=Index}/{id?}"
);

app.Run();