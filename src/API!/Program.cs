using Polly.RecilencyPatterns.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// API1 API2 ye recilency patternler üzerinden request atacak

builder.Services.AddHttpClient("api2", config =>
{
    config.BaseAddress = new Uri("https://localhost:5004");
    //Dýþ servisler ile çalýþýrken haberleþme yöntemimiz bu olmalý,
    ////Mesaj kuyruk sistemleri ile haberleþme sadece Internel olarak mantýklý
    /// Genel olarak https://localhost:5005 URI ise APIGetway URI. www.a.com
    /*
     * Mesaj Kuyruk sistemi üzerinden RequesstClient ile haabeleþmenin avantajlarý
     * 1. Protocol veya Port bilgisinden izole oluyoruz
     * 2. Dinamik olarak IP port deðiþimi gibi durumlardan izoleyiz
     * 3. Hata yönetimi veyaRecilency gibi kavramlarý MassTransit gibi paketlere býrakýyoruz
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
