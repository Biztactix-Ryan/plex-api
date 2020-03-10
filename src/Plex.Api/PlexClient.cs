﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Plex.Api.Helpers;
using Plex.Api.Models;
using Plex.Api.Models.Friends;
using Plex.Api.Models.MetaData;
using Plex.Api.Models.OAuth;
using Plex.Api.Models.Server;
using Plex.Api.Models.Status;

namespace Plex.Api
{
    public class PlexClient : IPlexClient
    {
        private IApi Api { get; }
        private readonly ClientOptions _clientOptions;
        
        private const string SignInUri = "https://plex.tv/users/sign_in.json";
        private const string FriendsUri = "https://plex.tv/pms/friends/all";
        private const string GetAccountUri = "https://plex.tv/users/account.json";
        private const string ServerUri = "https://plex.tv/pms/servers.xml";
        
        public PlexClient(IApi api, ClientOptions clientOptions)
        {
            Api = api;
            _clientOptions = clientOptions;
        }
     
        /// <summary>
        /// Sign into the Plex API
        /// This is for authenticating users credentials with Plex
        /// <para>NOTE: Plex "Managed" users do not work</para>
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<PlexAuthentication> SignIn(UserRequest user)
        {
            var userModel = new PlexUserRequest
            {
                User = user
            };
            var request = new Request(SignInUri, string.Empty, HttpMethod.Post);

            AddHeaders(request);
            request.AddJsonBody(userModel);

            var obj = await Api.Request<PlexAuthentication>(request);

            return obj;
        }

        public async Task<PlexStatus> GetStatus(string authToken, string uri)
        {
            var request = new Request(uri, string.Empty, HttpMethod.Get);
            AddHeaders(request, authToken);
            return await Api.Request<PlexStatus>(request);
        }

        public async Task<PlexAccount> GetAccount(string authToken)
        {
            var request = new Request(GetAccountUri, string.Empty, HttpMethod.Get);
            AddHeaders(request, authToken);
            return await Api.Request<PlexAccount>(request);
        }

        /// <summary>
        /// http://[PMS_IP_Address]:32400/status/sessions?X-Plex-Token=YourTokenGoesHere
        /// Retrieves a list of active sessions on the Plex Media Server instance
        /// </summary>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="plexFullHost">Full Uri of Plex Media Server Instance</param>
        /// <returns></returns>
        public async Task<SessionWrapper> GetSessions(string authToken, string plexFullHost)
        {
            var request = new Request("status/sessions", plexFullHost, HttpMethod.Get);

            AddHeaders(request, authToken);
            
            return await Api.Request<SessionWrapper>(request);
        }

        /// <summary>
        /// http://[PMS_IP_Address]:32400/library/sections?X-Plex-Token=YourTokenGoesHere
        /// Retrieves a list of servers tied to your Plex Account
        /// </summary>
        /// <param name="authToken">Authentication Token</param>
        /// <returns></returns>
        public async Task<Models.Server.PlexServers> GetServers(string authToken)
        {
            var request = new Request(ServerUri, string.Empty, HttpMethod.Get, ContentType.Xml);

            AddHeaders(request, authToken);
            
            return await Api.Request<Models.Server.PlexServers>(request);
        }

        /// <summary>
        /// http://[PMS_IP_Address]:32400/library/sections?X-Plex-Token=YourTokenGoesHere
        /// </summary>
        /// <param name="authToken">Authentication Token</param>
        /// <param name="plexFullHost">Full Uri of Plex Media Server Instance</param>
        /// <returns></returns>
        public async Task<LibrariesWrapper> GetLibrarySections(string authToken, string plexFullHost)
        {
            var request = new Request("library/sections", plexFullHost, HttpMethod.Get);
            AddHeaders(request, authToken);
            return await Api.Request<LibrariesWrapper>(request);
        }

        /// <summary>
        /// http://[PMS_IP_Address]:32400/library/sections/29/all?X-Plex-Token=YourTokenGoesHere
        /// </summary>
        /// <param name="authToken">Plex Auth Token</param>
        /// <param name="plexFullHost">Full Host Uri</param>
        /// <param name="libraryId">Id key for the Plex Library Section</param>
        /// <returns></returns>
        public async Task<LibraryWrapper> GetLibrary(string authToken, string plexFullHost, string libraryId)
        {
            var request = new Request($"library/sections/{libraryId}/all", plexFullHost, HttpMethod.Get);
            AddHeaders(request, authToken);
            return await Api.Request<LibraryWrapper>(request);
        }

        // public async Task<PlexLibrariesForMachineId> GetLibrariesForMachineId(string authToken, string machineId)
        // {
        //     var request = new Request("", $"https://plex.tv/api/servers/{machineId}", HttpMethod.Get, ContentType.Xml);
        //     AddHeaders(request, authToken);
        //     return await Api.Request<PlexLibrariesForMachineId>(request);
        // }

        /// <summary>
        // 192.168.1.69:32400/library/metadata/3662/allLeaves
        // The metadata ratingkey should be in the Cache
        // Search for it and then call the above with the Directory.RatingKey
        // THEN! We need the episode metadata using result.Vide.Key ("/library/metadata/3664")
        // We then have the GUID which contains the TVDB ID plus the season and episode number: guid="com.plexapp.agents.thetvdb://269586/2/8?lang=en"
        /// </summary>
        /// <param name="authToken"></param>
        /// <param name="plexFullHost"></param>
        /// <param name="ratingKey"></param>
        /// <returns></returns>
        public async Task<PlexMetadata> GetEpisodeMetaData(string authToken, string plexFullHost, int ratingKey)
        {
            var request = new Request($"/library/metadata/{ratingKey}", plexFullHost, HttpMethod.Get);
            AddHeaders(request, authToken);
            return await Api.Request<PlexMetadata>(request);
        }

        public async Task<MetadataWrapper> GetMetadata(string authToken, string plexFullHost, int itemId)
        {
            var request = new Request($"library/metadata/{itemId}", plexFullHost, HttpMethod.Get);
            AddHeaders(request, authToken);
            return await Api.Request<MetadataWrapper>(request);
        }

        public async Task<PlexMetadata> GetSeasons(string authToken, string plexFullHost, int ratingKey)
        {
            var request = new Request($"library/metadata/{ratingKey}/children", plexFullHost, HttpMethod.Get);
            AddHeaders(request, authToken);
            return await Api.Request<PlexMetadata>(request);
        }

        /// <summary>
        /// Gets all episodes.
        /// </summary>
        /// <param name="authToken">The authentication token.</param>
        /// <param name="host">The host.</param>
        /// <param name="section">The section.</param>
        /// <param name="start">The start count.</param>
        /// <param name="retCount">The return count, how many items you want returned.</param>
        /// <returns></returns>
        public async Task<PlexContainer> GetAllEpisodes(string authToken, string host, string section, int start, int retCount)
        {
            var request = new Request($"/library/sections/{section}/all", host, HttpMethod.Get);

            request.AddQueryString("type", "4");
            AddLimitHeaders(request, start, retCount);
            AddHeaders(request, authToken);

            return await Api.Request<PlexContainer>(request);
        }

        /// <summary>
        /// Retuns all the Plex users for this account
        /// NOTE: For HOME USERS. There is no username or email, the user's home name is under the title property
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public async Task<PlexFriends> GetUsers(string authToken)
        {
            var request = new Request(string.Empty, FriendsUri, HttpMethod.Get, ContentType.Xml);
            AddHeaders(request, authToken);
          
            return await Api.Request<PlexFriends>(request);
        }

        public async Task<PlexMetadata> GetRecentlyAdded(string authToken, string uri, string sectionId)
        {
            var request = new Request($"library/sections/{sectionId}/recentlyAdded", uri, HttpMethod.Get);
            AddHeaders(request, authToken);
            AddLimitHeaders(request, 0, 50);

            return await Api.Request<PlexMetadata>(request);
        }

        public async Task<OAuthPin> GetPin(int pinId)
        {
            var request = new Request($"api/v2/pins/{pinId}", "https://plex.tv/", HttpMethod.Get);
            AddHeaders(request);

            return await Api.Request<OAuthPin>(request);
        }

        public async Task<Uri> GetOAuthUrl(string code, string applicationUrl)
        {
            var request = new Request("auth#", "https://app.plex.tv", HttpMethod.Get);
            AddHeaders(request);
            
            request.AddQueryString("code", code);
            request.AddQueryString("context[device][product]", _clientOptions.ApplicationName);
            request.AddQueryString("context[device][environment]", "bundled");
            request.AddQueryString("context[device][layout]", "desktop");
            request.AddQueryString("context[device][platform]", "Web");
            request.AddQueryString("context[device][device]", _clientOptions.DeviceName);

            request.AddQueryString("clientID", _clientOptions.ClientId.ToString("N"));

            if (request.FullUri.Fragment.Equals("#"))
            {
                var uri = request.FullUri.ToString();
                var withoutEnd = uri.Remove(uri.Length - 1, 1);
                var startOfQueryLocation = withoutEnd.IndexOf('?');
                var better = withoutEnd.Insert(startOfQueryLocation, "#");
                request.FullUri = new Uri(better);
            }

            return request.FullUri;
        }

        public async Task<PlexAddWrapper> AddUser(string emailAddress, string serverId, string authToken, int[] libs)
        {
            var request = new Request(string.Empty, $"https://plex.tv/api/servers/{serverId}/shared_servers", HttpMethod.Post, ContentType.Xml);
            AddHeaders(request, authToken);
            request.AddJsonBody(new
            {
                server_id = serverId,
                shared_server = new
                {
                    library_section_ids = libs.Length > 0 ? libs : new int[]{},
                    invited_email = emailAddress
                },
                sharing_settings = new { }
            });
            var result = await Api.RequestContent(request);
            try
            {
                var add = Api.DeserializeXml<PlexAdd>(result);
                return new PlexAddWrapper{Add = add};
            }
            catch (InvalidOperationException)
            {
                var error = Api.DeserializeXml<AddUserError>(result);
                return new PlexAddWrapper{Error = error};
            }
        }


        /// <summary>
        /// Adds the required headers and also the authorization header
        /// </summary>
        /// <param name="request"></param>
        /// <param name="authToken"></param>
        private void AddHeaders(Request request, string authToken)
        {
            request.AddHeader("X-Plex-Token", authToken);
            AddHeaders(request);
        }

        /// <summary>
        /// Adds the main required headers to the Plex Request
        /// </summary>
        /// <param name="request"></param>
        private void AddHeaders(Request request)
        {
            request.AddHeader("X-Plex-Client-Identifier", _clientOptions.ClientId.ToString("N"));
            request.AddHeader("X-Plex-Product", _clientOptions.ApplicationName);
            request.AddHeader("X-Plex-Version", "3");
            request.AddHeader("X-Plex-Device", _clientOptions.DeviceName);
            request.AddHeader("X-Plex-Platform", "Web");
            request.AddContentHeader("Content-Type", request.ContentType == ContentType.Json ? "application/json" : "application/xml");
            request.AddHeader("Accept", "application/json");
        }

        private void AddLimitHeaders(Request request, int from, int to)
        {
            request.AddHeader("X-Plex-Container-Start", from.ToString());
            request.AddHeader("X-Plex-Container-Size", to.ToString());
        }
    }
}