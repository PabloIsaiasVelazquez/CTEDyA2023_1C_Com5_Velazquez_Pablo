using System;
using tp1;

namespace tpfinal
{ 
    public class Backend
    {
        public static ArbolGeneral<DatoDistancia> arbol = new ArbolGeneral<DatoDistancia>(new DatoDistancia(0,"."," "));

        public static string aProfundidad()
        {
            return (new Estrategia()).Consulta3(arbol);
        }

        public static string caminoAPrediccion()
        {
            return (new Estrategia()).Consulta2(arbol);
        }

        public static string todasLasPredicciones()
        {
            return (new Estrategia()).Consulta1(arbol);
        }

        public static void buscar(string elementoABuscar, int umbral, List<DatoDistancia> collected)
        {
            (new Estrategia()).Buscar(arbol, elementoABuscar, umbral, collected);
            collected.Sort((a, b) => a.distancia.CompareTo(b.distancia));
        }
    }

}