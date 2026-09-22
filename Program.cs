using System;
using TrabalhoN1;
Corpo corpo1 = new Corpo(
    "Corpo 1",
    1000,
    1000,
    0,
    0,
    1,
    1
);

Corpo corpo2 = new Corpo(
    "Corpo 2",
    1000,
    1000,
    0.8,
    0.8,
    -1,
    -1
);

Universo universo = new Universo();

universo.AdicionarCorpo(corpo1);
universo.AdicionarCorpo(corpo2);

universo.TratarColisoes();

double deltaX = corpo2.PosX - corpo1.PosX;
double deltaY = corpo2.PosY - corpo1.PosY;

double distanciaFinal = Math.Sqrt(
    deltaX * deltaX +
    deltaY * deltaY
);

double somaRaios =
    corpo1.Raio + corpo2.Raio;

Console.WriteLine(
    $"Distância final: {distanciaFinal:F6} m");

Console.WriteLine(
    $"Soma dos raios: {somaRaios:F6} m");

Console.WriteLine(
    $"Velocidade final Corpo 1: " +
    $"({corpo1.VelX:F4}, {corpo1.VelY:F4})");

Console.WriteLine(
    $"Velocidade final Corpo 2: " +
    $"({corpo2.VelX:F4}, {corpo2.VelY:F4})");

