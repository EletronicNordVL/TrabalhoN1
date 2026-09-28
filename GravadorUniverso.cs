using System;
using System.IO;
namespace TrabalhoN1;

/* Essa é a classe abstrata para gravar os dados do universo em um arquivo txt. */
public abstract class GravadorUniverso
{
    /* Essa função salva os dados do universo em um arquivo txt. */
    public abstract void Salvar(Universo universo, string caminho);
    /* Essa função tem a função de ler os dados do arquivo txt e cria um objeto Universo com os corpos carregados. */
    public abstract Universo Carregar(string caminho);
}

/* Essa é a classe concreta que implementa a gravação e leitura dos dados do universo em um arquivo txt. */
public class GravadorArquivoTexto : GravadorUniverso
{
    /* Essa função salva os dados do universo em um arquivo txt. */
    public override void Salvar(Universo universo, string caminho)
    {
        using StreamWriter arquivo = new StreamWriter(caminho);

        arquivo.WriteLine(
            $"{universo.Corpos.Count};" +
            $"{universo.QuantidadeIteracoes};" +
            $"{universo.TempoEntreIteracoes}"
        );

        /* Salvar os dados de cada corpo no arquivo. */
        foreach (Corpo corpo in universo.Corpos)
        {
            arquivo.WriteLine(
                $"{corpo.Nome};" +
                $"{corpo.Massa};" +
                $"{corpo.Densidade};" +
                $"{corpo.PosX};" +
                $"{corpo.PosY};" +
                $"{corpo.VelX};" +
                $"{corpo.VelY}"
            );
        }
    }

    /* Essa função lê os dados do arquivo txt e cria um objeto Universo com os corpos carregados. */
    public override Universo Carregar(string caminho)
    {
        /* Abrir o arquivo para leitura. */
        using StreamReader arquivo = new StreamReader(caminho);

        /* Ler a primeira linha do arquivo, que contém a quantidade de corpos, quantidade de iterações e tempo entre iterações. */
        string primeiraLinha = arquivo.ReadLine()!;

        /* Separar os dados da primeira linha em um array de strings. */
        string[] dados = primeiraLinha.Split(';');

        /* Ler a quantidade de corpos, quantidade de iterações e tempo entre iterações do arquivo. */
        int quantidadeCorpos = int.Parse(dados[0]);
        int quantidadeIteracoes = int.Parse(dados[1]);
        double tempoEntreIteracoes = double.Parse(dados[2]);

        Universo universo = new Universo();

        /* Atribuir a quantidade de iterações e o tempo entre iterações ao objeto */
        universo.QuantidadeIteracoes = quantidadeIteracoes;
        universo.TempoEntreIteracoes = tempoEntreIteracoes;

        /* Ler os corpos do arquivo e adicioná-los ao universo. */
        for (int i = 0; i < quantidadeCorpos; i++)
        {
            string linhaCorpo = arquivo.ReadLine()!;

            string[] dadosCorpo = linhaCorpo.Split(';');

            /* Ler os dados do corpo do arquivo. */
            string nome = dadosCorpo[0];
            double massa = double.Parse(dadosCorpo[1]);
            double densidade = double.Parse(dadosCorpo[2]);
            double posX = double.Parse(dadosCorpo[3]);
            double posY = double.Parse(dadosCorpo[4]);
            double velX = double.Parse(dadosCorpo[5]);
            double velY = double.Parse(dadosCorpo[6]);

            /* Criar um novo corpo com os dados lidos do arquivo e adicioná-lo ao universo. */
            Corpo corpo = new Corpo(
                nome,
                massa,
                densidade,
                posX,
                posY,
                velX,
                velY
            );

            /* Adicionar o corpo ao universo. */
            universo.AdicionarCorpo(corpo);
        }

        return universo;
    }
}
