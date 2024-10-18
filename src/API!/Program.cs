using Polly.RecilencyPatterns.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// API1 API2 ye recilency patternler �zerinden request atacak

builder.Services.AddHttpClient("api2", config =>
{
    config.BaseAddress = new Uri("https://localhost:5004");
    //D�� servisler ile �al���rken haberle�me y�ntemimiz bu olmal�,
    ////Mesaj kuyruk sistemleri ile haberle�me sadece Internel olarak mant�kl�
    /// Genel olarak https://localhost:5005 URI ise APIGetway URI. www.a.com
    /*
     * Mesaj Kuyruk sistemi �zerinden RequesstClient ile haabele�menin avantajlar�
     * 1. Protocol veya Port bilgisinden izole oluyoruz
     * 2. Dinamik olarak IP port de�i�imi gibi durumlardan izoleyiz
     * 3. Hata y�netimi veyaRecilency gibi kavramlar� MassTransit gibi paketlere b�rak�yoruz
     */
}).AddPolicyHandler(HttpRecilencyHelper.CreateRetryPolicy(retryCount:3, sleepDuration:TimeSpan.FromSeconds(30)));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
