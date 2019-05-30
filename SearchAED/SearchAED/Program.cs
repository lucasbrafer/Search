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
        static int Pesquisa_Sequencial(Dados[] Vetor_Dados_Ordenados, int room_id)
        {
            for(int i=0; i < Vetor_Dados_Ordenados.Length; i++)
            {
                if(Vetor_Dados_Ordenados[i].room_id == room_id)
                    return i;
            }
            return -1;
        }
        static int Pesquisa_Binaria(Dados[] Vetor_Dados_Ordenados, int room_id)
        {
            int Inicio = 0;
            int Fim = Vetor_Dados_Ordenados.Length - 1;
            while (Inicio <= Fim)
            {
                int Meio = (Inicio + Fim) / 2;

                if (Vetor_Dados_Ordenados[Meio].room_id == room_id)
                    return Meio;

                if (Vetor_Dados_Ordenados[Meio].room_id > room_id)
                    Fim = Meio - 1;
                else
                    Inicio = Meio + 1;
            }

            return -1;
        }
        class Registro
        {
            public Dados Dado { get; set; }

            public Registro(int _chave)
            {
                Dado.room_id = _chave;
            }
        }

        class Arvore_Binaria
        {
            class No
            {
                public Registro Dado;
                public No Esq, Dir;
                public No(Registro _dado)
                {
                    Dado = _dado;
                    Esq = Dir = null;
                }
            }    

            private No Raiz;

            public ArvoreBinaria()
            {
                Raiz = null;
            }

            public bool Vazia()
            {
                return Raiz == null;
            }

            public void Inserir(Registro _dado)
            {
                Raiz = Inserir(Raiz, _dado);            
            }

            private No Inserir(No no, Registro _dado)
            {
                if (no == null)
                    no = new No(_dado);
                else if (_dado.Chave < no.Dado.Chave) no.Esq = Inserir(no.Esq, _dado);
                else if (_dado.Chave > no.Dado.Chave) no.Dir = Inserir(no.Dir, _dado);
                else Console.WriteLine("Erro: Registro ja existente");
                return no;
            }

            private void ImprimirEmOrdem(No no)
            {
                if(no != null)
                {               
                    ImprimirEmOrdem(no.Esq);
                    Console.WriteLine(no.Dado.Chave);
                    ImprimirEmOrdem(no.Dir);
                }
            }
            public void ImprimirEmOrdem() 
            {
                ImprimirEmOrdem(Raiz);
            }
            public void Remover(int _chave)
            {
                Raiz = Remover(Raiz, _chave);
            }

            private No Remover(No no, int _chave)
            {
                if (no == null) Console.WriteLine("Erro: Registro nao encontrado");
                else if (_chave < no.Dado.Chave) no.Esq = Remover(no.Esq, _chave);
                else if (_chave > no.Dado.Chave) no.Dir = Remover(no.Dir, _chave);
                else
                {
                    if (no.Dir == null) no = no.Esq;
                    else if (no.Esq == null) no = no.Dir;
                    else no.Esq = Antecessor(no, no.Esq);
                }
                return no;
            }

            private No Antecessor(No no, No ant)
            {
                if (ant.Dir != null) ant.Dir = Antecessor(no, ant.Dir);
                else
                {
                    no.Dado = ant.Dado;
                    ant = ant.Esq;
                }
                return ant;
            }
        }
    }
}