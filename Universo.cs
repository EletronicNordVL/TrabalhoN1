using System;

public class Universo
{
    // Lista de corpos com nome "G".
    private const double G = 6.674184e-11;

    // Essa é a soma e a força de todos os pares de corpos, decompondoe em Fx/Fy 
    public void CalcularForcas() { }

    // Utilizar a fórmula F=ma, depois de calcular o v=v0+at e s=s0+v0t+(a/2)t², por eixo.
    public void AtualizarPosicoes(double tempoEntreIteracoes) { }

    // Tem a função de verificar a distância entre centros < soma dos raios
    // E se colidir, deve-se recalcular as velocidades usando a conservação de Q=mv
    public void TratarColisoes() { }

    // Essa é a função que irá exibir o estado atual do universo, com os corpos e suas posições.
    public void ExibirEstado() { }

    // Esse é o laço principal chamando os métodos acima com a "quantidade de iterações" em vezes
    public void ExecutarSimulacao(int iteracoes, double tempoEntreIteracoes) { }
}