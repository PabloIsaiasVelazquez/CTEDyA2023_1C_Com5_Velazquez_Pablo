using System;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{
    public class Estrategia
    {
        private int CalcularDistancia(string str1, string str2)
        {
            // usando el método
            string[] strlist1 = str1.ToLower().Split(' ');
            string[] strlist2 = str2.ToLower().Split(' ');
            int distance = 1000;
            foreach (string s1 in strlist1)
            {
                foreach (string s2 in strlist2)
                {
                    distance = Math.Min(distance, Utils.calculateLevenshteinDistance(s1, s2));
                }
            }

            return distance;
        }

        public String Consulta1(ArbolGeneral<DatoDistancia> arbol)
		{
			string resutl = "";
			if (arbol.esHoja()) //si fuera hoja retorno su texto, el cual seria agregado a resutl
			{
				return arbol.getDatoRaiz().texto + "\n"; //la forma "\n" indica un salto de linea
			}
			else
			{
				foreach (ArbolGeneral<DatoDistancia> listaHijos in arbol.getHijos())
				{
					resutl += Consulta1(listaHijos); //se le agrega a resutl los textos de los hijos
				}
			}
			return resutl;
		}

        public string Consulta2(ArbolGeneral<DatoDistancia> arbol)
        {
            List<string> caminos = new List<string>(); // Lista donde se almacenarán todos los caminos posibles
            ExploradorCaminos(arbol, "", caminos); // Llena la lista "caminos" con los caminos
            string resultado = "";
            foreach (string camino in caminos)
            { // Cada "camino" es el conjunto de recorridos hasta una hoja 
                resultado += camino + "\n"; // Agrega el camino al resultado y hace un salto de línea
            }
            return resultado; // Devuelve el resultado final
        }

        private void ExploradorCaminos(ArbolGeneral<DatoDistancia> arbol, string caminoActual, List<string> caminos)
        {
            caminoActual += " --> " + arbol.getDatoRaiz().ToString(); // Agrega el texto del nodo actual al camino actual
            if (arbol.esHoja()) // Si el nodo actual es una hoja, se ha alcanzado el final del camino
            {
                caminos.Add(caminoActual); // Agrega el camino actual a la lista de caminos
                return; // Termina y vuelve a la recursión anterior
            }
            foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos()) // Itera sobre los hijos del nodo actual
            {
                ExploradorCaminos(hijo, caminoActual, caminos); // Llama recursivamente al explorador para cada hijo
            }
        }


        public string Consulta3(ArbolGeneral<DatoDistancia> arbol)
        {
            string resultado = ""; // Variable para almacenar el resultado final
            int nivel = 0; // Variable para rastrear el nivel actual
            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>(); // Creamos una cola utilizando la clase Cola<T>
            cola.encolar(arbol); // Encolamos el árbol raíz para comenzar el recorrido
            cola.encolar(null); // Encolamos un nulo al final para marcar el inicio del siguiente nivel

            while (!cola.esVacia())
            {
                ArbolGeneral<DatoDistancia> nodoActual = cola.desencolar(); // Desencolamos el nodo actual

                if (nodoActual != null) // Si el nodo actual no es nulo
                {
                    if (!string.IsNullOrEmpty(resultado))
                    {
                        resultado += " - "; // Agregamos un guion entre los elementos del mismo nivel
                    }

                    resultado += nodoActual.getDatoRaiz().texto; // Agregamos el texto del nodo actual al resultado

                    foreach (ArbolGeneral<DatoDistancia> hijo in nodoActual.getHijos()) // Iteramos sobre los hijos del nodo actual
                    {
                        cola.encolar(hijo); // Encolamos los hijos para continuar el recorrido en el siguiente nivel
                    }
                }
                else // Si el nodo actual es nulo, se alcanzó el final del nivel actual
                {
                    nivel++; // Incrementamos el nivel
                    resultado += "\n"; // Agregamos un salto de línea después de cada nivel
                    resultado += "nivel " + nivel + ":\n"; // Agregamos el número de nivel al resultado

                    if (!cola.esVacia())
                        cola.encolar(null); // Encolamos un nulo al final para marcar el inicio del siguiente nivel
                }
            }

            return resultado; // Devolvemos el resultado final
        }

        public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato)
        {
            if (arbol == null || dato == null)
            {
                return; // Se termina la función si alguno de los parámetros es nulo
            }

            DatoDistancia datoArbol = arbol.getDatoRaiz(); // Obtiene el dato del nodo actual del árbol
            int distancia = CalcularDistancia(datoArbol.texto, dato.texto); // Calcula la distancia entre el texto del nodo actual y el texto del nuevo dato a agregar

            if (distancia == 0) // Si la distancia es cero, significa que las palabras son iguales, por lo que no se agrega el nuevo dato
            {
                return; // Se termina la función
            }

            bool existeMismaDistancia = false; // Variable para determinar si ya existe un hijo con la misma distancia

            foreach (var hijo in arbol.getHijos()) // Itera sobre los hijos del nodo actual del árbol
            {
                DatoDistancia datoHijo = hijo.getDatoRaiz(); // Obtiene el dato del hijo actual
                int distanciaHijo = datoHijo.distancia; // Obtiene la distancia del hijo actual

                if (distancia == distanciaHijo) // Si la distancia es igual a la distancia del hijo actual
                {
                    existeMismaDistancia = true; // Marca que ya existe un hijo con la misma distancia
                    AgregarDato(hijo, dato); // Llama recursivamente a la función AgregarDato para agregar el nuevo dato al hijo correspondiente
                    break; // Sale del bucle, ya que hemos encontrado la misma distancia y agregado el dato
                }
            }

            if (!existeMismaDistancia) // Si no existe un hijo con la misma distancia
            {
                DatoDistancia nuevoDato = new DatoDistancia(distancia, dato.texto, dato.descripcion); // Crea un nuevo objeto DatoDistancia con la distancia calculada y los valores del nuevo dato
                ArbolGeneral<DatoDistancia> nuevoHijo = new ArbolGeneral<DatoDistancia>(nuevoDato); // Crea un nuevo árbol con el nuevo dato como raíz
                arbol.agregarHijo(nuevoHijo); // Agrega el nuevo árbol como hijo del árbol actual
            }
        }


        public void Buscar(ArbolGeneral<DatoDistancia> arbol, string elementoABuscar, int umbral, List<DatoDistancia> collected)
        {
            if (arbol == null || elementoABuscar == null || collected == null)
            {
                return; // Se termina la función si alguno de los parámetros es nulo
            }

            DatoDistancia datoArbol = arbol.getDatoRaiz(); // Obtiene el dato del nodo actual del árbol
            int distanciaArbolElemento = CalcularDistancia(datoArbol.texto, elementoABuscar); // Calcula la distancia entre el texto del nodo actual y el elemento a buscar

            if (distanciaArbolElemento <= umbral)
            {
                collected.Add(datoArbol); // Si la distancia es menor o igual al umbral, se agrega el dato al resultado
            }

            if (!arbol.esHoja())
            {
                foreach (var hijo in arbol.getHijos()) // Itera sobre los hijos del nodo actual
                {
                    Buscar(hijo, elementoABuscar, umbral, collected); // Llama recursivamente a la función Buscar para cada hijo
                }
            }
        }

    }
}
