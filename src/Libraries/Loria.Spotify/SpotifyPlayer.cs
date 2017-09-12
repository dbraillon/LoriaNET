using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loria.Spotify
{
    public class SpotifyPlayer
    {
        private Scope Scope =>
            Scope.PlaylistModifyPrivate | Scope.PlaylistModifyPublic |
            Scope.PlaylistReadCollaborative | Scope.PlaylistReadPrivate |
            Scope.Streaming |
            Scope.UserFollowModify | Scope.UserFollowRead |
            Scope.UserLibraryModify | Scope.UserLibraryRead |
            Scope.UserModifyPlaybackState | Scope.UserReadPlaybackState |
            Scope.UserReadBirthdate | Scope.UserReadEmail |
            Scope.UserReadPrivate | Scope.UserReadRecentlyPlayed |
            Scope.UserTopRead;
        private SpotifyWebAPI SpotifyWebAPI { get; set; }
        private string UserId { get; set; }

        public string CallbackUrl { get; set; }
        public int CallbackPort { get; set; }
        public string ApiKey { get; set; }
        
        public SpotifyPlayer(string apiKey, string callbackUrl)
            : this(apiKey, callbackUrl, 80)
        {
        }
        public SpotifyPlayer(string apiKey, string callbackUrl, int callbackPort)
        {
            ApiKey = apiKey;
            CallbackUrl = callbackUrl;
            CallbackPort = callbackPort;
        }
        
        public async Task InitializeAsync()
        {
            var webAPIFactory = new WebAPIFactory(CallbackUrl, CallbackPort, ApiKey, Scope);

            SpotifyWebAPI = await webAPIFactory.GetWebApi();
            UserId = SpotifyWebAPI.GetPrivateProfile().Id;
        }

        public PlaybackContext GetPlayback() => SpotifyWebAPI.GetPlayback();
        public AvailabeDevices GetDevices() => SpotifyWebAPI.GetDevices();
        
        public void SetPlayback(string deviceId)
        {
            SetPlayback(deviceId, false);
        }
        public void SetPlayback(string deviceId, bool play)
        {
            SpotifyWebAPI.TransferPlayback(deviceId, play: play);
        }
        public void SetPlayback()
        {
            var computerName = Environment.MachineName;
            var devices = GetDevices();
            var device = devices.Devices.FirstOrDefault(d => string.Equals(d.Name, computerName, StringComparison.InvariantCultureIgnoreCase));
            if (device != null)
            {
                SetPlayback(device.Id);
            }
        }
        public void SetVolume(int volumePercent)
        {
            SetVolume(volumePercent, string.Empty);
        }
        public void SetVolume(int volumePercent, string deviceId)
        {
            SpotifyWebAPI.SetVolume(volumePercent, deviceId: deviceId ?? string.Empty);
        }
        
        public SearchItem Search(string query) => SpotifyWebAPI.SearchItems(query, SearchType.All);
        public IEnumerable<FullAlbum> SearchAlbum(string query) => ToFullAlbum(SpotifyWebAPI.SearchItems(query, SearchType.Album, limit: 3).Albums);
        public Paging<FullArtist> SearchArtist(string query) => SpotifyWebAPI.SearchItems(query, SearchType.Artist, limit: 3).Artists;
        public Paging<SimplePlaylist> SearchPlaylist(string query) => SpotifyWebAPI.SearchItems(query, SearchType.Playlist, limit: 3).Playlists;
        public Paging<FullTrack> SearchTrack(string query) => SpotifyWebAPI.SearchItems(query, SearchType.Track, limit: 3).Tracks;

        public ErrorResponse Play() => SpotifyWebAPI.ResumePlayback();
        public ErrorResponse Pause() => SpotifyWebAPI.PausePlayback();
        public ErrorResponse Previous() => SpotifyWebAPI.SkipPlaybackToPrevious();
        public ErrorResponse Next() => SpotifyWebAPI.SkipPlaybackToNext();

        public void Play(string uri) => SpotifyWebAPI.ResumePlayback(contextUri: uri);
        public void PlayTrack(string uri) => SpotifyWebAPI.ResumePlayback(uris: new List<string> { uri });

        public IEnumerable<FullAlbum> ToFullAlbum(Paging<SimpleAlbum> albums)
        {
            foreach (var album in albums.Items)
            {
                yield return SpotifyWebAPI.GetAlbum(album.Id);
            }
        }
    }
}
