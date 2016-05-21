using Microsoft.VisualStudio.TestTools.UnitTesting;
using BibliotecaClases.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ManejadorArchivos.Tests
{
    [TestClass()]
    public class ManejadorArchivosTests
    {
        [TestMethod()]
        public void EscribirTest()
        {
            //En esta prueba se valida la correcta ejecución del método Escribir 

            //Arrange
            string texto = "Nuevo texto";

            //RUTA EXISTENTE
            string path = @"C:\demoarchivos\Archivo01.txt";

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.Escribir(path, texto);
            
        }

        [TestMethod()]
        [ExpectedException(typeof(IOException))]
        public void EscribirFakePathTest()
        {
            //Esta prueba devuelve una excepción por la ruta no encontrada en el método Escribir
            //Arrange
            string texto = "asd";

            //RUTA NO EXISTENTE
            string fakePath = @"C:\Fake\Fakefile.txt";

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.Escribir(fakePath, texto);
            

        }

        [TestMethod()]
        public void LeerTest()
        {
            //En esta prueba se valida la correcta ejecución del método Leer 

            //Arrange
            
            //RUTA EXISTENTE
            string path = @"C:\demoarchivos\Archivo01.txt";

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.Leer<StreamReader>(path);
            
        }

        [TestMethod()]
        [ExpectedException(typeof(IOException))]
        public void LeerFakePathTest()
        {
            //Esta prueba devuelve una excepción por la ruta no encontrada en el método Leer
            //Arrange
            
            //RUTA NO EXISTENTE
            string fakePath = @"C:\Fake\Fakefile.txt";

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.Leer<StreamReader>(fakePath);

        }

        [TestMethod()]
        public void ComprimirGZIPTest()
        {
            //En esta prueba se valida la correcta ejecución del método ComprimirGZIP 

            //Arrange

            //RUTA EXISTENTE
            string origen = @"C:\demoarchivos\Archivo01.txt";
            string destino = origen + ".rar";

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.ComprimirGZIP(origen, destino);

        }

        [TestMethod()]
        [ExpectedException(typeof(IOException))]
        public void ComprimirGZIPFakePathTest()
        {
            //Esta prueba devuelve una excepción por la ruta no encontrada en el método ComprimirGZIP
            //Arrange

            //RUTAS NO EXISTENTES
            string origen = @"C:\Fake\Fakefile.txt";            
            string destino = origen + ".rar";

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.ComprimirGZIP(origen, destino);

        }

        [TestMethod()]
        public void DesComprimirGZIPTest()
        {
            //En esta prueba se valida la correcta ejecución del método DesComprimirGZIP 

            //Arrange

            //RUTAS EXISTENTES
            string origen = @"C:\demoarchivos\Archivo01.txt.rar";
            string destino = System.IO.Path.GetDirectoryName(origen);

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.DesComprimirGZIP(origen, destino);

        }

        [TestMethod()]
        [ExpectedException(typeof(IOException))]
        public void DesComprimirGZIPFakePathTest()
        {
            //Esta prueba devuelve una excepción por la ruta no encontrada en el método DesComprimirGZIP
            //Arrange

            //RUTAS NO EXISTENTES
            string origen = @"C:\Fake\Fakefile.txt.rar";
            string destino = System.IO.Path.GetDirectoryName(origen);

            var fileWriter = new ManejadorArchivosController();

            // Act
            fileWriter.DesComprimirGZIP(origen, destino);
        }
    }
}