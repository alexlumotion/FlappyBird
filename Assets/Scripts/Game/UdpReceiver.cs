using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System;

public class UdpReceiver : MonoBehaviour
{
    public int port = 5005;

    public UnityEngine.Events.UnityEvent<string> OnAnyEvent;
    public UnityEngine.Events.UnityEvent OnMoveLeft;
    public UnityEngine.Events.UnityEvent OnMoveRight;
    public UnityEngine.Events.UnityEvent OnNoPerson;

    Thread t;
    UdpClient client;
    ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
    volatile bool running;

    [Serializable]
    public class EventMsg { public string type; public string value; public double ts; public float x, speed, delta, dist; }

    void Start()
    {
        Debug.Log($"[UDP] Listening on :{port}");
        running = true;
        t = new Thread(Listen) { IsBackground = true };
        t.Start();
    }

    void Listen()
    {
        try
        {
            client = new UdpClient(port);
            // (не обов'язково) таймаут, щоб раз на N мс повертатись в цикл і перевіряти running
            client.Client.ReceiveTimeout = 1000;

            while (running)
            {
                try
                {
                    IPEndPoint any = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = client.Receive(ref any);     // блокуючий виклик
                    string msg = Encoding.UTF8.GetString(data);
                    foreach (var line in msg.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        queue.Enqueue(line.Trim());
                }
                catch (SocketException se)
                {
                    // 10060 = timeout; 10004/10038/operation aborted — коли закриваємо сокет під час Receive
                    if (!running)
                        break; // виходимо тихо при зупинці
                    if (se.SocketErrorCode == SocketError.TimedOut)
                        continue; // просто чекали таймаут
                    Debug.LogWarning($"[UDP] SocketException: {se.SocketErrorCode}");
                }
                catch (ObjectDisposedException)
                {
                    // client закрито під час виходу
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            if (running) // логуй тільки якщо це не штатний вихід
                Debug.LogError($"[UDP] Exception: {ex.Message}");
        }
        // тут потік завершується
    }

    void Update()
    {
        while (queue.TryDequeue(out var line))
        {
            EventMsg evt = null;
            try { evt = JsonUtility.FromJson<EventMsg>(line); } catch { /* ok */ }
            var value = evt != null && !string.IsNullOrEmpty(evt.value) ? evt.value : line;

            Debug.Log($"[UDP] Event: {value}");
            OnAnyEvent?.Invoke(value);
            if (string.Equals(value, "MOVE_LEFT", StringComparison.OrdinalIgnoreCase))  OnMoveLeft?.Invoke();
            else if (string.Equals(value, "MOVE_RIGHT", StringComparison.OrdinalIgnoreCase)) OnMoveRight?.Invoke();
            else if (string.Equals(value, "NO_PERSON", StringComparison.OrdinalIgnoreCase))  OnNoPerson?.Invoke();
        }
    }

    void OnDisable()    { StopReceiver(); }
    void OnApplicationQuit() { StopReceiver(); }

    void StopReceiver()
    {
        if (!running) return;
        running = false;

        try { client?.Close(); } catch { }
        try
        {
            if (t != null && t.IsAlive)
                t.Join(300); // трохи зачекати, щоб потік завершився
        }
        catch { /* не страшно */ }
    }
}