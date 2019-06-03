using System;
using System.IO;
using System.Threading;

namespace SearchAED
{
    class SemiRegistro
    {
        public string country { get; set; }
        public int accommodates { get; set; }
    }

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
            Celula aux = Inicio.Prox;

            int cont = 0;

            Registro temp  = null;

            while (aux != null)
            {
                cont++;
                if (aux.Dado.room_id == Reg)
                {
                    temp = aux.Dado;
                    break;
                }
                aux = aux.Prox;
            }


            FileStream arq = new FileStream("teste.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            write.WriteLine("{0};{1};","hash",cont);


            write.Close();
            arq.Close();

            return temp;
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
           int cont = 0;

           Registro temp = Pesquisar(Raiz, _key, ref cont);

            FileStream arq = new FileStream("teste.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            write.WriteLine("{0};{1};", "Tree", cont);


            write.Close();
            arq.Close();

            return temp;

        }

        private Registro Pesquisar(No no, int key, ref int cont)
        {
            if (no == null)
                return null;

            cont++;

            if (key < no.Dado.room_id) return Pesquisar(no.Esq, key,ref cont);
            else if (key > no.Dado.room_id) return Pesquisar(no.Dir, key,ref cont);
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

        static Registro Pesquisa_Binaria(ref Registro[] vetor, int room_id)
        {
            int Inicio = 0;
            int Fim = vetor.Length - 1;
            int cont = 0;

            Registro temp = null;

            while (Inicio <= Fim)
            {
                int Meio = (Inicio + Fim) / 2;

                cont++;

                if (vetor[Meio].room_id == room_id)
                {
                    temp = vetor[Meio];
                    break;
                }

                if (vetor[Meio].room_id > room_id)
                    Fim = Meio - 1;
                else
                    Inicio = Meio + 1;

            }

            FileStream arq = new FileStream("teste.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            write.WriteLine("{0};{1};", "binary", cont);


            write.Close();
            arq.Close();

            return temp;
        }


        //--> Pesquisa Sequencial
        static Registro Pesquisa_Sequencial(ref Registro[] vetor, int room_id)
        {
            int cont = 0;
            Registro temp = null;

            for(int i=0; i < vetor.Length; i++)
            {
                cont++;
                if (vetor[i].room_id == room_id)
                {
                    temp = vetor[i];
                    break;
                }
            }

            FileStream arq = new FileStream("teste.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            write.WriteLine("{0};{1};", "sequential", cont);


            write.Close();
            arq.Close();

            return temp;
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



        //--> Media do tempo dos testes
        public static double[] MediaTempo(double[,] vetor)
        {
            double[] A = new double[5];
            double[] B = new double[5];
            double[] C = new double[5];

            for (int i = 0; i < 5; i++)
            {
                A[i] = vetor[i, 0];
                B[i] = vetor[i, 1];
                C[i] = vetor[i, 2];
            }

            double[] Resultado = new double[3];

            Resultado[0] = MediaTempo(A);
            Resultado[1] = MediaTempo(B);
            Resultado[2] = MediaTempo(C);

            return Resultado;

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


        static void Main(string[] args)
        {
            Registro[] dados = new Registro[128000];
            LerArquivo(ref dados);

            id_test(ref dados);
          
        }


        //teste --> room_id
        #region room_id

        public static void id_test(ref Registro[] dados)
        {
            int mid = dados.Length / 2;
            int final = dados.Length - 1;

            int id_initial = dados[0].room_id;
            int id_mid = dados[mid].room_id;
            int id_final = dados[final].room_id;

            Console.WriteLine(id_initial);
            Console.WriteLine(id_mid);
            Console.WriteLine(id_final);

            Test_Sequential(ref dados, id_initial, id_mid, id_final);
            Test_BinarySearch(ref dados, id_initial, id_mid, id_final);
            Test_HashTable(ref dados, id_initial, id_mid, id_final);
            //Test_BinaryTree(ref dados, id_initial, id_mid, id_final);

        }

        #region Search Sequential

        public static void Test_Sequential(ref Registro[] dados, int id_initial, int id_mid, int id_final)
        {
            double[,] Sequencial = new double[5, 3];


            for (int i = 0; i < 5; i++)
            {
                //Pesquisa Sequencial --> room_id
                #region Sequencial

                var watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Sequencial(ref dados, id_initial);

                watch.Stop();
                Sequencial[i, 0] = watch.ElapsedMilliseconds * 1000;


                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Sequencial(ref dados, id_mid);

                watch.Stop();
                Sequencial[i, 1] = watch.ElapsedMilliseconds * 1000;

                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Sequencial(ref dados, id_final);

                watch.Stop();
                Sequencial[i, 2] = watch.ElapsedMilliseconds * 1000;

                #endregion

            }

            //--> Teste Sequencial
            FileStream arq = new FileStream("sequential.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            double[] r = new double[3];
            r = MediaTempo(Sequencial);

            write.WriteLine("{0};{1};{2}", r[0], r[1], r[2]);


            write.Close();
            arq.Close();

        }
        #endregion


        #region Binary Search
        public static void Test_BinarySearch(ref Registro[] dados, int id_initial, int id_mid, int id_final)
        {
            double[,] PesquisaBinaria = new double[5, 3];

            //--> Pesquisa Binaria
            Registro[] DadosOrdenados = QuickSort(dados, 0, dados.Length - 1);


            for (int i = 0; i < 5; i++)
            {

                //Pesquisa Binaria -> room_id
                #region PesquisaBinaria

                var watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Binaria(ref DadosOrdenados, id_initial);

                watch.Stop();
                PesquisaBinaria[i, 0] = watch.ElapsedMilliseconds * 1000;


                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Binaria(ref DadosOrdenados, id_mid);

                watch.Stop();
                PesquisaBinaria[i, 1] = watch.ElapsedMilliseconds * 1000;

                watch = System.Diagnostics.Stopwatch.StartNew();

                Pesquisa_Binaria(ref DadosOrdenados, id_final);

                watch.Stop();
                PesquisaBinaria[i, 2] = watch.ElapsedMilliseconds * 1000;

                #endregion
               
            }

            //--> Teste Pesquisa Binaria
            FileStream arq = new FileStream("BinarySearch.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            double[] r = new double[3];
            r = MediaTempo(PesquisaBinaria);

            write.WriteLine("{0};{1};{2}", r[0], r[1], r[2]);


            write.Close();
            arq.Close();


        }


        #endregion


        #region Binary Tree

        public static void Test_BinaryTree(ref Registro[] dados, int id_initial, int id_mid, int id_final)
        {

            double[,] ArvoreBinaria = new double[5, 3];

            //--> Arvore
            ArvoreBinaria arvore = new ArvoreBinaria();

            for (int i = 0; i < dados.Length; i++)
                arvore.Inserir(dados[i]);

            for (int i = 0; i < 5; i++)
            {

                //Arvore Binaria --> room_id
                #region ArvoreBinaria
                var watch = System.Diagnostics.Stopwatch.StartNew();

                arvore.Pesquisar(id_initial);

                watch.Stop();
                ArvoreBinaria[i, 0] = watch.ElapsedMilliseconds * 1000;


                watch = System.Diagnostics.Stopwatch.StartNew();

                arvore.Pesquisar(id_mid);

                watch.Stop();
                ArvoreBinaria[i, 1] = watch.ElapsedMilliseconds * 1000;

                watch = System.Diagnostics.Stopwatch.StartNew();

                arvore.Pesquisar(id_final);

                watch.Stop();
                ArvoreBinaria[i, 2] = watch.ElapsedMilliseconds * 1000;
                #endregion

            }

            FileStream arq = new FileStream("BinaryTree.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            double[] r = new double[3];
            r = MediaTempo(ArvoreBinaria);

            write.WriteLine("{0};{1};{2}", r[0], r[1], r[2]);


            write.Close();
            arq.Close();



        }

        #endregion


        #region Table Hash

        public static void Test_HashTable(ref Registro[] dados, int id_initial, int id_mid, int id_final)
        {

            double[,] TabelaHash = new double[5, 3];

            //--> TabelaHash
            TabelaHash hash = new TabelaHash(63647);

            for (int i = 0; i < dados.Length; i++)
                hash.Inserir(dados[i]);

            for (int i = 0; i < 5; i++)
            {
                //Tabela Hash --> room_id
                #region TabelaHash
                var watch = System.Diagnostics.Stopwatch.StartNew();

                hash.Pesquisar(id_initial);

                watch.Stop();
                TabelaHash[i, 0] = watch.ElapsedMilliseconds * 1000;


                watch = System.Diagnostics.Stopwatch.StartNew();

                hash.Pesquisar(id_mid);

                watch.Stop();
                TabelaHash[i, 1] = watch.ElapsedMilliseconds * 1000;

                watch = System.Diagnostics.Stopwatch.StartNew();

                hash.Pesquisar(id_final);

                watch.Stop();
                TabelaHash[i, 2] = watch.ElapsedMilliseconds * 1000;
                #endregion

            }




            FileStream arq = new FileStream("HashTable.txt", FileMode.Append);
            StreamWriter write = new StreamWriter(arq);

            double[] r = new double[3];
            r = MediaTempo(TabelaHash);

            write.WriteLine("{0};{1};{2}", r[0], r[1], r[2]);


            write.Close();
            arq.Close();
        }

        #endregion

        #endregion

        //teste --> Número de quartos
        #region bedrooms

        public static void test_bedroom(ref Registro[] dados)
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


        #endregion



    }
}