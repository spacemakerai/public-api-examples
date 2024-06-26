﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace FormaHelloTerrainClient
{
    public class OAuthHandler
    {
        private const string CLIENT_ID = "CLIENT_TOKEN_HERE";
        private const string REDIRECT_URI = "http://localhost:8080/oauth/callback/";
        private static readonly string[] EDITOR_SCOPES = { "data:read", "data:write" };
        private static readonly string[] VIEWER_SCOPES = { "data:read" };

        private static (string, string) GenerateCodeVerifier()
        {
            var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            var verifier = Base64UrlEncode(randomBytes);

            var buffer = Encoding.UTF8.GetBytes(verifier);
            var hash = SHA256.Create().ComputeHash(buffer);
            var challenge = Base64UrlEncode(hash);

            return (verifier, challenge);

        }

        private static string Base64UrlEncode(byte[] data)
        {
            return Convert.ToBase64String(data)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
        }


        public static string GetAuthorizationUrl(string challenge)
        {
            var queryParams = new Dictionary<string, string>
        {
            { "client_id", CLIENT_ID },
            { "response_type", "code" },
            { "redirect_uri", REDIRECT_URI },
            { "scope", string.Join(" ", EDITOR_SCOPES) },
            { "nonce", "123123123" },
            { "state", "123123123" },
            { "prompt", "login" },
            { "code_challenge", challenge },
            { "code_challenge_method", "S256" }
        };

            string queryString = string.Join("&", queryParams
                .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            return $"https://developer.api.autodesk.com/authentication/v2/authorize?{queryString}";
        }

        public static async Task<string> GetAccessToken()
        {
            var (verifier, challenge) = GenerateCodeVerifier();
            string url = GetAuthorizationUrl(challenge);
            HttpListener httplistener = new HttpListener();
            httplistener.Prefixes.Add(REDIRECT_URI);

            httplistener.Start(); // Ensure the listener is started to accept connections.

            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });


            var contextTask = httplistener.GetContextAsync();
            CancellationToken cancellationToken;
            var cancelationTask = Task.Delay(-1, cancellationToken);

            await Task.WhenAny(contextTask, cancelationTask).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            var context = await contextTask;

            var response = context.Response;

            // Extract the code
            var code = context.Request.QueryString.Get("code");

            var buffer = Encoding.UTF8.GetBytes("success!");
            response.ContentLength64 = buffer.Length;
            response.OutputStream.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>
            {
                response.OutputStream.Close();
                httplistener.Stop();
            });

            using (var client = new HttpClient())
            {
                // Set the base address for HTTP requests
                client.BaseAddress = new Uri("https://developer.api.autodesk.com");

                // Add accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Data to be sent in the request body
                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", CLIENT_ID),
                    new KeyValuePair<string, string>("code_verifier", verifier),
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("redirect_uri", REDIRECT_URI)
                };

                // Encode the data into application/x-www-form-urlencoded format
                HttpContent content = new FormUrlEncodedContent(postData);

                // Send a POST request
                HttpResponseMessage tokenResponse = await client.PostAsync("/authentication/v2/token", content);

                // Ensure we received a successful response.
                tokenResponse.EnsureSuccessStatusCode();

                // Read and return the response body as a string.
                string responseBody = await tokenResponse.Content.ReadAsStringAsync();
                return responseBody;
            }
        }
    }



}
