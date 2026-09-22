using System;
using System.Collections.Generic;
namespace TrabalhoN1;

public class Universo
{
    // Constante gravitacional utilizada no cálculo da força entre os corpos.
    private const double G = 6.674184e-11;

    public List<Corpo> Corpos { get; private set; }

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
    }

    // Verifica colisões e recalcula as velocidades quando necessário.
    public void TratarColisoes()
    {
    }

    // Exibe o estado atual dos corpos.
    public void ExibirEstado()
    {
    }

    // Executa as iterações da simulação.
    public void ExecutarSimulacao(
        int iteracoes,
        double tempoEntreIteracoes)
    {
    }
}