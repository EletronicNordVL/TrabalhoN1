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

        public double PosX { get; set; }
        public double PosY { get; set; }
        public double VelX { get; set; }
        public double VelY { get; set; }

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