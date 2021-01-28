﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using LibVLCSharp.Shared;

namespace video_tracker_v2
{
    class VideoPlayer
    {
        public MediaPlayer mPlayer { get; set; }
        public Video currentVideo;
        public Media currentMedia;

        private LibVLC _libVLC;

        public VideoPlayer()
        {
            Core.Initialize();

            _libVLC = new LibVLC();
            mPlayer = new MediaPlayer(_libVLC);

            mPlayer.EnableMouseInput = false;
            // external subtitles
            //mPlayer.AddSlave(MediaSlaveType.Subtitle, )
        }

        public void Pause()
        {
            mPlayer.Pause();
        }

        public void Play(Video video)
        {
            currentVideo = video;

            currentMedia = new Media(_libVLC, new Uri(video.Path));

            mPlayer.Media = currentMedia;

            Play();
            
        }

        public void Play()
        {
            mPlayer.Play();
            // debug Season 7 videos
            // figure out how to close track
            //mPlayer.SpuCount;
            //mPlayer.
            //mPlayer.Pause();
            ////mPlayer.Media.AddOption(":sub-track-id=" + int.MaxValue.ToString());
            //var test = mPlayer.Media.AddOption();
            ////var test = currentMedia.Tracks;
            ////int i = 0;
            ////foreach(MediaTrack track in test)
            ////{
            ////    if(track.TrackType == TrackType.Text)
            ////    {
            ////        break;
            ////    }
            ////    i++;
            ////}

            //mPlayer.Play();
        }

        public void SetTime(double time)
        {
            if (currentMedia != null)
            {
                mPlayer.Time = (long)time * 60;
                currentVideo.CurrentTime = Convert.ToUInt32(time);
            }
        }

        public void SetVolume(int vol)
        {
            mPlayer.Volume = vol;
        }

        public void Dispose()
        {
            _libVLC.Dispose();
            mPlayer.Dispose();
        }

        public Video GetVideo()
        {
            return currentVideo;
        }
    }
}