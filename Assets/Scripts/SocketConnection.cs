using JetBrains.Annotations;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Scripts
{
    public class SocketConnection : MonoBehaviour
    {
        /// <summary>
        ///     主机名或者IP地址
        ///     <remarks>
        ///         <para>doc.google.com (由于连接方在一段时间后没有正确答复或连接的主机没有反应，连接尝试失败。)</para>
        ///     </remarks>
        /// </summary>
        public string HostNameOrAddress;

        public int Port;

        private void OnGUI()
        {
            var rect = new Rect(50, 50, 200, 50);
            if (GUI.Button(rect, "Connect Sync"))
                Connect(HostNameOrAddress, Port);

            rect.y += 60;
            if (GUI.Button(rect, "Connect Async"))
            {
                var thread = new Thread(() => ConnectAsync(HostNameOrAddress, Port));
                thread.Start();
            }
        }

        /// <summary>
        /// 异步的连接
        /// </summary>
        /// <param name="hostNameOrAddress"></param>
        /// <param name="port"></param>
        public void ConnectAsync(string hostNameOrAddress, int port)
        {
            var remoteEndPoint = GetRemoteEndPoint(hostNameOrAddress, port);
            if (remoteEndPoint == null)
                return;

            // 如果连接失败, 如超时, 是不会有回调的也不会抛异常
            // 如果要使用asyncResult.AsyncWaitHandle.WaitOne, 也会被阻塞, 所以要放到线程中使用
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var asyncResult = socket.BeginConnect(remoteEndPoint, OnConnect, socket);
            var success = asyncResult.AsyncWaitHandle.WaitOne(5000);
            Debug.Log("Result: " + success);
        }

        private void OnConnect(IAsyncResult asyncResult)
        {
            var socket = (Socket)asyncResult.AsyncState;
            socket.EndConnect(asyncResult);
            Debug.Log("Connect: " + socket.Connected);
        }

        [CanBeNull]
        private IPEndPoint GetRemoteEndPoint(string hostNameOrAddress, int port)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(hostNameOrAddress);
            var hostEntry = Dns.GetHostEntry(hostNameOrAddress);

            foreach (var ipAddress in hostEntry.AddressList)
                stringBuilder.AppendLine(ipAddress.ToString());
            Debug.Log(stringBuilder.ToString());

            foreach (var ipAddress in hostEntry.AddressList)
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    var remoteEndPoint = new IPEndPoint(ipAddress, port);
                    return remoteEndPoint;
                }

            Debug.Log($"Cannot get host entry for {HostNameOrAddress}:{port}");
            return null;
        }

        public void Connect(string hostNameOrAddress, int port)
        {
            var remoteEndPoint = GetRemoteEndPoint(hostNameOrAddress, port);
            if (remoteEndPoint == null)
                return;

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // 完全卡住, 大约20秒后抛SocketException
                socket.Connect(remoteEndPoint);
                Debug.Log("Connect: " + socket.Connected);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                socket.Close();
            }
        }
    }
}
