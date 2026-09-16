using System;

public class Corpo
{
    public string Nome { get; set; }
    public double Massa { get; set; }      // kg
    public double Densidade { get; set; }  // kg/m³
    public double PosX { get; set; }       // m
    public double PosY { get; set; }       // m
    public double VelX { get; set; }       // m/s
    public double VelY { get; set; }       // m/s

    public double Raio { get; private set; } // Com o raio calculado, não vem de fora.

    // O construtor irá recebe os 7 atributos e já calcula o Raio a partir da
    // Massa/Densidade (Essa é a fórmula do volume da esfera isolando o "r")
}

