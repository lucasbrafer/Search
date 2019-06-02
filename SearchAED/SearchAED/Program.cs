using System;
using System.IO;

namespace SearchAED
{
    class Registro
    {
        //chave -> room_id
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

    class List
    {
        class Celula
        {
            public Registro Dado { get; set; }
            public Celula Prox { get; set; }
        }

        private Celula Inicio;
        private Celula Fim;
        private int Tam;

        public List()
        {
            Tam = 0;
            Inicio = new Celula();
            Fim = Inicio;
            Inicio.Prox = null;
        }

        public bool Vazia()
        {
            return Tam == 0;
        }

        public int Tamanho()
        {
            return Tam;
        }

        public void Inserir(Registro _dado)
        {
            Celula temp = new Celula();
            temp.Dado = _dado;
            temp.Prox = null;
            Fim.Prox = temp;
            Fim = temp;
            Tam++;
        }

        public Registro Pesquisar(int Reg)
        {
            Celula temp = Inicio.Prox;

            while (temp != null)
            {
                if (temp.Dado.room_id == Reg)
                    return temp.Dado;

                temp = temp.Prox;
            }

            return null;
        }



    }

    class ArvoreBinaria
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
            else if (_dado.room_id < no.Dado.room_id) no.Esq = Inserir(no.Esq, _dado);
            else if (_dado.room_id > no.Dado.room_id) no.Dir = Inserir(no.Dir, _dado);
            else Console.WriteLine("Erro: Registro ja existente");
            return no;
        }

        public Registro Pesquisar(int _key)
        {
           return Pesquisar(Raiz, _key);
        }

        private Registro Pesquisar(No no, int key)
        {
            if (no == null)
                return null;

            if (key < no.Dado.room_id) return Pesquisar(no.Esq, key);
            else if (key > no.Dado.room_id) return Pesquisar(no.Dir, key);
            else return no.Dado;
        }

    }

    class TabelaHash
    {
        List[] tabela;
        int Max;

        public TabelaHash(int _max)
        {
            Max = _max;
            tabela = new List[Max];

            for (int i = 0; i < Max; i++)
                tabela[i] = new List();
        }

        private int h(int _chave)
        {
            return _chave % Max;
        }

        public void Inserir(Registro reg)
        {
            int pos = h(reg.room_id);
            tabela[pos].Inserir(reg);
        }

        public Registro Pesquisar(int chave)
        {
            int pos = h(chave);
            Registro r = tabela[pos].Pesquisar(chave);

            return r;

        }
    }

    class Program
    {
        //--> Pesquisa Binaria
        static Registro[] QuickSort(Registro[] vetor, int primeiro, int ultimo)
        {

            int baixo, alto, meio, pivo;
            Registro repositorio;
            baixo = primeiro;
            alto = ultimo;
            meio = (int)((baixo + alto) / 2);

            pivo = vetor[meio].room_id;

            while (baixo <= alto)
            {
                while (vetor[baixo].room_id < pivo)
                    baixo++;
                while (vetor[alto].room_id > pivo)
                    alto--;
                if (baixo < alto)
                {
                    repositorio = vetor[baixo];
                    vetor[baixo++] = vetor[alto];
                    vetor[alto--] = repositorio;
                }
                else
                {
                    if (baixo == alto)
                    {
                        baixo++;
                        alto--;
                    }
                }
            }

            if (alto > primeiro)
                QuickSort(vetor, primeiro, alto);
            if (baixo < ultimo)
                QuickSort(vetor, baixo, ultimo);

            return vetor;
        }

        static Registro Pesquisa_Binaria(Registro[] vetor, int room_id)
        {
            int Inicio = 0;
            int Fim = vetor.Length - 1;

            while (Inicio <= Fim)
            {
                int Meio = (Inicio + Fim) / 2;

                if (vetor[Meio].room_id == room_id)
                    return vetor[Meio];

                if (vetor[Meio].room_id > room_id)
                    Fim = Meio - 1;
                else
                    Inicio = Meio + 1;
            }

            return null;
        }


        //--> Pesquisa Sequencial
        static Registro Pesquisa_Sequencial(Registro[] vetor, int room_id)
        {
            for(int i=0; i < vetor.Length; i++)
            {
                if (vetor[i].room_id == room_id)
                    return vetor[i];
            }
            return null;
        }


        // Read --->  airbnb
        static Registro GeraRegistro(string[] linhasplit)
        {
            Registro x = new Registro();
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

        static void LerArquivo(ref Registro[] dados)
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
                    dados[i] = GeraRegistro(linhasplit);
                }
            }
            read.Close();
            arq.Close();
        }



        static void Main(string[] args)
        {
            Registro[] dados = new Registro[128000];
            LerArquivo(ref dados);

            double[,] Sequencial = new double[5,3];
            double[,] PesquisaBinaria = new double[5,3];
            double[,] ArvoreBinaria = new double[5,3];
            double[,] TabelaHash = new double[5,3];

            //--> Pesquisa Binaria
            Registro[] DadosOrdenados = QuickSort(dados, 0, dados.Length - 1);

            //--> Arvore
            ArvoreBinaria arvore = new ArvoreBinaria();

            for (int i = 0; i < dados.Length; i++)
                arvore.Inserir(dados[i]);

            //--> TabelaHash
            TabelaHash hash = new TabelaHash(120001);

            for (int i = 0; i < dados.Length; i++)
                hash.Inserir(dados[i]);


            int mid = dados.Length % 2;
            int final = dados.Length - 1;

            int id_initial = dados[0].room_id;
            int id_mid = dados[mid].room_id;
            int id_final = dados[final].room_id;


            for (int i = 0; i < 5; i++)
            {
                //Pesquisa Sequencial --> room_id
                #region Sequencial

                var watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Sequencial(dados,id_initial);

                watch.Stop();
                Sequencial[i,0] = watch.ElapsedMilliseconds / 1000.0;


                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Sequencial(dados, id_mid);

                watch.Stop();
                Sequencial[i, 1] = watch.ElapsedMilliseconds / 1000.0;

                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Sequencial(dados, id_final);

                watch.Stop();
                Sequencial[i, 2] = watch.ElapsedMilliseconds / 1000.0;

                #endregion


                //Pesquisa Binaria -> room_id
                #region PesquisaBinaria

                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Binaria(DadosOrdenados, id_initial);

                watch.Stop();
                PesquisaBinaria[i, 0] = watch.ElapsedMilliseconds / 1000.0;


                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Binaria(DadosOrdenados, id_mid);

                watch.Stop();
                PesquisaBinaria[i, 1] = watch.ElapsedMilliseconds / 1000.0;

                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Binaria(DadosOrdenados, id_final);

                watch.Stop();
                PesquisaBinaria[i, 2] = watch.ElapsedMilliseconds / 1000.0;

                #endregion


                //Arvore Binaria --> room_id
                #region ArvoreBinaria
                watch = System.Diagnostics.Stopwatch.StartNew();

                arvore.Pesquisar(id_initial);

                watch.Stop();
                ArvoreBinaria[i, 0] = watch.ElapsedMilliseconds / 1000.0;


                watch = System.Diagnostics.Stopwatch.StartNew();

                arvore.Pesquisar(id_mid);

                watch.Stop();
                ArvoreBinaria[i, 1] = watch.ElapsedMilliseconds / 1000.0;

                watch = System.Diagnostics.Stopwatch.StartNew();

                arvore.Pesquisar(id_final);

                watch.Stop();
                ArvoreBinaria[i, 2] = watch.ElapsedMilliseconds / 1000.0;
                #endregion


                //Tabela Hash --> room_id
                #region TabelaHash
                watch = System.Diagnostics.Stopwatch.StartNew();

                hash.Pesquisar(id_initial);

                watch.Stop();
                TabelaHash[i, 0] = watch.ElapsedMilliseconds / 1000.0;


                watch = System.Diagnostics.Stopwatch.StartNew();

                hash.Pesquisar(id_mid);

                watch.Stop();
                TabelaHash[i, 1] = watch.ElapsedMilliseconds / 1000.0;

                watch = System.Diagnostics.Stopwatch.StartNew();

                hash.Pesquisar(id_final);

                watch.Stop();
                TabelaHash[i, 2] = watch.ElapsedMilliseconds / 1000.0;
                #endregion

            }



            FileStream arq = new FileStream("RelatorioID.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);


            write.WriteLine("{0};{1};{2};{3}");

            write.Close();
            arq.Close();


            Console.ReadKey();
        }

        public static double MediaTempo(double[] A)
        {
            double aux;
            for (int i = 0; i < A.Length - 1; i++)
                for (int j = 0; j < A.Length - i - 1; j++)
                    if (A[j] > A[j + 1])
                    {
                        aux = A[j];
                        A[j] = A[j + 1];
                        A[j + 1] = aux;
                    }

            return ((A[1] + A[2] + A[3]) / 3);

        }



    }
}