using System;
using System.Collections.Generic;
namespace TrabalhoN1;

public class Universo
{
    // Constante gravitacional utilizada no cálculo da força entre os corpos.
    private const double G = 6.674184e-11;

    public List<Corpo> Corpos { get; private set; }

    public int QuantidadeIteracoes { get; set; }

    public double TempoEntreIteracoes { get; set; }

    public Universo()
    {
        Corpos = new List<Corpo>();
    }

    public void AdicionarCorpo(Corpo corpo)
    {
        Corpos.Add(corpo);
    }

    private double CalcularDistancia(Corpo corpo1, Corpo corpo2)
    {
        double deltaX = corpo2.PosX - corpo1.PosX;
        double deltaY = corpo2.PosY - corpo1.PosY;

        return Math.Sqrt(
            deltaX * deltaX +
            deltaY * deltaY
        );
    }

    // Calcula a força gravitacional entre dois corpos.
    private double CalcularForcaGravitacional(
        Corpo corpo1,
        Corpo corpo2)
    {
        double distancia = CalcularDistancia(corpo1, corpo2);

        if (distancia == 0)
        {
            return 0;
        }

        return G * corpo1.Massa * corpo2.Massa
               / (distancia * distancia);
    }

    // Calcula a força gravitacional resultante sobre os corpos.
    public void CalcularForcas()
    {
        // Zera as forças da iteração anterior.
        foreach (Corpo corpo in Corpos)
        {
            corpo.ForcaX = 0;
            corpo.ForcaY = 0;
        }

        // Percorre cada par de corpos uma única vez.
        for (int i = 0; i < Corpos.Count; i++)
        {
            for (int j = i + 1; j < Corpos.Count; j++)
            {
                Corpo corpo1 = Corpos[i];
                Corpo corpo2 = Corpos[j];

                double deltaX = corpo2.PosX - corpo1.PosX;
                double deltaY = corpo2.PosY - corpo1.PosY;

                double distancia = CalcularDistancia(
                    corpo1,
                    corpo2
                );

                if (distancia == 0)
                {
                    continue;
                }

                double forca = CalcularForcaGravitacional(
                    corpo1,
                    corpo2
                );

                double direcaoX = deltaX / distancia;
                double direcaoY = deltaY / distancia;

                double forcaX = forca * direcaoX;
                double forcaY = forca * direcaoY;

                corpo1.ForcaX += forcaX;
                corpo1.ForcaY += forcaY;

                corpo2.ForcaX -= forcaX;
                corpo2.ForcaY -= forcaY;
            }
        }
    }

    // Atualiza velocidade e posição dos corpos em cada iteração.
    public void AtualizarPosicoes(double tempoEntreIteracoes)
    {
        foreach (Corpo corpo in Corpos)
        {
            corpo.AceleracaoX = corpo.ForcaX / corpo.Massa;
            corpo.AceleracaoY = corpo.ForcaY / corpo.Massa;

            corpo.PosX +=
            corpo.VelX * tempoEntreIteracoes
            + (corpo.AceleracaoX / 2)
            * Math.Pow(tempoEntreIteracoes, 2);

            corpo.PosY +=
                corpo.VelY * tempoEntreIteracoes
                + (corpo.AceleracaoY / 2)
                * Math.Pow(tempoEntreIteracoes, 2);

            corpo.VelX +=
                corpo.AceleracaoX * tempoEntreIteracoes;

            corpo.VelY +=
                corpo.AceleracaoY * tempoEntreIteracoes;
        }
    }

    // Verifica colisões e recalcula as velocidades quando necessário.
    public void TratarColisoes()
    {
        for (int i = 0; i < Corpos.Count; i++)
        {
            for (int j = i + 1; j < Corpos.Count; j++)
            {
                Corpo corpo1 = Corpos[i];
                Corpo corpo2 = Corpos[j];

                double distancia =
                    CalcularDistancia(corpo1, corpo2);

                double somaRaios =
                    corpo1.Raio + corpo2.Raio;

                if (distancia <= somaRaios && distancia > 0)
                {
                    Console.WriteLine(
                        $"Colisão detectada entre " +
                        $"{corpo1.Nome} e {corpo2.Nome}");

                    double normalX =
                        (corpo2.PosX - corpo1.PosX) / distancia;

                    double normalY =
                        (corpo2.PosY - corpo1.PosY) / distancia;

                    Console.WriteLine(
                        $"Normal da colisão: " +
                        $"({normalX:F4}, {normalY:F4})");

                    double velocidadeRelativaX =
                        corpo2.VelX - corpo1.VelX;

                    double velocidadeRelativaY =
                        corpo2.VelY - corpo1.VelY;

                    double velocidadeNaNormal =
                        velocidadeRelativaX * normalX
                        + velocidadeRelativaY * normalY;

                    Console.WriteLine(
                        $"Velocidade relativa na normal: " +
                        $"{velocidadeNaNormal:F4}");

                    if (velocidadeNaNormal >= 0)
                    {
                        continue;
                    }

                    double impulso =
                        -(2 * velocidadeNaNormal)
                        / ((1 / corpo1.Massa) + (1 / corpo2.Massa));

                    double impulsoX = impulso * normalX;
                    double impulsoY = impulso * normalY;

                    corpo1.VelX -= impulsoX / corpo1.Massa;
                    corpo1.VelY -= impulsoY / corpo1.Massa;

                    corpo2.VelX += impulsoX / corpo2.Massa;
                    corpo2.VelY += impulsoY / corpo2.Massa;

                    double sobreposicao =
                        somaRaios - distancia;

                    double correcao =
                        sobreposicao / 2;

                    corpo1.PosX -= correcao * normalX;
                    corpo1.PosY -= correcao * normalY;

                    corpo2.PosX += correcao * normalX;
                    corpo2.PosY += correcao * normalY;
                }
            }
        }
    }

    // Exibe o estado atual dos corpos.
    public void ExibirEstado()
    {
        foreach (Corpo corpo in Corpos)
        {
            Console.WriteLine(
                $"{corpo.Nome} - " +
                $"Posição: ({corpo.PosX:F4}, {corpo.PosY:F4}) - " +
                $"Velocidade: ({corpo.VelX:F4}, {corpo.VelY:F4})");
        }
    }

    // Executa as iterações da simulação.
    public void ExecutarSimulacao(
        int iteracoes,
        double tempoEntreIteracoes)
    {
        for (int i = 1; i <= iteracoes; i++)
        {
            CalcularForcas();

            AtualizarPosicoes(tempoEntreIteracoes);

            TratarColisoes();

            Console.WriteLine($"Iteração {i}");

            ExibirEstado();

            Console.WriteLine();
        }
    }
}