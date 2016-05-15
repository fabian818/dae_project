using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;
using System.Security.Permissions;

namespace BibliotecaClases.IO
{
    public class ManejadorArchivosController : IStreams
    {
        
        /// <summary>
        /// Lee el contenido de un archivo de texto
        /// </summary>
        /// <param name="ruta">Ruta física del archivo</param>
        /// <returns></returns>
        public string Leer(string ruta)
        {
            string Lectura = "";
            if (File.Exists(ruta))
            {
                try
                {
                    using (var f = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        using (var sr = new StreamReader(f))
                        {
                            Lectura = sr.ReadToEnd();
                            sr.Close();
                        }

                        f.Close();
                    }
                    return Lectura;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new ArgumentException("La ruta es inválida");
            }

        }

        /// <summary>
        /// Comprime un archivo de texto
        /// </summary>
        /// <param name="Origen">ruta física del archivo a comprimir</param>
        /// <param name="Destino">nueva ruta física del archivo comprimido</param>
        public void ComprimirGZIP(string Origen, string Destino)
        {
            using (FileStream FsOrigen = File.OpenRead(Origen))
            {
                using (FileStream FsDestino = File.Open(Destino, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (GZipStream FsComprimir = new GZipStream(FsDestino, CompressionMode.Compress))
                    {
                        int bData = FsOrigen.ReadByte();
                        while (bData != -1)
                        {
                            FsComprimir.WriteByte((byte)bData);
                            bData = FsOrigen.ReadByte();
                        }
                        FsComprimir.Close();
                    }
                }
                FsOrigen.Close();
            }
        }
        
        /// <summary>
        /// Descomprimir un archivo
        /// </summary>
        /// <param name="Origen">Ruta física del archivo a descomprimir</param>
        /// <param name="Destino">Nueva ruta física del archivo descomprimido</param>
        public void DesComprimirGZIP(string Origen, string Destino)
        {
            try
            {
                using (FileStream FsOrigen = File.OpenRead(Origen))
                {
                    using (FileStream FsDestino = new FileStream(Destino, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (GZipStream FsComprimir = new GZipStream(FsOrigen, CompressionMode.Decompress))
                            
                        {
                            int bData = FsComprimir.ReadByte();
                            while (bData != -1)
                            {
                                //Escribe un byte en la posicion actual
                                FsDestino.WriteByte((byte)bData);
                                bData = FsComprimir.ReadByte();
                            }
                            FsComprimir.Close();
                        }
                        FsDestino.Close();
                    }
                    FsOrigen.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
       

     
       /// <summary>
       /// Escribe en un archivo de texto
       /// </summary>
       /// <param name="pRuta">Ruta física del archivo a escribir</param>
       /// <param name="Contenido">Texto a ser escrito en el archivo</param>
       /// <returns></returns>
        public bool Escribir(string pRuta, string Contenido)
        { 
            using (FileStream Fs = new FileStream(pRuta, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // StreamWriter se usa para escribir sobre el archivo
                using (StreamWriter Sw = new StreamWriter(Fs))
                {
                    Sw.WriteLine(Contenido);

                    Sw.Close();
                }
                Fs.Close();

                return true;
            }
        }


    }
}
