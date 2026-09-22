using System;
using TrabalhoN1;

Corpo corpo1 = new Corpo(
    "Corpo 1",
    1000,
    1000,
    0,
    0,
    1,
    0
);

Corpo corpo2 = new Corpo(
    "Corpo 2",
    2000,
    2000,
    10,
    0,
    -1,
    0
);

Universo universo = new Universo();

universo.AdicionarCorpo(corpo1);
universo.AdicionarCorpo(corpo2);

universo.QuantidadeIteracoes = 10;
universo.TempoEntreIteracoes = 1;

GravadorArquivoTexto gravador =
    new GravadorArquivoTexto();

gravador.Salvar(
    universo,
    "universo.txt"
);

Console.WriteLine("Universo salvo com sucesso.");

Universo universoCarregado =
    gravador.Carregar("universo.txt");

Console.WriteLine(
    $"Iterações carregadas: " +
    $"{universoCarregado.QuantidadeIteracoes}");

Console.WriteLine(
    $"Tempo entre iterações carregado: " +
    $"{universoCarregado.TempoEntreIteracoes}");

Console.WriteLine(
    $"Quantidade de corpos carregados: " +
    $"{universoCarregado.Corpos.Count}");

foreach (Corpo corpo in universoCarregado.Corpos)
{
    Console.WriteLine(
        $"{corpo.Nome} - " +
        $"Massa: {corpo.Massa} - " +
        $"Densidade: {corpo.Densidade} - " +
        $"Posição: ({corpo.PosX}, {corpo.PosY}) - " +
        $"Velocidade: ({corpo.VelX}, {corpo.VelY})"
    );
}