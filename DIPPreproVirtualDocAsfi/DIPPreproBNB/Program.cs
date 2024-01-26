using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Collections;

namespace DIPPreproBNB
{
    class Program
    {
        public class TiposDocumentos
        {

            public string TipoDocumento { get; set; }
            public string value { get; set; }

        }

        
        public static int Main(string[] args)
        {
            string value = string.Empty;
            string TipoDocumento = string.Empty;
            System.Xml.XmlDocument oXML;
            System.Xml.XmlElement root;
            System.Xml.XmlNodeList elemList;
            System.IO.StreamWriter oWriter = null;
            StreamWriter log = null;

            try
            {
                ArrayList listaDoc = new ArrayList();
                ArrayList listaDType = new ArrayList();

                TiposDocumentos tp = new TiposDocumentos();
                
                log = new StreamWriter(@"c:\tempBC\log.txt");
                log.WriteLine("Envia parametro de entrada/salida");

                //System.IO.StreamReader textFile = new System.IO.StreamReader(@"D:\ASFI Prueba\BNB_DIP.txt");
                //oWriter = new System.IO.StreamWriter(@"D:\ASFI Prueba\resultado.txt", true);
                System.IO.StreamReader textFile = new System.IO.StreamReader(args[0]); //produccion
                oWriter = new System.IO.StreamWriter(args[1], true, Encoding.Default);


                log.WriteLine("Inicia lectura de XML");
                //StreamReader textFile = new StreamReader(args[0]);
                //StreamWriter oWriter = new StreamWriter(args[1], true);

                oXML = new System.Xml.XmlDocument();

                log.WriteLine("ruta de xml");

                oXML.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).ToString() + "\\XmlTipoDocumentos.xml");


                root = oXML.DocumentElement;
                elemList = root.GetElementsByTagName("book");
                log.WriteLine("Obtiene elementos del XML");
                //System.Xml.XmlNode node = (elemList[0]);
                //int i = elemList.Count;

                //carga de xml tipos de documentos
                log.WriteLine("Carga de los Tipos de Documentos");
                if (elemList != null)
                {
                    foreach (System.Xml.XmlNode oNode in elemList)
                    {
                        tp = new TiposDocumentos();
                        foreach (System.Xml.XmlNode oNode2 in oNode)
                        {
                            switch (oNode2.Name)
                            {
                                case "value": tp.value = oNode2.InnerText;
                                    break;
                                case "TipoDocumento": tp.TipoDocumento = oNode2.InnerText;
                                    break;

                                //default: break;
                            }
                        }
                        listaDoc.Add(tp);
                    }
                }
                log.WriteLine("Finaliza la obtencion de Tipos de Documentos");


                string path = "";
                string app = "";

                //leer los documentos del directorio
                log.WriteLine("Inicia a leer directorio con imagenes");

                oXML = new System.Xml.XmlDocument();
                oXML.Load(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\XmlTipoDocumentos.xml");
                root = oXML.DocumentElement;

                elemList = root.GetElementsByTagName("path");
                System.Xml.XmlNode node = (elemList[0]);
                path = node.InnerText;

                elemList = root.GetElementsByTagName("app");
                System.Xml.XmlNode nodo = (elemList[0]);
                app = nodo.InnerText;

                if (app == "Tagged")
                {
                    string[] documents = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    ArrayList texto = new ArrayList();

                    //string rutaFinal = "";

                    log.WriteLine("Cant archivos: " + documents.Length.ToString());
                    foreach (string ruta in documents)
                    {
                        //rutaFinal = "";
                        string cuerpoTXT = string.Empty;

                        string[] d = ruta.Split('\\');

                        int n = d.Length;
                        int pos = d[n - 1].LastIndexOf('.');

                        string nombreArchivo = d[n - 1].Substring(0, pos);

                        string[] datosImagen = nombreArchivo.Split('_');
                        int cantidadDatosImagen = datosImagen.Length;
                        string tipoDocumentoImg = "";

                        if (cantidadDatosImagen > 3)
                            tipoDocumentoImg = nombreArchivo.Split('_')[0];

                        //obtener tipoDocumento

                        foreach (TiposDocumentos item in listaDoc)
                        {
                            if (item.TipoDocumento.ToUpper().Equals(tipoDocumentoImg.ToUpper()))
                            {
                                if (cantidadDatosImagen == 4) //Credito(CIC)
                                {
                                    string fecha = datosImagen[3].Substring(0, 2) + "/" + datosImagen[3].Substring(2, 2) + "/" + datosImagen[3].Substring(4, 4);
                                    //rutaFinal = item.value + ";" + datosImagen[1] + ";" + datosImagen[2] + ";" + fecha + ";" + ruta;
                                    cuerpoTXT += "BEGIN:" + Environment.NewLine;
                                    cuerpoTXT += ">>DocTypeName:" + item.value + Environment.NewLine ;
                                    cuerpoTXT += "Numero de Identidad:" + datosImagen[1] + Environment.NewLine;
                                    cuerpoTXT += "Nombre/Razon Social:" + datosImagen[2] + Environment.NewLine;
                                    cuerpoTXT += ">>Fecha Consulta:" + fecha + Environment.NewLine;
                                    cuerpoTXT += ">>FullPath:" + ruta + Environment.NewLine;
                                    cuerpoTXT += "END: \n";
                                    break;
                                }
                                if (cantidadDatosImagen == 6) //Credito(CIC)
                                {
                                    string fecha = datosImagen[5].Substring(0, 2) + "/" + datosImagen[5].Substring(2, 2) + "/" + datosImagen[5].Substring(4, 4);
                                    //rutaFinal = item.value + ";" + datosImagen[1] + ";" + datosImagen[2] + ";" + fecha + ";" + ruta;
                                    cuerpoTXT += "BEGIN:" + Environment.NewLine;
                                    cuerpoTXT += ">>DocTypeName:" + item.value + Environment.NewLine;
                                    cuerpoTXT += "Numero de Identidad:" + datosImagen[1] + Environment.NewLine;
                                    cuerpoTXT += "Nombre/Razon Social:" + datosImagen[2] + Environment.NewLine;
                                    cuerpoTXT += "Numero de Identidad:" + datosImagen[3] + Environment.NewLine;
                                    cuerpoTXT += "Nombre/Razon Social:" + datosImagen[4] + Environment.NewLine;
                                    cuerpoTXT += ">>Fecha Consulta:" + fecha + Environment.NewLine;
                                    cuerpoTXT += ">>FullPath:" + ruta + Environment.NewLine;
                                    cuerpoTXT += "END:" + Environment.NewLine;
                                    break;
                                }
                            }
                        }
                        //log.WriteLine("Linea: " + cuerpoTXT);
                        if (cuerpoTXT.Length > 0)
                            texto.Add(cuerpoTXT);
                    }
                    if (texto.Count > 0)
                    {

                        foreach (var item in texto)
                        {
                            //log.WriteLine("Escribe en archivo resultados");
                            oWriter.WriteLine(item.ToString());
                            oWriter.Flush();
                        }
                    }
                    else
                    { oWriter.Flush(); }

                    //return 1;
                }
                if (app == "Ordered")
                {
                    string[] documents = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                    ArrayList texto = new ArrayList();

                    string rutaFinal = "";

                    //log.WriteLine("Cant archivos: " + documents.Length.ToString());
                    foreach (string ruta in documents)
                    {
                        rutaFinal = "";
                        //string cuerpoTXT = string.Empty;

                        string[] d = ruta.Split('\\');

                        int n = d.Length;
                        int pos = d[n - 1].LastIndexOf('.');
                        //log.WriteLine("Obtengo la posicion: " + pos);
                        string[] extensionArchivo = d[n - 1].Split('.');
                        //log.WriteLine("Obtengo extension archivo: " + extensionArchivo[1]);
                        if (extensionArchivo[1].ToUpper() == "TIF" || extensionArchivo[1].ToUpper() == "TIFF")
                        {
                            string nombreArchivo = d[n - 1].Substring(0, pos);
                            log.WriteLine("Obtengo nombre del archivo: " + nombreArchivo);


                            string[] datosImagen = nombreArchivo.Split('_');
                            int cantidadDatosImagen = datosImagen.Length;
                            string tipoDocumentoImg = "";

                            if (cantidadDatosImagen > 9)
                                tipoDocumentoImg = nombreArchivo.Split('_')[0];
                            log.WriteLine("Obtengo codigo de Tipo de documento: " + tipoDocumentoImg);

                            //obtener tipoDocumento

                            foreach (TiposDocumentos item in listaDoc)
                            {
                                if (item.TipoDocumento.ToUpper().Equals(tipoDocumentoImg.ToUpper()))
                                {

                                    if (cantidadDatosImagen == 10) //Virtual Doc ASFI
                                    {
                                        string fecha = datosImagen[2].Substring(0, 2) + "/" + datosImagen[2].Substring(2, 2) + "/" + datosImagen[2].Substring(4, 4);
                                        rutaFinal = item.value + ";" + datosImagen[1] + ";" + fecha + ";" + datosImagen[3] + ";" + datosImagen[4] + ";" + datosImagen[5] + ";" + datosImagen[6] + ";" + datosImagen[7] + ";" + datosImagen[8] + ";" + datosImagen[9] + ";" + ruta;
                                        break;
                                    }
                                }
                            }
                            log.WriteLine("Linea: " + rutaFinal);
                            if (rutaFinal.Length > 0)
                                texto.Add(rutaFinal);
                        }
                    }
                    if (texto.Count > 0)
                    {

                        foreach (var item in texto)
                        {
                            log.WriteLine("Escribe en archivo resultados");
                            oWriter.WriteLine(item.ToString());
                            oWriter.Flush();
                        }
                    }
                    else
                    { oWriter.Flush(); }

                    //return 1;
                }
                return 1;
            }
            catch (Exception ex)
            {
                if (log != null)
                {
                    log.WriteLine("Error: " + ex.ToString());
                    log.Close();
                    log.Dispose();
                }
                if (oWriter != null)
                {
                    oWriter.Close();
                    oWriter.Dispose();
                    oWriter = null;
                }
                return 0;

            }
            finally
            {
                //textFile.Close();
                //textFile.Dispose();
                //textFile = null;
            }
        }

    }
}
