using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<EnderecoService>();
builder.Services.AddScoped<ItemCardapioService>();
builder.Services.AddScoped<SugestaoChefeService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<AtendimentoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<RelatorioService>();
builder.Services.AddScoped<MesaService>();
builder.Services.AddScoped<IngredienteService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key não configurado.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Administrador"));
});

builder.Services.AddControllers();

// Swagger com suporte a JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gestão de Restaurante API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT no header. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

var app = builder.Build();

await GarantirBancoDeDadosAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS
app.UseCors("ReactPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task GarantirBancoDeDadosAsync(WebApplication app)
{
    await using var scope = app.Services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (app.Environment.IsDevelopment())
    {
        await context.Database.MigrateAsync();
    }

    const string sqlCompatibilidadeCardapio = """
        DECLARE @temPlural bit =
            CASE
                WHEN OBJECT_ID(N'dbo.ItensCardapio', N'U') IS NOT NULL THEN 1
                WHEN OBJECT_ID(N'dbo.ItensCardapio', N'V') IS NOT NULL THEN 1
                WHEN OBJECT_ID(N'dbo.ItensCardapio', N'SN') IS NOT NULL THEN 1
                ELSE 0
            END;

        DECLARE @temSingular bit =
            CASE
                WHEN OBJECT_ID(N'dbo.ItemCardapio', N'U') IS NOT NULL THEN 1
                WHEN OBJECT_ID(N'dbo.ItemCardapio', N'V') IS NOT NULL THEN 1
                WHEN OBJECT_ID(N'dbo.ItemCardapio', N'SN') IS NOT NULL THEN 1
                ELSE 0
            END;

        IF @temPlural = 1 AND @temSingular = 0
            EXEC(N'CREATE SYNONYM dbo.ItemCardapio FOR dbo.ItensCardapio;');
        ELSE IF @temSingular = 1 AND @temPlural = 0
            EXEC(N'CREATE SYNONYM dbo.ItensCardapio FOR dbo.ItemCardapio;');
        ELSE IF @temPlural = 0 AND @temSingular = 0
            THROW 51000, N'Nenhuma tabela de cardapio foi encontrada. Rode as migrations ou recrie o banco.', 1;
        """;

    await context.Database.ExecuteSqlRawAsync(sqlCompatibilidadeCardapio);
    await GarantirCompatibilidadeAtendimentosAsync(context);
    await ExecutarSeedDesenvolvimentoAsync(app, context);
}

static async Task GarantirCompatibilidadeAtendimentosAsync(AppDbContext context)
{
    const string sqlCompatibilidadeAtendimentos = """
        IF OBJECT_ID(N'dbo.Atendimentos', N'U') IS NULL
            RETURN;

        IF COL_LENGTH(N'dbo.Atendimentos', N'Discriminator') IS NULL
            ALTER TABLE dbo.Atendimentos
            ADD Discriminator nvarchar(80) NOT NULL
                CONSTRAINT DF_Atendimentos_Discriminator DEFAULT N'Atendimento';

        IF COL_LENGTH(N'dbo.Atendimentos', N'ObservacaoEntrega') IS NULL
            ALTER TABLE dbo.Atendimentos
            ADD ObservacaoEntrega nvarchar(max) NULL;

        IF COL_LENGTH(N'dbo.Atendimentos', N'NomeAplicativo') IS NULL
            ALTER TABLE dbo.Atendimentos
            ADD NomeAplicativo nvarchar(80) NULL;

        EXEC(N'
        UPDATE dbo.Atendimentos
        SET Discriminator =
            CASE TipoAtendimento
                WHEN 1 THEN N''AtendimentoPresencial''
                WHEN 2 THEN N''AtendimentoDeliveryProprio''
                WHEN 3 THEN N''AtendimentoDeliveryAplicativo''
                ELSE N''Atendimento''
            END
        WHERE Discriminator IS NULL
           OR LTRIM(RTRIM(Discriminator)) = N''''
           OR Discriminator = N''Atendimento'';
        ');
        """;

    await context.Database.ExecuteSqlRawAsync(sqlCompatibilidadeAtendimentos);
}

static async Task ExecutarSeedDesenvolvimentoAsync(WebApplication app, AppDbContext context)
{
    if (!app.Environment.IsDevelopment())
        return;

    var seedPath = ObterCaminhoSeed(app);
    if (seedPath == null)
        return;

    var script = await File.ReadAllTextAsync(seedPath);
    script = Regex.Replace(
        script,
        @"^\s*USE\s+[^\r\n;]+;?\s*$",
        string.Empty,
        RegexOptions.Multiline | RegexOptions.IgnoreCase);

    var batches = Regex.Split(
        script,
        @"^\s*GO\s*$",
        RegexOptions.Multiline | RegexOptions.IgnoreCase);

    foreach (var batch in batches.Select(b => b.Trim()).Where(b => !string.IsNullOrWhiteSpace(b)))
    {
        var tabelaInsert = ExtrairTabelaInsert(batch);
        if (tabelaInsert != null && tabelaInsert.Equals("ItemIngredientes", StringComparison.OrdinalIgnoreCase))
        {
            await ExecutarSeedItemIngredientesAsync(context, batch);
            continue;
        }

        if (tabelaInsert != null && await TabelaTemRegistrosAsync(context, tabelaInsert))
            continue;

        if (tabelaInsert != null && !await DependenciasDoSeedExistemAsync(context, tabelaInsert))
        {
            app.Logger.LogWarning(
                "Seed da tabela {Tabela} ignorado porque os registros relacionados esperados nao existem.",
                tabelaInsert);
            continue;
        }

        await context.Database.ExecuteSqlRawAsync(batch);
    }
}

static string? ObterCaminhoSeed(WebApplication app)
{
    var candidatos = new[]
    {
        Path.Combine(app.Environment.ContentRootPath, "seed.sql"),
        Path.Combine(Directory.GetParent(app.Environment.ContentRootPath)?.FullName ?? string.Empty, "seed.sql"),
        Path.Combine(Directory.GetCurrentDirectory(), "seed.sql")
    };

    return candidatos.FirstOrDefault(File.Exists);
}

static string? ExtrairTabelaInsert(string batch)
{
    var match = Regex.Match(
        batch,
        @"INSERT\s+INTO\s+(?:\[[^\]]+\]\.)?(?:\[?(?<table>[A-Za-z0-9_]+)\]?)",
        RegexOptions.IgnoreCase);

    return match.Success ? match.Groups["table"].Value : null;
}

static async Task ExecutarSeedItemIngredientesAsync(AppDbContext context, string batch)
{
    var pares = ExtrairParesItemIngredientes(batch);
    if (pares.Count == 0)
        return;

    var values = string.Join(
        $",{Environment.NewLine}",
        pares.Select(par => $"({par.ItemCardapioId}, {par.IngredienteId})"));

    var sql = $"""
        INSERT INTO ItemIngredientes (ItemCardapioId, IngredienteId)
        SELECT dados.ItemCardapioId, dados.IngredienteId
        FROM (VALUES
        {values}
        ) AS dados(ItemCardapioId, IngredienteId)
        WHERE EXISTS (
            SELECT 1
            FROM ItensCardapio item
            WHERE item.Id = dados.ItemCardapioId
        )
        AND EXISTS (
            SELECT 1
            FROM Ingredientes ingrediente
            WHERE ingrediente.Id = dados.IngredienteId
        )
        AND NOT EXISTS (
            SELECT 1
            FROM ItemIngredientes itemIngrediente
            WHERE itemIngrediente.ItemCardapioId = dados.ItemCardapioId
              AND itemIngrediente.IngredienteId = dados.IngredienteId
        );
        """;

    await context.Database.ExecuteSqlRawAsync(sql);
}

static IReadOnlyList<(int ItemCardapioId, int IngredienteId)> ExtrairParesItemIngredientes(string batch)
{
    var indiceValues = batch.IndexOf("VALUES", StringComparison.OrdinalIgnoreCase);
    var conteudoValues = indiceValues >= 0 ? batch[(indiceValues + "VALUES".Length)..] : batch;

    return Regex.Matches(conteudoValues, @"\(\s*(?<item>\d+)\s*,\s*(?<ingrediente>\d+)\s*\)")
        .Select(match => (
            ItemCardapioId: int.Parse(match.Groups["item"].Value),
            IngredienteId: int.Parse(match.Groups["ingrediente"].Value)))
        .Distinct()
        .ToArray();
}

static async Task<bool> DependenciasDoSeedExistemAsync(AppDbContext context, string tableName)
{
    return tableName switch
    {
        "Enderecos" => await TodosIdsExistemAsync(context, "Usuarios", new[] { 2, 3, 4, 5 }),
        "SugestoesChefe" => await TodosIdsExistemAsync(context, "ItensCardapio", new[] { 1, 2, 3, 4, 5, 21, 22, 23, 24, 25, 26 }),
        "Reservas" => await TodosIdsExistemAsync(context, "Usuarios", new[] { 2, 3, 4, 5 })
            && await TodosIdsExistemAsync(context, "Mesas", Enumerable.Range(1, 10).ToArray()),
        "Pedidos" => await TodosIdsExistemAsync(context, "Usuarios", new[] { 2, 3, 4, 5 })
            && await TodosIdsExistemAsync(context, "Atendimentos", Enumerable.Range(1, 10).ToArray()),
        "ItensPedidos" => await TodosIdsExistemAsync(context, "Pedidos", Enumerable.Range(1, 10).ToArray())
            && await TodosIdsExistemAsync(context, "ItensCardapio", new[] { 1, 2, 4, 5, 21, 22, 23, 24 }),
        _ => true
    };
}

static async Task<bool> TodosIdsExistemAsync(AppDbContext context, string tableName, IReadOnlyCollection<int> ids)
{
    var idsUnicos = ids.Distinct().ToArray();
    if (idsUnicos.Length == 0)
        return true;

    if (!TabelaSeedValida(tableName))
        return false;

    var idsSql = string.Join(", ", idsUnicos);
    var sql = $"SELECT COUNT(*) FROM [{tableName}] WHERE [Id] IN ({idsSql})";
    var connection = context.Database.GetDbConnection();

    if (connection.State != System.Data.ConnectionState.Open)
        await connection.OpenAsync();

    await using var command = connection.CreateCommand();
    command.CommandText = sql;
    var result = await command.ExecuteScalarAsync();

    return Convert.ToInt32(result) == idsUnicos.Length;
}

static async Task<bool> TabelaTemRegistrosAsync(AppDbContext context, string tableName)
{
    if (!TabelaSeedValida(tableName))
        return false;

    var sql = $"SELECT CASE WHEN EXISTS (SELECT 1 FROM [{tableName}]) THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END";
    var connection = context.Database.GetDbConnection();

    if (connection.State != System.Data.ConnectionState.Open)
        await connection.OpenAsync();

    await using var command = connection.CreateCommand();
    command.CommandText = sql;
    var result = await command.ExecuteScalarAsync();

    return result is bool hasRows && hasRows;
}

static bool TabelaSeedValida(string tableName)
{
    var tableNamesValidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Usuarios",
        "Enderecos",
        "Mesas",
        "Ingredientes",
        "ItensCardapio",
        "ItemIngredientes",
        "SugestoesChefe",
        "Reservas",
        "Atendimentos",
        "Pedidos",
        "ItensPedidos",
        "ConfiguracoesRestaurante"
    };

    return tableNamesValidos.Contains(tableName);
}
