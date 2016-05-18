using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PRACTICA_MANEJOARCHIVOS.Classes;
using System.Windows.Controls;
using BibliotecaClases.IO;
using System.IO;
using System.Security.Cryptography;
using System.IO.MemoryMappedFiles;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Configuration;

namespace PRACTICA_MANEJOARCHIVOS
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {          
            InitializeComponent();
            /* Es parte del listado para recorrer los directorios.
               Se usa para listas directorios.
            */
   
            DriveInfo[] cosas = DriveInfo.GetDrives();
            foreach (DriveInfo cosasInfo in cosas)
                ListadoArbol.Items.Add(CreararbolItem(cosasInfo));

        }

        private void btnLectura_Click(object sender, RoutedEventArgs e)
        {
            
            //Ruta del archivo a leer
            string sRuta = txtFileLeer.Text;
            
            //Llamamos a la clase de nuestro framework
            ManejadorArchivosController A = new ManejadorArchivosController();
            //Llamamos a la interfaz
            IStreams B = new ManejadorArchivosController();

            try
            {
                //Muestra el contenido del archivo leído
                MessageBox.Show(A.Leer(sRuta), "Contenido", MessageBoxButton.OK);
                txtc.Text = A.Leer(sRuta);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                        
        }


        private void btnEscribir_Click(object sender, RoutedEventArgs e)
        {

            ManejadorArchivosController A = new ManejadorArchivosController();
            IStreams escritura = new ManejadorArchivosController();
            try
            {
                //Escribe en el archivo
                A.Escribir(txtFileEscribir.Text, txtc.Text);
                MessageBox.Show("Archivo escrito correctamente.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        
        private void btnComprimir_Click(object sender, RoutedEventArgs e)
        {

            //Llamamos a nuestro framework
            IStreams A = new ManejadorArchivosController();

            try
            {
                //Especificando ruta origen y destino del archivo a comprimir
                string rutadestino = txtFileComprimir.Text + ".rar";
                A.ComprimirGZIP(@txtFileComprimir.Text, @rutadestino);
                MessageBox.Show("Archivo comprimido con éxito.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception Ex)
            {               
                MessageBox.Show(Ex.Message);
            }

        }
        
        private void btnDescomprimir_Click(object sender, RoutedEventArgs e)
        {
            //Llamamos a nuestro framework
            IStreams A = new ManejadorArchivosController();

            // Obtener el nombre del directorio del gz
            string path = @txtFileDesComprimir.Text;
            string DirectoryName = System.IO.Path.GetDirectoryName(path);

            try
            {
                //Descomprime el archivo               
                A.DesComprimirGZIP(@txtFileDesComprimir.Text, DirectoryName);
                MessageBox.Show("Archivo desencriptado con éxito.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception Ex)
            {               
                MessageBox.Show(Ex.Message);
            }

        }

        //Inicializamos los vectores de asignacion de claves aleatorias
        private byte[] bkey;
        private byte[] bIV;
        private void btnEncriptar_Click(object sender, RoutedEventArgs e)
        {
            TripleDESCryptoServiceProvider Algoritmo = new TripleDESCryptoServiceProvider();
            //Generación de clave aleatoria
            Algoritmo.GenerateKey(); 
            Algoritmo.GenerateIV();
            //Asignamos valores a los vectores
            bkey = Algoritmo.Key;
            bIV = Algoritmo.IV;
            
            ICryptoTransform Transformacion = Algoritmo.CreateEncryptor();

            //Ruta a encriptar
            string path = @txtFileEncriptar.Text;
            //Generando nueva ruta del archivo destino
            string file = System.IO.Path.GetFileNameWithoutExtension(path);
            string NewPath = path.Replace(file, file + "-encriptado");
            

            using (FileStream FsDestino = new FileStream(NewPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (CryptoStream Cs = new CryptoStream(FsDestino, Transformacion, CryptoStreamMode.Write))
                {
                    string Contenido = File.ReadAllText(@txtFileEncriptar.Text);
                    using (StreamWriter Sw = new StreamWriter(Cs))
                    {
                        Sw.Write(Contenido);
                        Sw.Close();
                        MessageBox.Show("Archivo encriptado con éxito.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                FsDestino.Close();
            }
        }

        private void btnDesencriptar_Click(object sender, RoutedEventArgs e)
        {
            
            TripleDESCryptoServiceProvider Algoritmo = new TripleDESCryptoServiceProvider();
            //Generación de clave aleatoria
            Algoritmo.GenerateKey();
            Algoritmo.GenerateIV();

            Algoritmo.Key = bkey;
            Algoritmo.IV = bIV;

            ICryptoTransform Transformacion = Algoritmo.CreateDecryptor();

            //Generando el nuevo nombre del archivo desencriptado
            string path = @txtFileDesEncriptar.Text;
            string file = System.IO.Path.GetFileNameWithoutExtension(path);
            string NewPath = path.Replace(file, file + "-desencriptado");

            using (FileStream FsOrigen = new FileStream(txtFileDesEncriptar.Text, FileMode.Open, FileAccess.Read))
            {
                using (CryptoStream Cs = new CryptoStream(FsOrigen, Transformacion, CryptoStreamMode.Read))
                {
                    using (StreamReader Sr = new StreamReader(Cs))
                    {
                        
                        string Cadena = Sr.ReadToEnd();
                        File.WriteAllText(NewPath, Cadena);
                        MessageBox.Show("Archivo desencriptado con éxito.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                FsOrigen.Close();
            }

        }


        
        private void btnExaminar_Click(object sender, RoutedEventArgs e)
        {
            //OPENFILE
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Asignando extensión de archivo predeterminada 
            dlg.DefaultExt = ".txt";

            // Se muestra el OpenFileDialog llamando el método ShowDialog 
            Nullable<bool> result = dlg.ShowDialog();
 
            if (result == true)
            {
                // Se muestra la ruta seleccionada
                txtFileLeer.Text = dlg.FileName;
            }
            
        }

        private void btnExaminarComprimir_Click(object sender, RoutedEventArgs e)
        {
            //OPENFILE
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Asignando extensión de archivo predeterminada 
            dlg.DefaultExt = ".txt";

            // Se muestra el OpenFileDialog llamando el método ShowDialog 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Se muestra la ruta seleccionada
                txtFileComprimir.Text = dlg.FileName;
            }
        }

        private void btnExaminarDesComprimir_Click(object sender, RoutedEventArgs e)
        {
            //OPENFILE
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Asignando extensión de archivo predeterminada 
            dlg.DefaultExt = ".txt";

            // Se muestra el OpenFileDialog llamando el método ShowDialog 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Se muestra la ruta seleccionada
                txtFileDesComprimir.Text = dlg.FileName;
            }
        }

        private void btnExaminarEscribir_Click(object sender, RoutedEventArgs e)
        {
            //OPENFILE
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Asignando extensión de archivo predeterminada 
            dlg.DefaultExt = ".txt";

            // Se muestra el OpenFileDialog llamando el método ShowDialog 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                txtFileEscribir.Text = dlg.FileName;
            }
        }

        private void btnExaminarEncriptar_Click(object sender, RoutedEventArgs e)
        {
            //OPENFILE
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Asignando extensión de archivo predeterminada 
            dlg.DefaultExt = ".txt";

            // Se muestra el OpenFileDialog llamando el método ShowDialog 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                txtFileEncriptar.Text = dlg.FileName;
            }
        }

        private void btnExaminarDesEncriptar_Click(object sender, RoutedEventArgs e)
        {
            //OPENFILE
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Asignando extensión de archivo predeterminada 
            dlg.DefaultExt = ".txt";

            // Se muestra el OpenFileDialog llamando el método ShowDialog 
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                txtFileDesEncriptar.Text = dlg.FileName;
            }
        }


        

        //Creación de los items del treeview o ramas para listar carpetas

        private object CreararbolItem(object directorio)
        {
            TreeViewItem itemdeltree = new TreeViewItem();
            itemdeltree.Header = directorio.ToString();
            itemdeltree.Tag = directorio;
            itemdeltree.Items.Add("Cargando . . .");
            return itemdeltree;
        }
        

        private void ListadoArbol_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem itemdeltree = e.Source as TreeViewItem;
            if ((itemdeltree.Items.Count == 1) && (itemdeltree.Items[0] is string))
            {
                itemdeltree.Items.Clear();

                DirectoryInfo expansion = null;
                if (itemdeltree.Tag is DriveInfo)
                    expansion = (itemdeltree.Tag as DriveInfo).RootDirectory;
                if (itemdeltree.Tag is DirectoryInfo)
                    expansion = (itemdeltree.Tag as DirectoryInfo);
                try
                {
                    foreach (DirectoryInfo subDir in expansion.GetDirectories())
                        itemdeltree.Items.Add(CreararbolItem(subDir));

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                try
                {
                    foreach (FileInfo archivo in expansion.GetFiles())
                        itemdeltree.Items.Add(CreararbolItem(archivo));

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void btnExaminarSerializarXML_Click(object sender, RoutedEventArgs e)
        {
            
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result.ToString() == "OK")
            {
                txtFileSerializarXML.Text = dialog.SelectedPath;
            }            
            
        }

        private void btnSerializarXML_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString))
            using (var cmd = new SqlCommand("dbo.usp_GetPersona", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                
                cmd.Parameters.AddWithValue("@IdPersona", Convert.ToInt32(txtIDPersona.Text));

                SqlDataReader dr = cmd.ExecuteReader();


                try
                {
                    if (dr.HasRows)
                    {
                        List<Documento> DocList = new List<Documento>();
                        Documento doc;



                        while (dr.Read())
                        {
                            doc = new Documento();
                            doc.IdDocumento = dr.GetInt32(dr.GetOrdinal("IdDocumento"));
                            doc.NroDocumento = dr.GetString(dr.GetOrdinal("NroDocumento"));

                            DocList.Add(doc);

                            var entidad = new Persona()
                            {
                                Nombre = dr.GetString(dr.GetOrdinal("Nombre")),
                                Apellidos = dr.GetString(dr.GetOrdinal("Apellidos")),
                                Direccion = dr.GetString(dr.GetOrdinal("Direccion")),
                                Edad = dr.GetString(dr.GetOrdinal("Edad")),
                                FechaNacimiento = dr.GetDateTime(dr.GetOrdinal("FechaNacimiento")),
                                Documentos = DocList

                            };

                            IStreams A = new ManejadorArchivosController();
                            var c = A.SerializaXml(entidad);
                            string ruta = txtFileSerializarXML.Text + @"\" + txtFileSerializarXMLNew.Text + ".txt";

                            A.Escribir(@ruta, c);
                        }


                        dr.NextResult();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró registros con el IdPersona ingresado.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    MessageBox.Show("Archivo serializado en XML correctamente.", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dr.Close();
                    dr.Dispose();
                }
            }
        }
    }
}