using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aggregator.Task.Http;
using Xunit;

namespace Aggregator.Test.Integration.Servers
{
    public class HttpTests
    {
        [Fact]
        public async void ServingUpFragmentHtmlAllowsTheServerToGatherUrlFragments()
        {
            var server = new HttpServer();
            var expectedQuerystringKey = Guid.NewGuid().ToString();
            var expectedQuerystringValue = Guid.NewGuid().ToString();
            var uri = new Uri($"http://localhost:33433/path/#{expectedQuerystringKey}={expectedQuerystringValue}");

            var source = new TaskCompletionSource<NameValueCollection>();

            server.CreateServer(
                (request, response) =>
                {
                    if (request.Query.Count == 0)
                    {
                        response.WriteHtml("Infrastructure/fragment.html");
                    }
                    else
                    {
                        source.SetResult(request.Query);
                    }
                });

            server.Listen(uri);

            Process.Start(uri.ToString());

            var result = await source.Task;

            Assert.Equal(1, result.Count);
            Assert.Equal(expectedQuerystringKey, result.AllKeys[0]);
            Assert.Equal(expectedQuerystringValue, result[expectedQuerystringKey]);

            server.Close();
            Thread.Sleep(1000);
        }

        [Fact]
        public void StartingStoppingAndThenRestartingServerDoesntThrowAnException()
        {
            var server = new HttpServer();

            var uria = new Uri("http://localhost:33433");
            var urib = new Uri("http://localhost:33833");

            var task1 = server.Listen(uria);
            Thread.Sleep(1000);
            server.Close();
            Thread.Sleep(1000);

            var task2 = server.Listen(urib);
            Thread.Sleep(1000);
            server.Close();
            Thread.Sleep(1000);

            var task3 = server.Listen(uria);
            Thread.Sleep(1000);
            server.Close();
            Thread.Sleep(1000);

            Assert.Null(task1.Exception);
            Assert.Null(task2.Exception);
            Assert.Null(task3.Exception);

        }

        [Fact]
        public void StartingThenHandlingARequestThenClosingDoesntThrowAnException()
        {
            var server = new HttpServer();
            var uri = new Uri("http://localhost:33433");

            server.CreateServer(
                (request, response) => 
                {
                    Thread.Sleep(3000);
                    response.WriteHead((int)HttpStatusCode.OK);
                    response.WriteHead(new HeaderPair("Content-Type", "text/plain"));
                    response.Write("Some texty text");
                    response.End();
                });
#pragma warning disable 4014
            server.Listen(uri);
#pragma warning restore 4014

            Uri serviceUri = new Uri("http://localhost:33433/");
            WebClient downloader = new WebClient();
            downloader.OpenReadCompleted += (sender, args) => { };
            downloader.OpenReadAsync(serviceUri);
            Thread.Sleep(2000);

            server.Close();
        }

        [Fact]
        public void ServerDoesntBlockOnMainThreadWhenHandlingARequest()
        {
            var server = new HttpServer();
            var uri = new Uri("http://localhost:33433");

            server.CreateServer(
                (request, response) => System.Threading.Tasks.Task.Run(() =>
                {
                    Thread.Sleep(3000);
                    response.WriteHead((int)HttpStatusCode.OK);
                    response.WriteHead(new HeaderPair("Content-Type", "text/plain"));
                    response.Write("Some texty text");
                    response.End();
                }));
#pragma warning disable 4014
            server.Listen(uri);
#pragma warning restore 4014

            Uri serviceUri = new Uri("http://localhost:33433/");
            WebClient downloader = new WebClient();
            downloader.OpenReadCompleted += (sender, args) => { };

            var watch = Stopwatch.StartNew();
            downloader.OpenReadAsync(serviceUri);
            watch.Stop();

            Assert.True(watch.ElapsedMilliseconds < 3000);

            server.Close();
        }

        [Fact]
        public void ServerDoesntBlockOnSubsequentThreadingCalls()
        {
            var server = new HttpServer();
            bool isLongPauseOver = false;
            bool isShortPauseOver = false;
            var uri = new Uri("http://localhost:33433");

            server.CreateServer(
                (request, response) => System.Threading.Tasks.Task.Run(() =>
                {
                    if (request.Url == "/longpause")
                    {
                        Thread.Sleep(3000);
                        isLongPauseOver = true;
                    }
                    else
                    {
                        Thread.Sleep(1);
                        isShortPauseOver = true;
                    }
                    response.WriteHead((int) HttpStatusCode.OK);
                    response.WriteHead(new HeaderPair("Content-Type", "text/plain"));
                    response.Write("Some texty text");
                    response.End();
                }));
#pragma warning disable 4014
            server.Listen(uri);
#pragma warning restore 4014

            Uri longPauseUri = new Uri("http://localhost:33433/longpause");
            Uri shortPauseUri = new Uri("http://localhost:33433/");
            WebClient longpauseDownloader = new WebClient();
            longpauseDownloader.OpenReadCompleted += (sender, args) => { };

            WebClient otherDownloader = new WebClient();
            otherDownloader.OpenReadCompleted += (sender, args) => { };

            longpauseDownloader.OpenReadAsync(longPauseUri);
            otherDownloader.OpenReadAsync(shortPauseUri);

            Thread.Sleep(1000);

            Assert.True(isShortPauseOver);
            Assert.False(isLongPauseOver);

            server.Close();
        }
    }
}
