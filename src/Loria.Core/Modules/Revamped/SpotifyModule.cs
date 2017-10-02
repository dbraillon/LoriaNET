using Loria.Spotify;
using System.Linq;

namespace LoriaNET
{
    public sealed class SpotifyModule : Module, IAction
    {
        public const string PlayIntent = "play";
        public const string SetIntent = "set";
        public const string ResumeIntent = "resume";
        public const string PauseIntent = "pause";
        public const string NextIntent = "next";
        public const string PreviousIntent = "previous";

        public const string AlbumEntity = "album";
        public const string ArtistEntity = "artist";
        public const string PlaylistEntity = "playlist";
        public const string TrackEntity = "track";
        public const string VolumeEntity = "volume";

        public override string Name => "Spotify module";

        public string Description => "Controls spotify with Loria!";

        public string Command => "spotify";

        public string[] SupportedIntents => new string[]
        {
            PlayIntent, SetIntent, ResumeIntent, PauseIntent, NextIntent, PreviousIntent
        };

        public string[] SupportedEntities => new string[]
        {
            AlbumEntity, ArtistEntity, PlaylistEntity, TrackEntity, VolumeEntity
        };

        public string[] Samples => new string[]
        {
            "spotify play -track Heathens",
            "spotify play -album Dangerous Michael Jackson",
            "spotify play -artist Linkin Park",
            "spotify play -playlist Sleep",
            "spotify pause",
            "spotify resume",
            "spotify next",
            "spotify previous",
            "spotify set -volume 50"
        };

        public SpotifyPlayer SpotifyPlayer { get; }

        public SpotifyModule(Loria loria) 
            : base(loria)
        {
            SpotifyPlayer = new SpotifyPlayer(loria.Data.ConfigurationFile.Get("spotify::APIKey"), "http://localhost", 8080);
        }

        public override void Configure()
        {
            SpotifyPlayer.InitializeAsync().GetAwaiter().GetResult();
            Activate();
        }

        public void Perform(Command command)
        {
            SpotifyPlayer.SetPlayback();

            switch (command.Intent)
            {
                case PlayIntent:
                    Play(command);
                    break;
                case SetIntent:
                    Set(command);
                    break;
                case ResumeIntent:
                    SpotifyPlayer.Play();
                    break;
                case PauseIntent:
                    SpotifyPlayer.Pause();
                    break;
                case NextIntent:
                    SpotifyPlayer.Next();
                    break;
                case PreviousIntent:
                    SpotifyPlayer.Previous();
                    break;
            }
        }

        public void Play(Command command)
        {
            var album = command.GetEntity(AlbumEntity);
            var artist = command.GetEntity(ArtistEntity);
            var playlist = command.GetEntity(PlaylistEntity);
            var track = command.GetEntity(TrackEntity);

            if (track != null)
            {
                var uri = SpotifyPlayer.SearchTrack(track.Value).Items?.FirstOrDefault()?.Uri;
                if (uri != null)
                {
                    SpotifyPlayer.PlayTrack(uri);
                }
            }
            else
            {
                var uri =
                    album != null ? SpotifyPlayer.SearchAlbum(album.Value).FirstOrDefault()?.Uri :
                    artist != null ? SpotifyPlayer.SearchArtist(artist.Value).Items?.FirstOrDefault()?.Uri :
                    playlist != null ? SpotifyPlayer.SearchPlaylist(playlist.Value).Items?.FirstOrDefault()?.Uri :
                    null;
                if (uri != null)
                {
                    SpotifyPlayer.Play(uri);
                }
            }
        }

        public void Set(Command command)
        {
            var volumeStr = command.GetEntity(VolumeEntity)?.Value;
            if (int.TryParse(volumeStr, out int volume))
            {
                SpotifyPlayer.SetVolume(volume);
            }
        }
    }
}
