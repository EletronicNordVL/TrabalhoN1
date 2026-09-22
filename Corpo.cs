using System;
namespace TrabalhoN1
{
    public class Corpo
    {
        private const double DensidadeMaxima = 1e18;
        private string nome; 
        private double massa; 
        private double densidade;
        public string Nome
        {
            get => nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "O nome do corpo não pode ser vazio.");

                nome = value;
            }
        }

        /* É medida em quilogramas (kg) e representa a quantidade de matéria contida no corpo. */
        public double Massa
        {
            get => massa;
            set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "A massa deve ser maior que zero.");

                massa = value;
            }
        }

        /* Densidade do corpo, em quilogramas por metro cúbico (kg/m³) */
        public double Densidade
        {
            get => densidade;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException(
                        "A densidade deve ser maior que zero.");
                }

                if (value > DensidadeMaxima)
                {
                    throw new ArgumentException(
                        "A densidade excede o limite máximo permitido.");
                }

                densidade = value;
            }
        }

        public double PosX { get; set; } /* Posição no eixo X, em metros (m) */
        public double PosY { get; set; } /* Posição no eixo Y, em metros (m) */
        public double VelX { get; set; } /* Velocidade no eixo X, em metros por segundo (m/s) */
        public double VelY { get; set; } /* Velocidade no eixo Y, em metros por segundo (m/s) */

        public double ForcaX { get; set; }
        public double ForcaY { get; set; }

        // O raio não é armazenado separadamente porque depende
        // diretamente da massa e da densidade do corpo.
        public double Raio => CalcularRaio();

        public Corpo(
            string nome,
            double massa,
            double densidade,
            double posX,
            double posY,
            double velX,
            double velY)
        {
            Nome = nome;
            Massa = massa;
            Densidade = densidade;
            PosX = posX;
            PosY = posY;
            VelX = velX;
            VelY = velY;
        }

        public double CalcularRaio()
        {
            double volume = Massa / Densidade;

            return Math.Cbrt(
                (3 * volume) / (4 * Math.PI)
            );
        }
    }
}