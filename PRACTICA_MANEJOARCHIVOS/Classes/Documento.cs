using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRACTICA_MANEJOARCHIVOS.Classes
{
    [Serializable]
    public class Documento
    {
        [XmlAttribute]
        public int IdDocumento { get; set; }
        [XmlAttribute]
        public string NroDocumento { get; set; }
    }
}
