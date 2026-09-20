using TrabalhoN1;

Corpo corpoA = new Corpo(
    "Corpo A",
    1000,
    5000,
    10,
    20,
    2,
    -1
);

Corpo corpoB = new Corpo(
    "Corpo B",
    2000,
    6000,
    50,
    30,
    -1,
    2
);

Corpo corpoC = new Corpo(
    "Corpo C",
    1500,
    5500,
    30,
    60,
    0,
    0
);

Universo universo = new Universo();

universo.AdicionarCorpo(corpoA);
universo.AdicionarCorpo(corpoB);
universo.AdicionarCorpo(corpoC);

Console.WriteLine($"Quantidade de corpos: {universo.Corpos.Count}");

foreach (Corpo corpo in universo.Corpos)
{
    Console.WriteLine(
        $"{corpo.Nome} - Posição: ({corpo.PosX}, {corpo.PosY})");
}
universo.CalcularForcas();

foreach (Corpo corpo in universo.Corpos)
{
    Console.WriteLine(
        $"{corpo.Nome}: Fx = {corpo.ForcaX:E4} N | Fy = {corpo.ForcaY:E4} N");
}