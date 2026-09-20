using TrabalhoN1;

Corpo corpo = new Corpo(
    "Corpo A",
    1000,
    5000,
    10,
    20,
    2,
    -1
);

Console.WriteLine($"Nome: {corpo.Nome}");
Console.WriteLine($"Massa: {corpo.Massa} kg");
Console.WriteLine($"Densidade: {corpo.Densidade} kg/m³");
Console.WriteLine($"Posição: ({corpo.PosX}, {corpo.PosY}) m");
Console.WriteLine($"Velocidade: ({corpo.VelX}, {corpo.VelY}) m/s");
Console.WriteLine($"Raio: {corpo.Raio:F4} m");