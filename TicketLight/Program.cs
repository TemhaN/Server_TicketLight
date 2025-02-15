using Microsoft.EntityFrameworkCore;
using TicketLightAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// ����������� � ��
builder.Services.AddDbContext<TicketLightContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� ������������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
