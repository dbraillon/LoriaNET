//using SpotifyAPI.Web;
//using SpotifyAPI.Web.Auth;
//using SpotifyAPI.Web.Enums;
//using SpotifyAPI.Web.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;

//namespace LoriaNET
//{
//    public class SpotifyModule : IModule, IAction
//    {
//        public string Name => "Spotify module";
//        public string Command => "spotify";
//        public string Description => "Controls the spotify player";
//        public string Usage => 
//            "spotify primary [secondary] [value]" + Environment.NewLine +
//            $"{string.Join(Environment.NewLine, SimplifiedSpotifyWebAPI.SupportedCommands.Select(c => $" {c.Key}\t{string.Join("|", c.Value)}"))}";

//        public Configuration Configuration { get; set; }
//        public SimplifiedSpotifyWebAPI SpotifyWebAPI { get; set; }

//        public SpotifyModule(Configuration configuration)
//        {
//            Configuration = configuration;
//        }

//        public void Perform(string[] args)
//        {
//            var response = SpotifyWebAPI.Dispatch(args);
//            if (!string.IsNullOrEmpty(response))
//            {
//                Configuration.Callbacks.CallbackAll(response);
//            }
//        }

//        public bool IsEnabled() => true;

//        public void Configure()
//        {
//            SpotifyWebAPI = new SimplifiedSpotifyWebAPI(Configuration.Configs["spotify::APIKey"]);
//        }

//        public void Activate()
//        {
//        }

//        public void Deactivate()
//        {
//        }
//    }

//    public class SimplifiedSpotifyWebAPI
//    {
//        public static Dictionary<string, string[]> SupportedCommands = new Dictionary<string, string[]>
//        {
//            { "get", new string[] { "album", "artist", "device", "devices", "playlist", "track" } },
//            { "set", new string[] { "device", "volume" } },
//            { "search", new string[] { "any", "album", "artist", "playlist", "track" } },
//            { "play", new string[] { "current", "album", "artist", "playlist", "track", "next", "previous" } },
//            { "pause", new string[] { "now", "next" } }
//        };

//        public SpotifyWebAPI SpotifyWebAPI { get; set; }
//        public string UserId { get; set; }

//        public SimplifiedSpotifyWebAPI(string apiKey)
//        {
//            var webAPIFactory = new WebAPIFactory(
//                "http://localhost",
//                8000,
//                apiKey,
//                Scope.UserReadPrivate | Scope.Streaming | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState,
//                TimeSpan.FromSeconds(20)
//            );

//            SpotifyWebAPI = webAPIFactory.GetWebApi().GetAwaiter().GetResult();
//            UserId = SpotifyWebAPI.GetPrivateProfile().Id;
//        }

//        public string Dispatch(string command) => Dispatch(command.Split(' '));
//        public string Dispatch(IEnumerable<string> args)
//        {
//            var primary = args.FirstOrDefault();

//            switch (primary)
//            {
//                case "get": return Get(args.Skip(1));
//                case "set": return Set(args.Skip(1)) ? string.Empty : "error";
//                case "search": return Search(args.Skip(1));
//                case "play": return Play(args.Skip(1)) ? string.Empty : "error";
//                case "pause": return Pause(args.Skip(1)) ? string.Empty : "error";
//                default: return "error";
//            }
//        }
         
//        public string Get(IEnumerable<string> args)
//        {
//            var secondary = args.FirstOrDefault();

//            switch (secondary)
//            {
//                case "device": return GetDevice().Device.Name;
//                case "devices": return string.Join(", ", GetDevices().Devices.Select(d => d.Name));
//                default: return string.Empty;
//            }
//        }
//        public PlaybackContext GetDevice() => HandleError(SpotifyWebAPI.GetPlayback());
//        public AvailabeDevices GetDevices() => HandleError(SpotifyWebAPI.GetDevices());
         
//        public bool Set(IEnumerable<string> args)
//        {
//            var secondary = args.FirstOrDefault();

//            switch (secondary)
//            {
//                case "device": return SetDevice(args.Skip(1));
//                case "volume": return SetVolume(args.Skip(1));
//                default: return false;
//            }
//        }
//        public bool SetDevice(IEnumerable<string> args)
//        {
//            var devices = GetDevices();
//            var deviceName = string.Join(" ", args).Trim();

//            var device = devices.Devices.FirstOrDefault(d => string.Equals(d.Name, deviceName, StringComparison.InvariantCultureIgnoreCase));
//            if (device != null)
//            {
//                return HandleError(SpotifyWebAPI.TransferPlayback(device.Id)).StatusCode() == HttpStatusCode.NoContent;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public bool SetVolume(IEnumerable<string> args) => HandleError(SpotifyWebAPI.SetVolume(int.Parse(args.FirstOrDefault()))).StatusCode() == HttpStatusCode.NoContent;
         
//        public string Search(IEnumerable<string> args)
//        {
//            var secondary = args.FirstOrDefault();

//            switch (secondary)
//            {
//                //case "any": return SearchAny(args.Skip(1));
//                case "album": return string.Join(Environment.NewLine, SearchAlbum(args.Skip(1)).Items.Select(a => a.Name));
//                case "artist": return string.Join(Environment.NewLine, SearchArtist(args.Skip(1)).Items.Select(a => a.Name));
//                case "playlist": return string.Join(Environment.NewLine, SearchPlaylist(args.Skip(1)).Items.Select(p => p.Name));
//                case "track": return string.Join(Environment.NewLine, SearchTrack(args.Skip(1)).Items.Select(t => t.Name));
//                default: return string.Empty;
//            }
//        }
//        //public string SearchAny(IEnumerable<string> args) => SpotifyWebAPI.SearchItems(args.FirstOrDefault(), SearchType.All).;
//        public Paging<SimpleAlbum> SearchAlbum(IEnumerable<string> args) => HandleError(SpotifyWebAPI.SearchItems(string.Join(" ", args), SearchType.Album)).Albums;
//        public Paging<FullArtist> SearchArtist(IEnumerable<string> args) => HandleError(SpotifyWebAPI.SearchItems(string.Join(" ", args), SearchType.Artist)).Artists;
//        public Paging<SimplePlaylist> SearchPlaylist(IEnumerable<string> args) => HandleError(SpotifyWebAPI.SearchItems(string.Join(" ", args), SearchType.Playlist)).Playlists;
//        public Paging<FullTrack> SearchTrack(IEnumerable<string> args) => HandleError(SpotifyWebAPI.SearchItems(string.Join(" ", args), SearchType.Track)).Tracks;

//        public bool Play(IEnumerable<string> args)
//        {
//            var secondary = args.FirstOrDefault();

//            switch (secondary)
//            {
//                case "current": return PlayCurrent();
//                case "album": return PlayAlbum(args.Skip(1));
//                case "artist": return PlayArtist(args.Skip(1));
//                case "playlist": return PlayPlaylist(args.Skip(1));
//                case "track": return PlayTrack(args.Skip(1));
//                case "next": return PlayNext(args.Skip(1));
//                case "previous": return PlayPrevious(args.Skip(1));
//                case "":
//                case null: return PlayCurrent();
//                default: return false;
//            }
//        }
//        public bool PlayCurrent() => HandleError(SpotifyWebAPI.ResumePlayback()).StatusCode() == HttpStatusCode.NoContent;
//        public bool PlayAlbum(IEnumerable<string> args)
//        {
//            var albums = SearchAlbum(args);
//            var albumName = string.Join(" ", args).Trim();

//            var album = albums.Items.FirstOrDefault(a => string.Equals(a.Name, albumName, StringComparison.InvariantCultureIgnoreCase));
//            if (album != null)
//            {
//                return HandleError(SpotifyWebAPI.ResumePlayback(contextUri: album.Uri)).StatusCode() == HttpStatusCode.NoContent;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public bool PlayArtist(IEnumerable<string> args)
//        {
//            var artists = SearchArtist(args);
//            var artistName = string.Join(" ", args).Trim();

//            var artist = artists.Items.FirstOrDefault(a => string.Equals(a.Name, artistName, StringComparison.InvariantCultureIgnoreCase));
//            if (artist != null)
//            {
//                return HandleError(SpotifyWebAPI.ResumePlayback(contextUri: artist.Uri)).StatusCode() == HttpStatusCode.NoContent;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public bool PlayPlaylist(IEnumerable<string> args)
//        {
//            var playlists = SearchPlaylist(args);
//            var playlistName = string.Join(" ", args).Trim();

//            var playlist = playlists.Items.FirstOrDefault(a => string.Equals(a.Name, playlistName, StringComparison.InvariantCultureIgnoreCase));
//            if (playlist != null)
//            {
//                return HandleError(SpotifyWebAPI.ResumePlayback(contextUri: playlist.Uri)).StatusCode() == HttpStatusCode.NoContent;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public bool PlayTrack(IEnumerable<string> args)
//        {
//            var tracks = SearchTrack(args);
//            var trackName = string.Join(" ", args).Trim();

//            var track = tracks.Items.FirstOrDefault(a => string.Equals(a.Name, trackName, StringComparison.InvariantCultureIgnoreCase));
//            if (track != null)
//            {
//                return HandleError(SpotifyWebAPI.ResumePlayback(contextUri: track.Uri)).StatusCode() == HttpStatusCode.NoContent;
//            }
//            else
//            {
//                return false;
//            }
//        }
//        public bool PlayPrevious(IEnumerable<string> args) => HandleError(SpotifyWebAPI.SkipPlaybackToPrevious()).StatusCode() == HttpStatusCode.NoContent;
//        public bool PlayNext(IEnumerable<string> args) => HandleError(SpotifyWebAPI.SkipPlaybackToNext()).StatusCode() == HttpStatusCode.NoContent;
        
//        public bool Pause(IEnumerable<string> args)
//        {
//            var secondary = args.FirstOrDefault();

//            switch (secondary)
//            {
//                case "now": return PauseNow();
//                case "":
//                case null: return PauseNow();
//                default: return false;
//            }
//        }
//        public bool PauseNow() => HandleError(SpotifyWebAPI.PausePlayback()).StatusCode() == HttpStatusCode.NoContent;

//        public void HandleError(Error error)
//        {
//        }

//        public TBasicModel HandleError<TBasicModel>(TBasicModel basicModel)
//            where TBasicModel : BasicModel
//        {
//            HandleError(basicModel.Error);
//            return basicModel;
//        }
//    }
//}
