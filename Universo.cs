using System;
using System.Collections.Generic;
namespace TrabalhoN1;

public class Universo
{
    /* Constante gravitacional utilizada no cálculo da força entre os corpos. */
    private const double G = 6.674184e-11;

    public List<Corpo> Corpos { get; private set; }

    public int QuantidadeIteracoes { get; set; }

    public double TempoEntreIteracoes { get; set; }

    /* Construtor da classe Universo. */
    public Universo()
    {
        Corpos = new List<Corpo>();
    }

    /* Adiciona um corpo ao universo. */
    public void AdicionarCorpo(Corpo corpo)
    {
        Corpos.Add(corpo);
    }

    /* Calcula a distância entre dois corpos. */
    private double CalcularDistancia(Corpo corpo1, Corpo corpo2)
    {
        double deltaX = corpo2.PosX - corpo1.PosX;
        double deltaY = corpo2.PosY - corpo1.PosY;

        return Math.Sqrt(
            deltaX * deltaX +
            deltaY * deltaY
        );
    }

    /* Calcula a força gravitacional entre dois corpos. */
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

    /* Calcula a força gravitacional resultante sobre os corpos. */
    public void CalcularForcas()
    {
        /* Zera as forças da iteração anterior para não acumular com o novo ciclo. */
        foreach (Corpo corpo in Corpos)
        {
            corpo.ForcaX = 0;
            corpo.ForcaY = 0;
        }

        /* Percorre cada par de corpos uma única vez utilizando o (j = i + 1) para otimizar o processamento. */
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

                /* Áqui será aplicada a Lei da Gravitação Universal de Isaac Newton que é o F = G * (m1 * m2) / r² */
                double forca = CalcularForcaGravitacional(
                    corpo1,
                    corpo2
                );

                /* Utilizaremos a decomposição vetorial da força resultante para os eixos X e Y onde utilizaremos os triângulos (seno e cosseno do ângulo) */
                double direcaoX = deltaX / distancia;
                double direcaoY = deltaY / distancia;

                double forcaX = forca * direcaoX;
                double forcaY = forca * direcaoY;

                /* Terceira Lei de Newton (Ação e Reação) onde a força que o corpo 1 exerce sobre o corpo 2 é igual em módulo e oposta em direção á força
                 * que o corpo 2 exerce sobre o corpo 1. */
                corpo1.ForcaX += forcaX;
                corpo1.ForcaY += forcaY;

                corpo2.ForcaX -= forcaX;
                corpo2.ForcaY -= forcaY;
            }
        }
    }

    /* Atualiza velocidade e posição dos corpos em cada iteração utilizando MRUV (Movimento Retilíneo Uniformemente Variado). */
    public void AtualizarPosicoes(double tempoEntreIteracoes)
    {
        foreach (Corpo corpo in Corpos)
        {

            /* Aplica a 2ª Lei de Newton (F = m * a) isolando a aceleração (a = F / m) */
            corpo.AceleracaoX = corpo.ForcaX / corpo.Massa;
            corpo.AceleracaoY = corpo.ForcaY / corpo.Massa;

            /* Equação horária da posição (s = s0 + v0*t + (a*t²)/2) */
            corpo.PosX +=
            corpo.VelX * tempoEntreIteracoes
            + (corpo.AceleracaoX / 2)
            * Math.Pow(tempoEntreIteracoes, 2);

            corpo.PosY +=
                corpo.VelY * tempoEntreIteracoes
                + (corpo.AceleracaoY / 2)
                * Math.Pow(tempoEntreIteracoes, 2);

            /* Equação da velocidade (v = v0 + a*t) */
            corpo.VelX +=
                corpo.AceleracaoX * tempoEntreIteracoes;

            corpo.VelY +=
                corpo.AceleracaoY * tempoEntreIteracoes;
        }
    }

    /* Verifica colisões e recalcula as velocidades quando necessário. */
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

                /* A soma dos raios dos corpos é utilizada para determinar se houve colisão. */
                double somaRaios =
                    corpo1.Raio + corpo2.Raio;

                if (distancia <= somaRaios)
                {
                    Console.WriteLine(
                        $"Colisão detectada entre " +
                        $"{corpo1.Nome} e {corpo2.Nome}");

                    double normalX;
                    double normalY;

                    if (distancia > 0)
                    {
                        normalX =
                            (corpo2.PosX - corpo1.PosX) / distancia;

                        normalY =
                            (corpo2.PosY - corpo1.PosY) / distancia;
                    }
                    else
                    {
                        double diferencaVelX =
                            corpo1.VelX - corpo2.VelX;

                        double diferencaVelY =
                            corpo1.VelY - corpo2.VelY;

                        double moduloVelocidade =
                            Math.Sqrt(
                                diferencaVelX * diferencaVelX +
                                diferencaVelY * diferencaVelY
                            );

                        if (moduloVelocidade == 0)
                        {
                            normalX = 1;
                            normalY = 0;
                        }
                        else
                        {
                            normalX =
                                diferencaVelX / moduloVelocidade;

                            normalY =
                                diferencaVelY / moduloVelocidade;
                        }
                    }

                    /* A normal da colisão é um vetor unitário que aponta da posição do 
                     * corpo 1 para a posição do corpo 2. */
                    Console.WriteLine(
                        $"Normal da colisão: " +
                        $"({normalX:F4}, {normalY:F4})");

                    double velocidadeRelativaX =
                        corpo2.VelX - corpo1.VelX;

                    double velocidadeRelativaY =
                        corpo2.VelY - corpo1.VelY;

                    /* Aqui é para calcular a velocidade relativa entre os corpos na direção do impacto (normal) */
                    double velocidadeNaNormal =
                        velocidadeRelativaX * normalX
                        + velocidadeRelativaY * normalY;

                    Console.WriteLine(
                        $"Velocidade relativa na normal: " +
                        $"{velocidadeNaNormal:F4}");

                    /* Se a velocidade relativa é positiva, os corpos já estão se afastando e não é necessário tratar a colisão. */
                    if (velocidadeNaNormal >= 0)
                    {
                        continue;
                    }

                    /* CONSERVAÇÃO DA QUANTIDADE DE MOVIMENTO (Q = m * v) */
                    /* Este cálculo de impulso deriva da fórmula da conservação do momento linear em uma colisão elástica bidimensional (Q_antes = Q_depois). */
                    double impulso =
                        -(2 * velocidadeNaNormal)
                        / ((1 / corpo1.Massa) + (1 / corpo2.Massa));

                    double impulsoX = impulso * normalX;
                    double impulsoY = impulso * normalY;

                    /* Aplica a alteração das velocidades usando J = Delta_Q (Impulso = variação do momento) */
                    /* Portanto, isolamos a velocidade: Delta_v = J / m */
                    corpo1.VelX -= impulsoX / corpo1.Massa;
                    corpo1.VelY -= impulsoY / corpo1.Massa;

                    corpo2.VelX += impulsoX / corpo2.Massa;
                    corpo2.VelY += impulsoY / corpo2.Massa;

                    /* Resolve a sobreposição: separa os corpos fisicamente para não ficarem "presos". */
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

    /* Exibe o estado atual dos corpos. */
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

    /* Executa as iterações da simulação. */
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

    /* Gera corpos aleatórios para a simulação. */
    public void GerarCorposAleatorios(int quantidade)
    {
        Random random = new Random();

        /* Gera corpos com propriedades aleatórias dentro de intervalos definidos. */
        for (int i = 0; i < quantidade; i++)
        {
            string nome = $"Corpo {i + 1}";
            double massa = random.NextDouble() * (1e25 - 1e22) + 1e22;      // 10^22 a 10^25 kg
            double densidade = random.NextDouble() * (6000 - 3000) + 3000;  // 3000 a 6000 kg/m³
            double posX = random.NextDouble() * (5e9 - (-5e9)) + (-5e9);    // ±5 bilhões de metros
            double posY = random.NextDouble() * (5e9 - (-5e9)) + (-5e9);
            double velX = random.NextDouble() * (200 - (-200)) + (-200);    // ±200 m/s
            double velY = random.NextDouble() * (200 - (-200)) + (-200);

            Corpo corpo = new Corpo(
                nome,
                massa,
                densidade,
                posX,
                posY,
                velX,
                velY
            );

            AdicionarCorpo(corpo);
        }
    }
}