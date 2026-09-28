using System;
using System.Globalization;
using System.IO;
using TrabalhoN1;

bool continuar = true;

while (continuar)
{
    /* Apresentar o menu de opções para o usuário, permitindo criar um novo universo ou carregar um universo salvo. */
    Console.WriteLine("SIMULADOR GRAVITACIONAL 2D");
    Console.WriteLine();

    /* Exibir as opções do menu. */
    Console.WriteLine("1 - Criar novo universo");
    Console.WriteLine("2 - Carregar universo salvo");
    Console.WriteLine("3 - Apagar universo salvo");
    Console.WriteLine("0 - Sair do menu");

    Console.WriteLine();
    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    /* Limpar a tela após o usuário escolher uma opção do menu. */
    Console.Clear();

    /* Controlar se deve aguardar ENTER antes de voltar ao menu. */
    bool aguardarEnter = true;

    /* Executar a opção escolhida pelo usuário. */
    switch (opcao)
    {
        /* Criar um novo universo com base na quantidade de corpos, iterações e tempo entre iterações informados pelo usuário. */
        case "1":

            Console.WriteLine("Criar novo universo");
            Console.WriteLine();

            Console.Write("Digite a quantidade de corpos: ");

            int quantidadeCorpos =
                int.Parse(Console.ReadLine()!);

            Universo universo = new Universo();

            /* Gerar corpos aleatórios com base na quantidade informada pelo usuário. */
            universo.GerarCorposAleatorios(quantidadeCorpos);

            Console.Write("Digite a quantidade de iterações: ");

            universo.QuantidadeIteracoes =
                int.Parse(Console.ReadLine()!);

            Console.Write(
                "Digite o tempo entre as iterações (em segundos): "
            );

            string entradaTempo =
                Console.ReadLine()!.Replace(',', '.');

            universo.TempoEntreIteracoes =
                double.Parse(
                    entradaTempo,
                    CultureInfo.InvariantCulture
                );

            Console.WriteLine();
            Console.WriteLine("Corpos gerados:");
            Console.WriteLine();

            universo.ExibirEstado();

            GravadorArquivoTexto gravador =
                new GravadorArquivoTexto();

            /* Gerar um nome de arquivo diferente para cada universo salvo, evitando sobrescrever os anteriores. */
            int numeroArquivo = 1;
            string caminhoArquivo;

            do
            {
                caminhoArquivo =
                    $"universo_{numeroArquivo}.txt";

                numeroArquivo++;
            }
            while (File.Exists(caminhoArquivo));

            gravador.Salvar(
                universo,
                caminhoArquivo
            );

            Console.WriteLine();

            Console.WriteLine(
                $"Configuração inicial salva em {caminhoArquivo}."
            );

            /* Serve para aguardar o usuário pressionar ENTER antes de iniciar a simulação. */
            Console.WriteLine();
            Console.WriteLine("Iniciando simulação...");
            Console.WriteLine();

            /* Executar a simulação do universo com base na quantidade de iterações e tempo entre iterações informados pelo usuário. */
            universo.ExecutarSimulacao(
                universo.QuantidadeIteracoes,
                universo.TempoEntreIteracoes
            );

            break;


        /* Carregar um universo salvo de um arquivo txt e continuar a simulação. */
        case "2":

            Console.WriteLine("Carregar universo salvo");
            Console.WriteLine();

            /* Buscar todos os arquivos de universos salvos. */
            string[] universosSalvos =
                Directory.GetFiles(
                    ".",
                    "universo_*.txt"
                );

            /* Verificar se existem universos salvos. */
            if (universosSalvos.Length == 0)
            {
                Console.WriteLine(
                    "Nenhum universo salvo encontrado."
                );

                break;
            }

            /* Exibir os universos salvos para o usuário escolher. */
            for (int i = 0; i < universosSalvos.Length; i++)
            {
                Console.WriteLine(
                    $"{i + 1} - {Path.GetFileName(universosSalvos[i])}"
                );
            }

            Console.WriteLine("0 - Voltar");

            Console.WriteLine();
            Console.Write("Escolha um universo: ");

            int escolhaUniverso =
                int.Parse(Console.ReadLine()!);

            /* Voltar ao menu principal caso o usuário escolha a opção 0. */
            if (escolhaUniverso == 0)
            {
                aguardarEnter = false;
                Console.Clear();
                break;
            }

            /* Verificar se o universo escolhido existe na lista. */
            if (escolhaUniverso < 1 ||
                escolhaUniverso > universosSalvos.Length)
            {
                Console.WriteLine();
                Console.WriteLine("Opção inválida.");
                break;
            }

            string arquivoEscolhido =
                universosSalvos[escolhaUniverso - 1];

            /* Limpar a tela antes de exibir o universo escolhido. */
            Console.Clear();

            GravadorArquivoTexto gravadorCarregar =
                new GravadorArquivoTexto();

            /* Carregar o universo salvo do arquivo txt. */
            Universo universoCarregado =
                gravadorCarregar.Carregar(
                    arquivoEscolhido
                );

            Console.WriteLine("Universo carregado");
            Console.WriteLine();

            Console.WriteLine(
                $"Arquivo: {Path.GetFileName(arquivoEscolhido)}"
            );

            Console.WriteLine();

            Console.WriteLine(
                $"Quantidade de corpos: {universoCarregado.Corpos.Count}"
            );

            Console.WriteLine(
                $"Quantidade de iterações: {universoCarregado.QuantidadeIteracoes}"
            );

            Console.WriteLine(
                $"Tempo entre iterações: {universoCarregado.TempoEntreIteracoes}"
            );

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

        /* Apagar um universo salvo de um arquivo txt. */
        case "3":

            Console.WriteLine("Apagar universo salvo");
            Console.WriteLine();

            /* Aqui serve para buscar todos os arquivos de universos salvos. */
            string[] universosParaDeletar =
                Directory.GetFiles(
                    ".",
                    "universo_*.txt"
                );

            /* Verificar se existem universos salvos. */
            if (universosParaDeletar.Length == 0)
            {
                Console.WriteLine(
                    "Nenhum universo salvo foi encontrado no sistema."
                );

                break;
            }

            /* Exibir os universos salvos para o usuário escolher qual apagar. */
            for (int i = 0; i < universosParaDeletar.Length; i++)
            {
                Console.WriteLine(
                    $"{i + 1} - {Path.GetFileName(universosParaDeletar[i])}"
                );
            }

            Console.WriteLine("0 - Voltar");

            Console.WriteLine();
            Console.Write("Escolha um universo para apagar: ");

            int escolhaApagar =
                int.Parse(Console.ReadLine()!);

            /* Voltar ao menu principal caso o usuário escolha a opção 0. */
            if (escolhaApagar == 0)
            {
                aguardarEnter = false;
                Console.Clear();
                break;
            }

            /* Verificar se a opção escolhida existe na lista. */
            if (escolhaApagar < 1 ||
                escolhaApagar > universosParaDeletar.Length)
            {
                Console.WriteLine();
                Console.WriteLine("Opção inválida.");
                break;
            }

            string arquivoDeletar =
                universosParaDeletar[escolhaApagar - 1];

            /* Excluir o arquivo selecionado no disco. */
            File.Delete(arquivoDeletar);

            Console.WriteLine();
            Console.WriteLine(
                $"Arquivo {Path.GetFileName(arquivoDeletar)} apagado com sucesso."
            );

            break;

        /* Encerrar o programa. */
        case "0":

            Console.WriteLine("Programa encerrado.");

            continuar = false;
            aguardarEnter = false;

            break;


        default:

            Console.WriteLine("Opção inválida.");

            break;
    }

    /* Aguardar o usuário pressionar ENTER para voltar ao menu, caso a opção escolhida não seja sair. */
    if (continuar && aguardarEnter)
    {
        Console.WriteLine();
        Console.WriteLine(
            "Pressione ENTER para voltar ao menu."
        );

        Console.ReadLine();
        Console.Clear();
    }
}