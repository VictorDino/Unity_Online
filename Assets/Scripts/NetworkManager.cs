using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager _NETWORK_MANAGER;

    private TcpClient socket;
    private NetworkStream stream;

    private StreamWriter writer;
    private StreamReader reader;

    private bool connected = false;

    const string host = "192.168.1.36";
    const int port = 6543;

    private void Awake()
    {
        if (_NETWORK_MANAGER != null && _NETWORK_MANAGER != this)
        {
            Destroy(_NETWORK_MANAGER);
        }
        else
        {
            _NETWORK_MANAGER = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Update()
    {
        //Si estoy conectado reviso si existen datos
        if (connected)
        {
            //Si hay datos disponibles para leer
            if (stream.DataAvailable)
            {
                //Leo una linea de datos
                string data = reader.ReadLine();

                //Si los datos no son nulos los trabajo
                if (data != null)
                {
                    ManageData(data);
                }
            }
        }
    }
    public void ConnectToServer(string nick, string password)
    {
        try
        {
            //Instancia la clase para gestionar la conexion y el streaming de datos
            socket = new TcpClient(host, port);
            stream = socket.GetStream();

            //Si hay streaming de datos hay conexion
            connected = true;

            //Instancio clases de lectura y escritura
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //Envio 0 con nick y ususario separados por / ya que son los valores que he definido en el servidor
            writer.WriteLine("0" + "/" + nick + "/" + password);

            //Limpio el writer de datos
            writer.Flush();

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    private void ManageData(string data)
    {
        //Si recibo ping devuelvo 1 como respuesta al servidor
        if (data == "ping")
        {
            Debug.Log("Recibo ping");
            writer.WriteLine("1");
            writer.Flush();
        }
    }
}