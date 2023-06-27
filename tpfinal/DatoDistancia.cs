using System;
using System.Collections.Generic;

namespace tpfinal
{

	[Serializable]
	public class DatoDistancia
	{
		public int distancia { get; set; }
		public string texto { get; set; } // String of symbols

		public string descripcion { get; set; } // String of symbols


		public DatoDistancia(int distancia, string texto, string descripcion)
		{
			this.distancia = distancia;
			this.texto = texto;
			this.descripcion = descripcion;
		}




		public override string ToString()
		{
			if (texto != null)
			{

				return "(" + distancia + ") " + texto;

			}
			else
			{

				return "";
			}
		}

	}
}