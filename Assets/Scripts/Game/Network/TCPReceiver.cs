using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

public class TCPReceiver : MonoBehaviour
{
    [Header("Server")]
    public string serverIp = "127.0.0.1";
    public int serverPort = 5050;
    [Tooltip("Секунд між спробами перепідключення")]
    public float reconnectDelay = 2f;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent<string> OnAnyEvent;
    public UnityEngine.Events.UnityEvent OnMoveLeft;
    public UnityEngine.Events.UnityEvent OnMoveRight;
    public UnityEngine.Events.UnityEvent OnNoPerson;

    Thread worker;
    volatile bool running;
    TcpClient client;
    readonly ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

    [Serializable]
    public class EventMsg
    {
        public string type;
        public string value;
        public double ts;
        public float x, speed, delta, dist;
    }

    void Start()
    {
        running = true;
        worker = new Thread(Run) { IsBackground = true };
        worker.Start();
        Debug.Log($"[TCP] Client starting… target {serverIp}:{serverPort}");
    }

    void Run()
    {
        while (running)
        {
            try
            {
                Debug.Log($"[TCP] Trying to connect to {serverIp}:{serverPort}…");
                var c = new TcpClient();
                c.NoDelay = true;
                c.Connect(serverIp, serverPort);

                client = c;
                Debug.Log("[TCP] Connected");

                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line;
                    while (running && client.Connected && (line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                            queue.Enqueue(line.Trim());
                    }
                }

                Debug.Log("[TCP] Disconnected (server closed or stream ended)");
            }
            catch (SocketException se)
            {
                if (!running) break;
                Debug.LogWarning($"[TCP] SocketException: {se.SocketErrorCode}");
            }
            catch (Exception ex)
            {
                if (!running) break;
                Debug.LogWarning($"[TCP] Exception: {ex.GetType().Name}: {ex.Message}");
            }
            finally
            {
                try { client?.Close(); } catch { }
                client = null;
            }

            if (!running) break;
            Thread.Sleep(Mathf.Max(1, (int)(reconnectDelay * 1000)));
        }
    }

    void Update()
    {
        while (queue.TryDequeue(out var line))
        {
            EventMsg evt = null;
            try { evt = JsonUtility.FromJson<EventMsg>(line); } catch { /* ok */ }

            var value = (evt != null && !string.IsNullOrEmpty(evt.value)) ? evt.value : line;
            Debug.Log($"[TCP] Event: {value}  raw={line}");

            OnAnyEvent?.Invoke(value);

            if (string.Equals(value, "MOVE_LEFT", StringComparison.OrdinalIgnoreCase))
                OnMoveLeft?.Invoke();
            else if (string.Equals(value, "MOVE_RIGHT", StringComparison.OrdinalIgnoreCase))
                OnMoveRight?.Invoke();
            else if (string.Equals(value, "NO_PERSON", StringComparison.OrdinalIgnoreCase))
                OnNoPerson?.Invoke();

            // Якщо потрібні числа з payload:
            // if (evt != null) Debug.Log($"x={evt.x} v={evt.speed} d={evt.delta} dist={evt.dist}");
        }
    }

    void OnDisable()         => StopClient();
    void OnApplicationQuit() => StopClient();

    void StopClient()
    {
        if (!running) return;
        running = false;

        try { client?.Close(); } catch { }
        try
        {
            if (worker != null && worker.IsAlive)
                worker.Join(500);
        }
        catch { }
        Debug.Log("[TCP] Client stopped");
    }
}