using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using tp1;

namespace tpfinal
{
    internal class Utils
    {
        private static string patron;
        public static int lineCount;

        public static void init_patron()
        {
            patron = System.IO.Directory.GetCurrentDirectory() + "\\datasets\\dataset.csv";
            lineCount = File.ReadLines(@patron).Count();
        }

        public static void set_patron(string patron_parm)
        {
            patron = patron_parm;
            lineCount = File.ReadLines(@patron).Count();
        }
        public static string get_patron()
        {
            if (patron == null)
            {
                patron = System.IO.Directory.GetCurrentDirectory() + "\\datasets\\dataset.csv";
                lineCount = File.ReadLines(@patron).Count();

            }
            
            return patron;  
        }



        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            //string str = str_input.Replace(' ', '_');
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
       /* public static ArbolGeneral<DatoDistancia> indexing(ProgressBar progressBar1)
        {
            ArbolGeneral<DatoDistancia> arbol = null;
            Estrategia estrategia = new Estrategia();
            MethodInvoker m = new MethodInvoker(() => progressBar1.Maximum = lineCount);
            progressBar1.Invoke(m);
            m = new MethodInvoker(() => progressBar1.Step = 1);
            progressBar1.Invoke(m);

            using (TextFieldParser parser = new TextFieldParser(@patron))
            {
              
              parser.TextFieldType = FieldType.Delimited;
              parser.SetDelimiters(",");
              string[] columns = parser.ReadFields();
              string[] fields = parser.ReadFields();
              string titulo = RemoveSpecialCharacters(fields[1]);
              string descript = RemoveSpecialCharacters(fields[2]);
              arbol = new ArbolGeneral<DatoDistancia>(new DatoDistancia(0, titulo, descript));
              while (!parser.EndOfData)
              {
                fields = parser.ReadFields();
                titulo = RemoveSpecialCharacters(fields[1]);
                descript = RemoveSpecialCharacters(fields[2]);
                Console.WriteLine("* Procesando: " + titulo);
                estrategia.AgregarDato(arbol, new DatoDistancia(0, titulo, descript));
                m = new MethodInvoker(() => progressBar1.PerformStep());
                progressBar1.Invoke(m);
                Thread.Sleep(1);
               }
            }
            return arbol;
        }*/
        /*
        * Levenshtein distance
        * http://en.wikipedia.org/wiki/Levenshtein_distance
        *
        * The original author of this method in Java is Josh Clemm
        * http://code.google.com/p/java-bk-tree
        *
        */
        static public int calculateLevenshteinDistance(string source, string target)
        {
            int[,] distance; // distance matrix
            int n; // length of first string
            int m; // length of second string
            int i; // iterates through first string
            int j; // iterates through second string
            char s_i; // ith character of first string
            char t_j; // jth character of second string
            int cost; // cost

            // Step 1
            n = source.Length;
            m = target.Length;
            if (n == 0)
                return m;
            if (m == 0)
                return n;
            distance = new int[n + 1, m + 1];

            // Step 2
            for (i = 0; i <= n; i++)
                distance[i, 0] = i;
            for (j = 0; j <= m; j++)
                distance[0, j] = j;

            // Step 3
            for (i = 1; i <= n; i++)
            {
                s_i = source[i - 1];

                // Step 4
                for (j = 1; j <= m; j++)
                {
                    t_j = target[j - 1];

                    // Step 5
                    if (s_i == t_j)
                        cost = 0;
                    else
                        cost = 1;

                    // Step 6
                    distance[i, j] =
                        Math.Min(
                            Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                            distance[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return distance[n, m];
        }
    }
}
