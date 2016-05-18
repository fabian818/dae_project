using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;

namespace BibliotecaClases.IO
{
    public interface IStreams
    {
        string Leer(string ruta);
        void ComprimirGZIP(string Origen, string Destino);
        void DesComprimirGZIP(string Origen, string Destino);
        bool Escribir(string pRuta, string Contenido);

        string SerializaXml<T>(T entidad) where T : class;
    }
}
