using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



public class ServerHosting : MonoBehaviour
{

    static Socket listener;
    private CancellationTokenSource tokenSource;
    public ManualResetEvent allDone;
    public Renderer objectRenderer; //irrelevant in later processes
    private Color matColor; //irrelevant in later processes

    public static readonly int PORT = 9879;
    public static readonly int WAITTIME = 1;

    public int SocketListeningTime = 10;


    ServerHosting()
    {
        tokenSource = new CancellationTokenSource();
        allDone = new ManualResetEvent(false);
    }

    // Start is called before the first frame update
    async void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        await Task.Run(() => ListenEvents(tokenSource.Token));
    }

    // Update is called once per frame
    void Update()
    {
        objectRenderer.material.color = matColor;
    }

    private void ListenEvents(CancellationToken token)
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, PORT);

        listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(SocketListeningTime);

            while (!tokenSource.IsCancellationRequested)
            {
                allDone.Reset();
                print("Waiting for a connection... host:" + ipAddress.MapToIPv4().ToString() + " port: " + PORT);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                while (!tokenSource.IsCancellationRequested)
                {
                    if (allDone.WaitOne(WAITTIME))
                    {
                        break;
                    }
                }

            }
            
        }
        catch (Exception e)
        {
            print(e.ToString());
        }

    }

    void AcceptCallback(IAsyncResult ar)
    {
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        allDone.Set();

        StateObject state = new StateObject();
        state.workSocket = handler;
        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

    }

    void ReadCallback(IAsyncResult ar)
    {
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;

        int read = handler.EndReceive(ar);

        if(read > 0)
        {
            state.colorCoder.Append(Encoding.ASCII.GetString(state.buffer, 0, read));
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }
        else
        {
            if(state.colorCoder.Length > 1)
            {
                string content = state.colorCoder.ToString();
                print($"Read {content.Length} bytes from socket.\n Data : {content}");
                SetColors(content);
            }
            handler.Close();
        }
    }
    
    //This function delet when we change to use this Hosting system.
    private void SetColors (string data)
    {
        string[] colors = data.Split(',');
        matColor = new Color()
        {
            r = float.Parse(colors[1]) / 255f,
            g = float.Parse(colors[2]) / 255f,
            b = float.Parse(colors[3]) / 255f,
            a = float.Parse(colors[4]) / 255f,
        };
    }

    private void OnDestroy()
    {
        tokenSource.Cancel();
    }

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder colorCoder = new StringBuilder(); //dies hier ändern, ist aus dem Tutorial, wahrscheinlich muss hier der Json String hin.
    }

}
