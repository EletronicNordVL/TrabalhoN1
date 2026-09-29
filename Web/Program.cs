using System.Globalization;
using TrabalhoN1;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5080");
builder.WebHost.UseWebRoot(Path.Combine(builder.Environment.ContentRootPath, "wwwroot"));
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.PropertyNameCaseInsensitive = true);
var app = builder.Build();
var storage = Path.GetFullPath(builder.Configuration["UniverseStoragePath"]
    ?? Path.Combine(builder.Environment.ContentRootPath, ".."));
var recorder = new GravadorArquivoTexto();
var fileLock = new object();

app.Use(async (context, next) =>
{
    try { await next(context); }
    catch (Exception ex) when (ex is ArgumentException or FormatException or OverflowException or InvalidDataException or IndexOutOfRangeException)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (IOException)
    {
        context.Response.StatusCode = 409;
        await context.Response.WriteAsJsonAsync(new { error = "Não foi possível acessar o arquivo. Tente novamente." });
    }
});
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/health", (HttpResponse response) =>
{
    response.Headers.CacheControl = "no-store";
    return new { status = "ok", application = "TrabalhoN1", storagePath = storage, processId = Environment.ProcessId };
});

app.MapPost("/api/universes/create", (CreateRequest request) =>
{
    if (request.Count < 1 || request.Count > 100) throw new ArgumentException("Escolha entre 1 e 100 corpos.");
    var universe = new Universo { QuantidadeIteracoes = 10000, TempoEntreIteracoes = 3600 };
    universe.GerarCorposAleatorios(request.Count);
    universe.CalcularForcas();
    return Snapshot.From(universe, 0, 0);
});

app.MapPost("/api/simulation/step", (StepRequest request) =>
{
    ArgumentNullException.ThrowIfNull(request.State);
    var universe = request.State.ToUniverse();
    if (request.Steps < 1 || request.Steps > 100) throw new ArgumentException("Quantidade de passos inválida.");
    var steps = Math.Min(request.Steps, universe.QuantidadeIteracoes - request.State.Iteration);
    for (var i = 0; i < steps; i++)
    {
        universe.CalcularForcas();
        universe.AtualizarPosicoes(universe.TempoEntreIteracoes);
        universe.TratarColisoes();
        ValidateBodies(universe.Corpos);
    }
    universe.CalcularForcas();
    return Snapshot.From(universe, request.State.Iteration + steps,
        request.State.Elapsed + steps * universe.TempoEntreIteracoes);
});

app.MapGet("/api/universes", () => Directory.GetFiles(storage, "universo_*.txt")
    .Select(path => new { name = Path.GetFileName(path), modified = File.GetLastWriteTimeUtc(path), size = new FileInfo(path).Length })
    .OrderByDescending(file => file.modified));

app.MapPost("/api/universes/save", (Snapshot state) =>
{
    var universe = state.ToUniverse();
    lock (fileLock)
    {
        var number = 1;
        while (File.Exists(Path.Combine(storage, $"universo_{number}.txt"))) number++;
        var name = $"universo_{number}.txt";
        recorder.Salvar(universe, Path.Combine(storage, name));
        return Results.Ok(new { name });
    }
});
app.MapGet("/api/universes/{name}", (string name) =>
{
    var path = SafePath(name);
    if (!File.Exists(path)) return Results.NotFound(new { error = "Universo não encontrado." });
    var universe = recorder.Carregar(path);
    ValidateBodies(universe.Corpos);
    var state = Snapshot.From(universe, 0, 0);
    state.ToUniverse();
    return Results.Ok(state);
});
app.MapDelete("/api/universes/{name}", (string name) =>
{
    var path = SafePath(name);
    lock (fileLock)
    {
        if (!File.Exists(path)) return Results.NotFound(new { error = "Universo não encontrado." });
        File.Delete(path);
    }
    return Results.NoContent();
});
app.MapPost("/api/universes/export", (Snapshot state) =>
{
    var universe = state.ToUniverse();
    var lines = new List<string> { $"{universe.Corpos.Count};{universe.QuantidadeIteracoes};{universe.TempoEntreIteracoes.ToString("R", CultureInfo.InvariantCulture)}" };
    lines.AddRange(universe.Corpos.Select(c => string.Join(';', c.Nome,
        c.Massa.ToString("R", CultureInfo.InvariantCulture), c.Densidade.ToString("R", CultureInfo.InvariantCulture),
        c.PosX.ToString("R", CultureInfo.InvariantCulture), c.PosY.ToString("R", CultureInfo.InvariantCulture),
        c.VelX.ToString("R", CultureInfo.InvariantCulture), c.VelY.ToString("R", CultureInfo.InvariantCulture))));
    return Results.File(System.Text.Encoding.UTF8.GetBytes(string.Join('\n', lines)), "text/plain; charset=utf-8", "universo_exportado.txt");
});
app.MapPost("/api/universes/import", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var content = await reader.ReadToEndAsync();
    if (content.Length > 100000) throw new ArgumentException("Arquivo muito grande (máximo 100 KB).");
    var lines = content.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);
    if (lines.Length < 2) throw new ArgumentException("Arquivo de universo inválido.");
    var header = lines[0].Trim().Split(';');
    if (header.Length != 3) throw new ArgumentException("Cabeçalho inválido.");
    var count = int.Parse(header[0]);
    if (count < 1 || count > 100 || lines.Length != count + 1) throw new ArgumentException("Quantidade de corpos inválida no arquivo.");
    var state = new Snapshot([], int.Parse(header[1]), Parse(header[2]), 0, 0);
    foreach (var line in lines.Skip(1))
    {
        var fields = line.Trim().Split(';');
        if (fields.Length != 7) throw new ArgumentException("Cada corpo deve ter sete campos.");
        state.Corpos.Add(new Corpo(fields[0], Parse(fields[1]), Parse(fields[2]), Parse(fields[3]), Parse(fields[4]), Parse(fields[5]), Parse(fields[6])));
    }
    var universe = state.ToUniverse();
    universe.CalcularForcas();
    return Snapshot.From(universe, 0, 0);
});

app.Run();

double Parse(string value) => double.Parse(value.Trim().Replace(',', '.'), CultureInfo.InvariantCulture);
string SafePath(string name)
{
    if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^universo_[0-9]+\.txt$"))
        throw new ArgumentException("Nome de arquivo inválido.");
    return Path.Combine(storage, name);
}
static void ValidateBodies(List<Corpo> bodies) => Snapshot.ValidateBodies(bodies);

record CreateRequest(int Count);
record StepRequest(Snapshot State, int Steps);
record Snapshot(List<Corpo> Corpos, int QuantidadeIteracoes, double TempoEntreIteracoes, int Iteration, double Elapsed)
{
    public static Snapshot From(Universo u, int iteration, double elapsed)
    {
        ValidateBodies(u.Corpos);
        if (u.Corpos.Any(c => new[] { c.ForcaX, c.ForcaY, c.AceleracaoX, c.AceleracaoY }.Any(v => !double.IsFinite(v))))
            throw new ArgumentException("O cálculo excedeu a precisão numérica. Reduza o passo de tempo ou aumente a distância entre os corpos.");
        return new(u.Corpos, u.QuantidadeIteracoes, u.TempoEntreIteracoes, iteration, elapsed);
    }
    public Universo ToUniverse()
    {
        if (QuantidadeIteracoes < 1 || QuantidadeIteracoes > 1000000) throw new ArgumentException("Use de 1 a 1.000.000 iterações.");
        if (!double.IsFinite(TempoEntreIteracoes) || TempoEntreIteracoes < .001 || TempoEntreIteracoes > 86400)
            throw new ArgumentException("O passo de tempo deve estar entre 0,001 e 86.400 segundos.");
        if (Iteration < 0 || Iteration > QuantidadeIteracoes || !double.IsFinite(Elapsed) || Elapsed < 0)
            throw new ArgumentException("Estado da simulação inválido.");
        ValidateBodies(Corpos);
        var u = new Universo { QuantidadeIteracoes = QuantidadeIteracoes, TempoEntreIteracoes = TempoEntreIteracoes };
        foreach (var c in Corpos) u.AdicionarCorpo(c);
        return u;
    }
    public static void ValidateBodies(List<Corpo>? bodies)
    {
        if (bodies is null || bodies.Count < 1 || bodies.Count > 100) throw new ArgumentException("O universo deve ter entre 1 e 100 corpos.");
        foreach (var c in bodies)
        {
            if (c is null || string.IsNullOrWhiteSpace(c.Nome) || c.Nome.Length > 60 || c.Nome.IndexOfAny([';', '\r', '\n']) >= 0)
                throw new ArgumentException("Nome de corpo inválido (até 60 caracteres, sem ponto e vírgula).");
            if (!double.IsFinite(c.Massa) || c.Massa <= 0 || c.Massa > 1e35 || !double.IsFinite(c.Densidade) || c.Densidade <= 0 || c.Densidade > 1e18)
                throw new ArgumentException("Massa ou densidade fora do intervalo permitido.");
            if (!double.IsFinite(c.Raio) || c.Raio <= 0)
                throw new ArgumentException("A massa e a densidade produzem um raio fora do intervalo numérico.");
            if (new[] { c.PosX, c.PosY, c.VelX, c.VelY }.Any(v => !double.IsFinite(v) || Math.Abs(v) > 1e20))
                throw new ArgumentException("A simulação ultrapassou o intervalo numérico. Reduza o passo de tempo ou ajuste os corpos.");
        }
    }
}
