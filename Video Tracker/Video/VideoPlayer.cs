﻿using System;
using LibVLCSharp.Shared;
using LibVLCSharp.Shared.Structures;

namespace video_tracker_v2
{
    /// <summary>
    /// Handles video playing
    /// </summary>
    class VideoPlayer
    {
        public long Time
        {
            get
            {
                return mPlayer.Time;
            }
        }

        public long VideoDuration
        {
            get
            {
                if(currentMedia != null)
                {
                    return currentMedia.Duration;
                }
                return -1;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if(currentMedia != null)
                {
                    return mPlayer.IsPlaying;
                }
                return false;
            }
        }

        public bool MediaLoaded
        {
            get
            {
                return currentMedia != null;
            }
        }

        public TrackDescription[] SpuDescription
        {
            get
            {
                return mPlayer.SpuDescription;
            }
        }

        // not sure if some of them are video files
        public readonly static string ValidExtensions = ".asx " +
            ".gxf" +
            ".m2v" +
            ".m3u" +
            ".m4v" +
            ".mpeg1" +
            ".mpeg2" +
            ".mts" +
            ".mxf" +
            ".ogm" +
            ".pls" +
            ".bup" +
            ".a52" +
            ".acc" +
            ".b4s" +
            ".divx" +
            ".dv" +
            ".flv" +
            ".m1v" +
            ".m2ts" +
            ".mkv" +
            ".mov" +
            ".mpeg4" +
            ".oma" +
            ".spx" +
            ".ts" +
            ".vlc" +
            ".vob" +
            ".xspf" +
            ".dat" +
            ".ifo" +
            ".part" +
            ".3g2" +
            ".avi" +
            ".mpeg" +
            ".mpg" +
            ".flac" +
            ".m4a" +
            ".mp1" +
            ".ogg" +
            ".wav" +
            ".xm" +
            ".3gp" +
            ".wmv" +
            ".wma" +
            ".mp4";

        public Video currentVideo { get; set; }

        private MediaPlayer mPlayer;
        private Media currentMedia;


        private LibVLC _libVLC;

        public VideoPlayer()
        {
            Core.Initialize();

            _libVLC = new LibVLC();
            mPlayer = new MediaPlayer(_libVLC);

            mPlayer.EnableMouseInput = false;
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
        }

        public void SetTime(double time)
        {
            if (currentMedia != null)
            {
                long t = ((long)time) * 60;
                mPlayer.Time = t;
                currentVideo.CurrentTime = Convert.ToUInt32(time);
            }
        }

        public void SetVolume(int vol)
        {
            mPlayer.Volume = vol;
        }

        public void Dispose()
        {
            mPlayer.Dispose();
            _libVLC.Dispose();
        }

        public Video GetVideo()
        {
            return currentVideo;
        }

        public void MoveBy(int msTime)
        {
            // sometimes crashes, even though media is loaded
            try
            {
                    if (msTime < 0 && mPlayer.Time - msTime < 0 && mPlayer.Media != null)
                    {
                        mPlayer.Time = 0;
                        return;
                    }

                    mPlayer.Time += msTime;
            }
            catch
            {
            }
        }

        public MediaPlayer GetMediaPlayer()
        {
            return mPlayer;
        }

        public void AddSub(string path)
        {
            mPlayer.AddSlave(MediaSlaveType.Subtitle, "file:///" + path, true);
        }

        public void AddCallbackOnTimeChanged(EventHandler<MediaPlayerTimeChangedEventArgs> func)
        {
            mPlayer.TimeChanged += func;
        }

        public void AddCallbackOnMediaPlaying(EventHandler<EventArgs> func)
        {
            mPlayer.Playing += func;
        }

        public void RemoveCallbackOnMediaPlaying(EventHandler<EventArgs> func)
        {
            mPlayer.Playing -= func;
        }

        public void SetSpu(int spu)
        {
            mPlayer.SetSpu(spu);
        }
    }
}