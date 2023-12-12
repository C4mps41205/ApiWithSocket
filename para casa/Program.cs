using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using para_casa;
using Newtonsoft.Json;
using System.Reflection;

namespace para_casa
{
    internal class Program
    {

        const string IP_HOST = "192.168.0.22"; // CMD > ipconfig > conexão sem fio ipv4
        const int PORT = 666; 
        static void Main(string[] args)
        {
            Socket? server = Start_server();

            if(server == null)
            {
                Console.WriteLine("Erro ao tentar se conectar ao servidor");
                return;
            }

            Program.Accept_client(server);
        }

        /*
            Função responsável por achar as rotas na classe Routes.cs e retornar o conteúdo
         */
        public static string search_route(string route)
        {
            Routes routes = new Routes();
            string response = string.Empty;

            switch (route)
            {
                case "/resposta1":
                    response = $"HTTP/1.1 200 OK\r\n{routes.resposta1.data_type}\r\n\r\n{routes.resposta1.data}";
                    break;

                case "/resposta2":
                    response = $"HTTP/1.1 200 OK\r\n{routes.resposta2.data_type}\r\n{routes.resposta2.data}";
                    break;

                default:
                    response = "Não foi encontrada nenhuma rota";
                    break;

            }

            return response;
        }

        /*
            Função responsável por receber os dados da url e retornar um utf-8
         */
        public static async Task<string> Receive_dataAsync(Socket client)
        {
            // Entra em um loop até conseguir pegar todos os dados da requisição do cliente
            while (true)
            {
                // limitador de bits
                byte[] buffer = new byte[1024];
                int received = client.Receive(buffer);
                if (received > 0)
                {
                    // Decodificando em utf8
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, received);
                     
                    return receivedData;
                }
            }
        }

        public static string Get_Route(string dataUtf8)
        {
            int startIndex = dataUtf8.IndexOf("GET ");
            int endIndex = dataUtf8.IndexOf(" HTTP/1.1");

            if (startIndex >= 0 && endIndex > startIndex)
            {
                startIndex += 4; // Avança o início para além de "GET "
                string route = dataUtf8.Substring(startIndex, endIndex - startIndex);
                return route.Trim(); // Limpa espaços em branco extras, se houver
            }

            return "Rota não encontrada";
        }



        /*
            Função responsável por lidar com a requisição
         */
        public static async Task Handle_clientAsync(Socket client)
        {
            /*
                Printa no console o IP do cliente após o mesmo fazer a requisição
             */
            IPEndPoint clientEndPoint = (IPEndPoint)client.RemoteEndPoint;
            string clientIP = clientEndPoint.Address.ToString();

            Console.WriteLine($"Cliente {clientIP} encontrado");

            // descriptografa binário em utf-8
            string reciveData = await Program.Receive_dataAsync(client);

            Console.WriteLine($"Cliente {clientIP} passou pela descriptografia {reciveData}");

            string route = Program.Get_Route(reciveData);
            Console.WriteLine($"Cliente {clientIP} passou pela pesquisa de rota e encontrou o resultado: {route}");

            string response = Program.search_route(route);
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            await client.SendAsync(new ArraySegment<byte>(responseBytes), SocketFlags.None);

            // Encerrando a escrita na conexão
            client.Shutdown(SocketShutdown.Send);

            // Fechando o socket do cliente
            client.Close();
        }

        /* 
            Função responsável por validar com o cliente
        */
        public static async Task Accept_client(Socket socketServer)
        {
            while (true)
            {
                /*
                    Aceita o socket do cliente que pingar o IP_HOST no navegador
                */
                Socket clientSocket = socketServer.Accept();

                // Chama a função para lidar com o cliente
                await Program.Handle_clientAsync(clientSocket);
            }
        }

        /* 
            Função responsável por inicializar o servidor TPC através do socket e hospedando
            no endereço local (variável constante ID_HOST)
        */
        public static Socket Start_server()
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(IP_HOST);
                IPAddress ipAdress = ipHost.AddressList[0];

                /* 
                    Criando objeto socket com a protocolo de rede expecificando:
                        - adress family (familia de endereço) [PROTOCOLO DE REDE]
                        - TCP (SOCK_STREAM) [PROTOCOLO DE TRANSPORTE]
                */
                Socket socket = new Socket(ipAdress.AddressFamily,
                                           SocketType.Stream,
                                           ProtocolType.Tcp);

                /* 
                    Hospendando nosso socket em um endereço
                    para que ele possa ser encontrado por outros sockets
                */
                IPEndPoint localEndPoint = new IPEndPoint(ipAdress, PORT);
                socket.Bind(localEndPoint);

                // Disponibilizando acesso para a placa de rede
                socket.Listen(10);

                return socket;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}