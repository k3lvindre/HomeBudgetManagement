using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using IoFile = System.IO.File;

namespace HomeBudgetManagement.Api.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        static List<WebSocket> _webSocketConnections = new List<WebSocket>();

        //Web Sockets Example
        [Route("/ws")]
        [HttpPost]
        public async Task WebSocketAction()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _webSocketConnections.Add(webSocket);

                while (webSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    var receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;

                    var message2 = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    message2 = $"Server received:{message2}";
                    var bytes2 = Encoding.UTF8.GetBytes(message2);

                    var arraySegment2 = new ArraySegment<byte>(bytes2, 0, bytes2.Length);
                    foreach (var item in _webSocketConnections)
                    {
                        if (item.State == WebSocketState.Open)
                            await item.SendAsync(arraySegment2,
                            WebSocketMessageType.Text,
                            true,
                              CancellationToken.None);
                    }


                    //If you set the `Offset` property of an `ArraySegment` to 10, it means that the segment will start from the element at index 10 of the original array.
                    //Let's illustrate this with an example in C#:
                    //int[] originalArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
                    //
                    //Create an ArraySegment starting from index 10 with a count of 5 elements
                    //ArraySegment<int> segment = new ArraySegment<int>(originalArray, 10, 5);
                    //
                    //// Access elements of the segment
                    //foreach (int element in segment)
                    //{
                    //    Console.WriteLine(element);
                    //}
                    //
                    //In this example, the `ArraySegment` represents a segment starting from index 10 of the `originalArray` with a count of 5 elements.Therefore, the output of the `foreach` loop will be:
                    //
                    //10
                    //11
                    //12
                    //13
                    //14
                    //
                    //Setting the `Offset` to 10 effectively means that the segment starts 10 elements into the original array.
                    //This allows you to work with a subsection of the array without having to modify or copy the original data.
                    //It's useful when you only need to operate on a portion of the array for certain operations.

                    //So here we want to start at index of 0 because we need the whole file
                    var bytes3 = IoFile.ReadAllBytes(@"C:\Users\KRicafort\Documents\sample.jpg");
                    var arraySegment3 = new ArraySegment<byte>(bytes3, 0, bytes3.Length);
                    foreach (var item in _webSocketConnections)
                    {
                        if (item.State == WebSocketState.Open)
                            await item.SendAsync(arraySegment3,
                                WebSocketMessageType.Binary,
                                true,
                                CancellationToken.None);
                    }
                }

                _webSocketConnections.Remove(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
