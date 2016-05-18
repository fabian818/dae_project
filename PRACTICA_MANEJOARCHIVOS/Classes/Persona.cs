using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRACTICA_MANEJOARCHIVOS.Classes
{
    [Serializable]
    public class Persona
    {
        [XmlAttribute]
        public string Nombre { get; set; }
        [XmlAttribute]
        public string Apellidos { get; set; }
        [XmlAttribute]
        public string Direccion { get; set; }
        [XmlAttribute]
        public string Edad { get; set; }
        [XmlAttribute(DataType = "date")]
        public DateTime FechaNacimiento { get; set; }

        [XmlArray]
        public List<Documento> Documentos { get; set; }

    }
}
