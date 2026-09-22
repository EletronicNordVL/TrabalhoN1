using System;
using TrabalhoN1;

Console.WriteLine("SIMULADOR GRAVITACIONAL 2D");
Console.WriteLine();

Console.WriteLine("1 - Criar novo universo");
Console.WriteLine("2 - Carregar universo salvo");
Console.WriteLine("0 - Sair");

Console.WriteLine();
Console.Write("Escolha uma opção: ");

string? opcao = Console.ReadLine();

Console.WriteLine();

switch (opcao)
{
    case "1":
        Console.Write("Digite a quantidade de corpos: ");

        int quantidadeCorpos =
            int.Parse(Console.ReadLine()!);

        Universo universo = new Universo();

        universo.GerarCorposAleatorios(quantidadeCorpos);

        Console.Write("Digite a quantidade de iterações: ");
        universo.QuantidadeIteracoes =
            int.Parse(Console.ReadLine()!);

        Console.Write("Digite o tempo entre as iterações (em segundos): ");
        universo.TempoEntreIteracoes =
            double.Parse(Console.ReadLine()!);

        Console.WriteLine();
        Console.WriteLine("Corpos gerados:");
        Console.WriteLine();

        universo.ExibirEstado();

        GravadorArquivoTexto gravador =
            new GravadorArquivoTexto();

        gravador.Salvar(
            universo,
            "universo.txt"
        );

        Console.WriteLine();
        Console.WriteLine(
            "Configuração inicial salva em universo.txt."
        );

        Console.WriteLine();
        Console.WriteLine("Iniciando simulação...");
        Console.WriteLine();

        universo.ExecutarSimulacao(
            universo.QuantidadeIteracoes,
            universo.TempoEntreIteracoes
        );

        break;

    case "2":
        GravadorArquivoTexto gravadorCarregar =
        new GravadorArquivoTexto();

        Universo universoCarregado =
            gravadorCarregar.Carregar("universo.txt");

        Console.WriteLine("Universo carregado com sucesso.");
        Console.WriteLine();

        Console.WriteLine(
            $"Quantidade de corpos: {universoCarregado.Corpos.Count}");

        Console.WriteLine(
            $"Quantidade de iterações: {universoCarregado.QuantidadeIteracoes}");

        Console.WriteLine(
            $"Tempo entre iterações: {universoCarregado.TempoEntreIteracoes}");

        Console.WriteLine();
        Console.WriteLine("Corpos carregados:");
        Console.WriteLine();

        universoCarregado.ExibirEstado();

        Console.WriteLine();
        Console.WriteLine("Continuando simulação...");
        Console.WriteLine();

        universoCarregado.ExecutarSimulacao(
            universoCarregado.QuantidadeIteracoes,
            universoCarregado.TempoEntreIteracoes
        );

        break;

    case "0":
        Console.WriteLine("Programa encerrado.");
        break;

    default:
        Console.WriteLine("Opção inválida.");
        break;
}