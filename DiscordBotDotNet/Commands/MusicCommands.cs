using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotDotNet.Commands
{
    public class MusicCommands : BaseCommandModule
    {
        private readonly LavalinkExtension _Lavalink;
        private LavalinkNodeConnection _LavalinkNode;
        private LavalinkGuildConnection _LavalinkGuild;
        private Queue<LavalinkTrack> _Playlist;
        private CommandContext _Cmdctx;

        public MusicCommands(LavalinkExtension lavaLink)
        {
            _Lavalink = lavaLink;
            _Playlist = new Queue<LavalinkTrack>();
        }

        [Command("play")]
        public async Task EnqueueTrack(CommandContext ctx, [RemainingText] string input)
        {
            _Cmdctx = ctx;

            if (ctx.Member?.VoiceState?.Channel == null)
            {
                await ctx.RespondAsync("You must be connected to a voice channel to use this command.");
                return;
            }

            var lavaconfig = new LavalinkConfiguration
            {
                Password = ""
            };

            if (_LavalinkNode == null || !_LavalinkNode.IsConnected)
                _LavalinkNode = await _Lavalink.ConnectAsync(lavaconfig);

            if ((_LavalinkGuild == null || !_LavalinkGuild.IsConnected))
            {
                _LavalinkGuild = await _LavalinkNode.ConnectAsync(ctx.Member.VoiceState.Channel);
                _LavalinkGuild.PlaybackFinished += PlayTrack;
            }

            var tracks = new LavalinkLoadResult();

            Uri uri;

            if (Uri.TryCreate(input, UriKind.Absolute, out uri))
                tracks = await _LavalinkNode.GetTracksAsync(uri);
            else
                tracks = await _LavalinkNode.GetTracksAsync(input);

            switch (tracks.LoadResultType)
            {
                case LavalinkLoadResultType.SearchResult:

                case LavalinkLoadResultType.TrackLoaded:
                    _Playlist.Enqueue(tracks.Tracks.First());
                    await ctx.RespondAsync($"**[Enqueued]** {tracks.Tracks.First().Title}");
                    break;

                case LavalinkLoadResultType.PlaylistLoaded:
                    foreach (LavalinkTrack track in tracks.Tracks)
                        _Playlist.Enqueue(track);
                    await ctx.RespondAsync($"Playlist Loaded.");
                    break;

                case LavalinkLoadResultType.LoadFailed:
                    await ctx.RespondAsync($"Track could not be loaded.");
                    break;

                case LavalinkLoadResultType.NoMatches:
                    await ctx.RespondAsync($"Track Not Found.");
                    break;

                default:
                    await ctx.RespondAsync($"Error.");
                    return;
            }

            if (string.IsNullOrEmpty(_LavalinkGuild.CurrentState.CurrentTrack.Title))
            {
                _LavalinkGuild.Play(_Playlist.Peek());
            }
        }

        private async Task PlayTrack(TrackFinishEventArgs e)
        {
            if (e.Reason == TrackEndReason.Replaced)
                return;

            if (e.Player.Channel.Users.Count() == 1)
            {
                _Playlist.Clear();
                await _Cmdctx.RespondAsync("Good bye :person_bowing:");
                _LavalinkGuild.Disconnect();
                await _LavalinkNode.StopAsync();
                return;
            }

            if (_Playlist.Count > 1)
            {
                _Playlist.Dequeue();
                _LavalinkGuild.Play(_Playlist.Peek());
                await _Cmdctx.RespondAsync($"**[Playing]** {_Playlist.Peek().Title}");
            }
            else if (_Playlist.Count == 1)
            {
                _Playlist.Clear();
                await Task.Delay(2000);
                await _Cmdctx.RespondAsync("Queue concluded.");
                await Task.Delay(30000);
                _LavalinkGuild.Disconnect();
                await _LavalinkNode.StopAsync();
            }
        }

        [Command("skip")]
        public async Task SkipTrack(CommandContext ctx)
        {
            await ctx.RespondAsync($"**[Skipping]** {_Playlist.Peek().Title}");
            _Playlist.Dequeue();
            _LavalinkGuild.Play(_Playlist.Peek());
            await ctx.RespondAsync($"**[Playing]** {_Playlist.Peek().Title}");
        }

        [Command("stop")]
        public async Task StopMusic(CommandContext ctx)
        {
            _LavalinkGuild.Disconnect();
            await _LavalinkNode.StopAsync();
            _Playlist.Clear();
        }

        [Command("pause")]
        public async Task PauseMusic(CommandContext ctx)
        {
            _LavalinkGuild.Pause();
            await Task.CompletedTask;
        }

        [Command("resume")]
        public async Task ResumeMusic(CommandContext ctx)
        {
            _LavalinkGuild.Resume();
            await Task.CompletedTask;
        }

        [Command("playlist")]
        public async Task GetPlaylist(CommandContext ctx)
        {
            string playlist = string.Empty;
            TimeSpan totalTrackTime = new TimeSpan();
            int count = 0;

            foreach (var track in _Playlist)
            {
                if(count < 8)
                {
                    count++;
                    playlist += $"• {track.Title} ({track.Length.ToString("mm\\:ss")})\n";
                }
                totalTrackTime += track.Length;
            }

            if(_Playlist.Count > 0)
                await ctx.RespondAsync($"**Playlist - Total Queued Time ({totalTrackTime.ToString("hh\\:mm\\:ss")}**)\n{playlist}{ (_Playlist.Count >= 8 ? $"(+{(_Playlist.Count-8).ToString()} tracks)" : "") }\n **[Currently Playing]** {_Playlist.Peek().Title}");
            else
                await ctx.RespondAsync($"Playlist: Empty");
        }
    }
}
