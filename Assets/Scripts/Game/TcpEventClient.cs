// using UnityEngine;
// using System;
// using System.Net;
// using System.Net.Sockets;
// using System.Text;
// using System.Threading;
// using System.IO;
// using System.Collections.Concurrent;

// public class TcpEventClient : MonoBehaviour
// {
//     [Header("Server")]
//     public string serverIp = "192.168.68.102"; // IPv4 Windows-компа
//     public int serverPort = 5050;
//     public int connectTimeoutMs = 4000;

//     Thread receiveThread;
//     volatile bool running;
//     TcpClient client;
//     StreamReader reader;

//     ConcurrentQueue<string> eventQueue = new ConcurrentQueue<string>();

//     void Start()
//     {
//         Debug.Log("[TCP] Client starting...");
//         running = true;
//         receiveThread = new Thread(ReceiveLoop) { IsBackground = true };
//         receiveThread.Start();
//     }

//     void ReceiveLoop()
//     {
//         while (running)
//         {
//             try
//             {
//                 // парсимо явно IPv4 та форсимо AddressFamily.InterNetwork
//                 if (!IPAddress.TryParse(serverIp.Trim(), out var ip))
//                 {
//                     Debug.LogError("[TCP] serverIp is not a valid IP address");
//                     Thread.Sleep(2000);
//                     continue;
//                 }
//                 var endPoint = new IPEndPoint(ip, serverPort);
//                 Debug.Log($"[TCP] Trying to connect (IPv4) to {endPoint} ...");

//                 client = new TcpClient(AddressFamily.InterNetwork);
//                 client.NoDelay = true;

//                 // власний таймаут конекту
//                 IAsyncResult ar = client.BeginConnect(endPoint.Address, endPoint.Port, null, null);
//                 bool ok = ar.AsyncWaitHandle.WaitOne(connectTimeoutMs);
//                 if (!ok)
//                 {
//                     client.Close();
//                     Debug.LogError("[TCP] Connect timeout");
//                     Thread.Sleep(2000);
//                     continue;
//                 }
//                 client.EndConnect(ar);

//                 reader = new StreamReader(client.GetStream(), Encoding.UTF8);
//                 Debug.Log("[TCP] Connected to server!");

//                 while (running && client.Connected)
//                 {
//                     string line = reader.ReadLine(); // JSON per line
//                     if (line == null)
//                     {
//                         Debug.LogWarning("[TCP] Stream closed by server (null line)");
//                         break;
//                     }
//                     if (string.IsNullOrWhiteSpace(line))
//                     {
//                         Debug.LogWarning("[TCP] Received empty line");
//                         continue;
//                     }

//                     Debug.Log($"[TCP] Raw data: {line}");
//                     string value = ExtractValue(line);

//                     if (!string.IsNullOrEmpty(value))
//                     {
//                         Debug.Log($"[TCP] Parsed value: {value}");
//                         eventQueue.Enqueue(value);
//                     }
//                     else
//                     {
//                         Debug.LogWarning("[TCP] Could not parse 'value' from JSON");
//                     }
//                 }
//             }
//             catch (SocketException sx)
//             {
//                 Debug.LogError($"[TCP][SocketException] {sx.SocketErrorCode} :: {sx.Message}\n{sx}");
//                 Thread.Sleep(2000);
//             }
//             catch (Exception ex)
//             {
//                 Debug.LogError($"[TCP][Exception] {ex.Message}\n{ex}");
//                 Thread.Sleep(2000);
//             }
//             finally
//             {
//                 Debug.Log("[TCP] Closing connection...");
//                 try { reader?.Close(); } catch {}
//                 try { client?.Close(); } catch {}
//             }
//         }
//     }

//     string ExtractValue(string json)
//     {
//         var key = "\"value\":\"";
//         int i = json.IndexOf(key, StringComparison.Ordinal);
//         if (i < 0) return null;
//         i += key.Length;
//         int j = json.IndexOf("\"", i, StringComparison.Ordinal);
//         if (j < 0) return null;
//         return json.Substring(i, j - i);
//     }

//     void Update()
//     {
//         while (eventQueue.TryDequeue(out var value))
//         {
//             Debug.Log($"[TCP] Event in Unity: {value}");
//             // TODO: реакції в грі
//         }
//     }

//     void OnApplicationQuit()
//     {
//         Debug.Log("[TCP] Stopping client...");
//         running = false;
//         try { reader?.Close(); } catch {}
//         try { client?.Close(); } catch {}
//         try { receiveThread?.Abort(); } catch {}
//     }
// }

// using UnityEngine;
// using System.Net.Sockets;

// public class TestTCP : MonoBehaviour {
//     void Start() {
//         try {
//             var c = new TcpClient("192.168.68.102", 5050);
//             Debug.Log("CONNECTED OK");
//             c.Close();
//         } catch (SocketException e) {
//             Debug.LogError($"CONNECT FAIL: {e.Message}");
//         }
//     }
// }