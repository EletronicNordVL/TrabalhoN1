using System;
using System.IO;
namespace TrabalhoN1;

// Essa é a classe abstrata para gravar os dados do universo em um arquivo txt.
public abstract class GravadorUniverso
{
    public abstract void Salvar(Universo universo, string caminho);
    public abstract Universo Carregar(string caminho);
}

public class GravadorArquivoTexto : GravadorUniverso
{
    public override void Salvar(Universo universo, string caminho)
    { // System.IO // }
        using StreamWriter arquivo = new StreamWriter(caminho);

        arquivo.WriteLine(
            $"{universo.Corpos.Count};" +
            $"{universo.QuantidadeIteracoes};" +
            $"{universo.TempoEntreIteracoes}"
        );

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
    public override Universo Carregar(string caminho)
    { // System.IO // }
        using StreamReader arquivo = new StreamReader(caminho);

        string primeiraLinha = arquivo.ReadLine()!;

        string[] dados = primeiraLinha.Split(';');

        int quantidadeCorpos = int.Parse(dados[0]);
        int quantidadeIteracoes = int.Parse(dados[1]);
        double tempoEntreIteracoes = double.Parse(dados[2]);

        Universo universo = new Universo();

        universo.QuantidadeIteracoes = quantidadeIteracoes;
        universo.TempoEntreIteracoes = tempoEntreIteracoes;

        for (int i = 0; i < quantidadeCorpos; i++)
        {
            string linhaCorpo = arquivo.ReadLine()!;

            string[] dadosCorpo = linhaCorpo.Split(';');

            string nome = dadosCorpo[0];
            double massa = double.Parse(dadosCorpo[1]);
            double densidade = double.Parse(dadosCorpo[2]);
            double posX = double.Parse(dadosCorpo[3]);
            double posY = double.Parse(dadosCorpo[4]);
            double velX = double.Parse(dadosCorpo[5]);
            double velY = double.Parse(dadosCorpo[6]);

            Corpo corpo = new Corpo(
                nome,
                massa,
                densidade,
                posX,
                posY,
                velX,
                velY
            );

            universo.AdicionarCorpo(corpo);
        }

        return universo;
    }
}
