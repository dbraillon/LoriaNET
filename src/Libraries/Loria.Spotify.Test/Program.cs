using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Loria.Spotify.Test
{
    class Program
    {
        static SpotifyPlayer SpotifyPlayer { get; set; }
        static List<Device> AvailableDevices { get; set; }
        static IEnumerable<string> SearchUris { get; set; }

        static void Main(string[] args)
        {
            SpotifyPlayer = new SpotifyPlayer(ConfigurationManager.AppSettings.Get("spotify::APIKey"), "http://localhost");
            SpotifyPlayer.InitializeAsync().GetAwaiter().GetResult();

            var input = string.Empty;
            do
            {
                Console.WriteLine("Welcome to spotify test application, what you want to do?");
                Console.WriteLine("0: Quit");
                Console.WriteLine("1: Get current playback");
                Console.WriteLine("2: Get available devices");
                Console.WriteLine("3: Resume current playback");
                Console.WriteLine("4: Pause current playback");
                Console.WriteLine("5: Skip playback to next");
                Console.WriteLine("6: Skip playback to previous");
                Console.WriteLine("7: Set volume");
                Console.WriteLine("8: Set playback");
                Console.WriteLine("9: Search");
                Console.WriteLine("10: Search & play");

                Console.Write("> ");
                input = Console.ReadLine();

                HandleUserInput(input);
                
                Console.WriteLine();
            }
            while (input != "0");
        }

        static bool HandleUserInput(string input)
        {
            switch (input)
            {
                case "1": return GetCurrentPlayback();
                case "2": return GetAvailableDevices();
                case "3": return ResumePlayback();
                case "4": return PausePlayback();
                case "5": return SkipPlaybackToNext();
                case "6": return SkipPlaybackToPrevious();
                case "7": return SetVolume();
                case "8": return SetPlayback();
                case "9": return Search();
                case "10": return SearchAndPlay();
                default: return false;
            }
        }

        static bool GetCurrentPlayback()
        {
            var currentPlayback = SpotifyPlayer.GetPlayback();
            Console.WriteLine($"{currentPlayback.Device?.Name ?? "No device"} {(currentPlayback.IsPlaying ? "is" : "is not")} playing {currentPlayback.Item?.Name ?? "nothing"}");

            return true;
        }
        static bool GetAvailableDevices()
        {
            var availablesDevices = SpotifyPlayer.GetDevices();
            if (availablesDevices.Devices.Any())
            {
                AvailableDevices = availablesDevices.Devices;

                int index = 0;
                foreach (var device in availablesDevices.Devices)
                {
                    Console.WriteLine($"{index}: {device.Name}");
                }
            }
            else
            {
                Console.WriteLine("No device available.");
            }
            
            return true;
        }

        static bool ResumePlayback()
        {
            var errorResponse = SpotifyPlayer.Play();
            if (errorResponse.HasError())
            {
                Console.WriteLine($"{errorResponse.Error.Status}: {errorResponse.Error.Message}");
                return false;
            }

            return true;
        }
        static bool PausePlayback()
        {
            SpotifyPlayer.Pause();
            
            return true;
        }
        static bool SkipPlaybackToNext()
        {
            SpotifyPlayer.Next();

            return true;
        }
        static bool SkipPlaybackToPrevious()
        {
            SpotifyPlayer.Previous();

            return true;
        }

        static bool SetVolume()
        {
            var playback = SpotifyPlayer.GetPlayback();
            if (playback.Device != null)
            {
                var currentVolumePercent = playback.Device.VolumePercent;
                Console.WriteLine($"Volume is at {currentVolumePercent}%, what you want to do?");
                Console.WriteLine("0: Mute");
                Console.WriteLine("1: 25%");
                Console.WriteLine("2: 50%");
                Console.WriteLine("3: 75%");
                Console.WriteLine("4: 100%");
                Console.WriteLine("5: +10%");
                Console.WriteLine("6: -10%");
                Console.Write("> ");

                var input = Console.ReadLine();
                currentVolumePercent =
                    input == "0" ? 0 :
                    input == "1" ? 25 :
                    input == "2" ? 50 :
                    input == "3" ? 75 :
                    input == "4" ? 100 :
                    input == "5" ? currentVolumePercent + 10 :
                    input == "6" ? currentVolumePercent - 10 :
                    currentVolumePercent;

                currentVolumePercent =
                    currentVolumePercent < 0 ? 0 :
                    currentVolumePercent > 100 ? 100 :
                    currentVolumePercent;

                SpotifyPlayer.SetVolume(currentVolumePercent);
            }
            else
            {
                Console.WriteLine("No device set.");
            }

            return true;
        }
        static bool SetPlayback()
        {
            GetAvailableDevices();
            Console.WriteLine("On which device you want to play music?");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int index))
            {
                var device = AvailableDevices.ElementAtOrDefault(index);
                if (device != null)
                {
                    SpotifyPlayer.SetPlayback(device.Id);
                    return true;
                }
            }
            
            return false;
        }

        static bool Search()
        {
            Console.WriteLine("What you want to search?");
            Console.WriteLine("0: Everything");
            Console.WriteLine("1: Album");
            Console.WriteLine("2: Artist");
            Console.WriteLine("3: Playlist");
            Console.WriteLine("4: Track");

            Console.Write("> ");
            var input = Console.ReadLine();
            
            switch (input)
            {
                case "0": return false;
                case "1": return SearchAlbum();
                case "2": return SearchArtist();
                case "3": return SearchPlaylist();
                case "4": return SearchTrack();
                default: return false;
            }
        }
        static bool SearchAlbum()
        {
            Console.WriteLine("Please search an album:");
            Console.Write("> ");
            var input = Console.ReadLine();

            var results = SpotifyPlayer.SearchAlbum(input);
            var index = 0;

            SearchUris = results.Select(r => r.Uri);
            
            foreach (var result in results)
            {
                Console.WriteLine($"{index++}: {result.Name} from {string.Join(", ", result.Artists.Select(a => a.Name))}");
            }
            
            return true;
        }
        static bool SearchArtist()
        {
            Console.WriteLine("Please search an artist:");
            Console.Write("> ");
            var input = Console.ReadLine();

            var results = SpotifyPlayer.SearchArtist(input);
            var index = 0;

            SearchUris = results.Items.Select(r => r.Uri);
            
            foreach (var result in results.Items)
            {
                Console.WriteLine($"{index++}: {result.Name}");
            }

            return true;
        }
        static bool SearchPlaylist()
        {
            Console.WriteLine("Please search a playlist:");
            Console.Write("> ");
            var input = Console.ReadLine();

            var results = SpotifyPlayer.SearchPlaylist(input);
            var index = 0;

            SearchUris = results.Items.Select(r => r.Uri);
            
            foreach (var result in results.Items)
            {
                Console.WriteLine($"{index++}: {result.Name}");
            }

            return true;
        }
        static bool SearchTrack()
        {
            Console.WriteLine("Please search a track:");
            Console.Write("> ");
            var input = Console.ReadLine();

            var results = SpotifyPlayer.SearchTrack(input);
            var index = 0;

            SearchUris = results.Items.Select(r => r.Uri);
            
            foreach (var result in results.Items)
            {
                Console.WriteLine($"{index++}: {result.Name} from {string.Join(", ", result.Artists.Select(a => a.Name))}");
            }

            return true;
        }

        static bool SearchAndPlay()
        {
            Search();
            Console.WriteLine("What you want to play?");
            Console.Write("> ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int index))
            {
                var uri = SearchUris.ElementAtOrDefault(index);
                if (uri != null)
                {
                    SpotifyPlayer.Play(uri);
                    return true;
                }
            }

            return false;
        }
    }
}
