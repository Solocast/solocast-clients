using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Solocast.Core.Contracts;
using Solocast.Core.Interfaces;
using Solocast.Services.Extensions;
using Solocast.UWP.Infrastructure.Messages;
using Solocast.UWP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Solocast.UWP.Infrastructure.Services
{
	public interface IPlayService
	{
		Task PlayEpisodeAsync(Episode episode);
		void Pause();
		void Stop();
		void Resume();
		void GoToTime(int seconds);

		int Position { get; }
		int TotalSeconds { get; }
		MediaPlaybackState CurrentState { get; }
	}

	public class PlayService : IPlayService
	{
		private MediaPlayer mediaPlayer;

		public PlayService()
		{
			mediaPlayer = new MediaPlayer();
			Messenger.Default.Register<PlayEpisodeMessage>(this, async message => await PlayEpisodeAsync(message.Episode));
		}

		public void Pause()
		{
			mediaPlayer.Pause();
		}

		public async Task PlayEpisodeAsync(Episode episode)
		{
			if (episode.IsLocal)
			{
				var file = await StorageFile.GetFileFromPathAsync(episode.Path);
				mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
			}
			else
			{
				mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(episode.Path));
			}

			mediaPlayer.Play();
		}

		public void Stop()
		{
			mediaPlayer.Pause();
			mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(0);
		}

		public void Resume()
		{
			mediaPlayer.Play();
		}

		public void GoToTime(int seconds)
		{
			mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(seconds);
		}

		public int Position
		{
			get { return (int)mediaPlayer.PlaybackSession.Position.TotalSeconds; }
		}

		public int TotalSeconds
		{
			get { return (int)mediaPlayer.PlaybackSession.NaturalDuration.TotalSeconds; }
		}

		public MediaPlaybackState CurrentState
		{
			get { return mediaPlayer.PlaybackSession.PlaybackState; }
		}
	}
}
