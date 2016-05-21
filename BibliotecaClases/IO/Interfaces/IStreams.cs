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
        string Leer<T>(String ruta) where T : TextReader;
        void ComprimirGZIP(string Origen, string Destino);
        void DesComprimirGZIP(string Origen, string Destino);
        bool Escribir(string pRuta, string Contenido);

        string SerializaXml<T>(T entidad) where T : class;
        object DesSerializaXml<T>(T entidad, string file) where T : class;
        object DesSerializaJson<T>(T entidad, string file) where T : class;
        string SerializaSoap<T>(T entidad) where T : class;
        string SerializaBinary<T>(T entidad) where T : class;
        string SerializaJson<T>(T entidad) where T : class;

    }
}
