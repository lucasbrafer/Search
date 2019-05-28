using System;
using System.IO;

namespace SearchAED
{
    class Dados
    {
        public int room_id { get; set; }
        public int host_id { get; set; }
        public string room_type { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string neighborhood { get; set; }
        public int reviews { get; set; }
        public double overall_satisfaction { get; set; }
        public int accommodates { get; set; }
        public double bedrooms { get; set; }
        public double price { get; set; }
        public string property_type { get; set; }
    }
    class Program
    {
        static Dados PreencheArq(string[] linhasplit)
        {
            Dados x = new Dados();
            x.room_id = int.Parse(linhasplit[0]);
            x.host_id = int.Parse(linhasplit[1]);
            x.room_type = linhasplit[2];
            x.country = linhasplit[3];
            x.city = linhasplit[4];
            x.neighborhood = linhasplit[5];
            x.reviews = int.Parse(linhasplit[6]);
            x.overall_satisfaction = double.Parse(linhasplit[7]);
            x.accommodates = int.Parse(linhasplit[8]);
            x.bedrooms = double.Parse(linhasplit[9]);
            x.price = double.Parse(linhasplit[10]);
            x.property_type = linhasplit[11];

            return x;
        }
        static void LerArquivo()
        {
            FileStream arq = new FileStream("dados_airbnb.csv", FileMode.Open);
            StreamReader read = new StreamReader(arq);
            read.ReadLine();
            string linha;
            string[] linhasplit;

            for (int i = 0; i < dados.Length; i++)
            {
                linha = read.ReadLine();
                if (linha != null)
                {
                    linhasplit = linha.Split(';');
                    dados[i] = PreencheArq(linhasplit);
                }
            }
            arq.Close();
        }

        static Dados[] dados = new Dados[128000];

        static void Main(string[] args)
        {

        }
    }
}
