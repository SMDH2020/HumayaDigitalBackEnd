using Quobject.SocketIoClientDotNet.Client;
using System;

namespace HD.Generales
{
    public class WebSocketConfig
    {
        private readonly string serverURL;
        private Socket socketnotificaciones;
        private string jwtToken;

        public WebSocketConfig(string serverUrl, string token)
        {
            this.serverURL = serverUrl;
            this.jwtToken = token;
        }

        public void Connect()
        {
            socketnotificaciones = IO.Socket(serverURL);

            socketnotificaciones.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Conectado al servidor socket");

                // Emitir el token JWT una vez conectado
                socketnotificaciones.Emit("authenticate", jwtToken);
            });

            socketnotificaciones.On(Socket.EVENT_DISCONNECT, () =>
            {
                Console.WriteLine("Desconectado del servidor socket");
            });

            socketnotificaciones.On("notificacion", (data) =>
            {
                Console.WriteLine($"Notificacion recibida: {data}");
            });
        }

        public void Disconnect()
        {
            if (socketnotificaciones != null)
            {
                socketnotificaciones.Disconnect();
            }
        }

        public void EnviarNotificacion(string message)
        {
            if (socketnotificaciones != null)
            {
                socketnotificaciones.Emit("notificacion", message);
            }
        }
    }
}
