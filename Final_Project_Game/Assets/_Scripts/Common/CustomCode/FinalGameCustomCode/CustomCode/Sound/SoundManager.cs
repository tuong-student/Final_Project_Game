using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using NOOD.Sound;

namespace NOOD.Sound
{
    public class SoundManager
    {
        #region Object Init
        private static SoundDataSO soundData;
        private static GameObject soundManagerGlobal;
        #endregion

        #region List
        private static List<MusicPlayer> activeMusicPlayers;
        private static List<SoundPlayer> activeSoundPlayers;
        private static List<MusicPlayer> disableMusicPlayers;
        private static List<SoundPlayer> disableSoundPlayers;
        #endregion

        public static void FindSoundData()
        {
            SoundDataSO[] soundDataSOs = Resources.LoadAll<SoundDataSO>("");
            if(soundDataSOs.Length > 0)
                soundData = Resources.FindObjectsOfTypeAll<SoundDataSO>()[0];
            if(soundData == null)
                Debug.LogError("Can't find SoundData, please create one in Resources folder using Create -> SoundData");
            else
                Debug.Log("Load SoundData success");
        }

        private static void InitIfNeed()
        {
            if(soundManagerGlobal == null)
            {
                Debug.Log("SoundManager Init");
                soundManagerGlobal = new GameObject("SoundManagerGlobal");
                disableMusicPlayers = new List<MusicPlayer>();
                disableSoundPlayers = new List<SoundPlayer>();
                activeMusicPlayers = new List<MusicPlayer>();
                activeSoundPlayers = new List<SoundPlayer>();
            }
        }

#region SoundRegion
        public static void PlaySound(SoundEnum soundEnum, float volume = 1)
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            AudioSource soundAudioPayer;
            SoundPlayer soundPlayer;
            if (disableSoundPlayers.Any(x => x.soundType == soundEnum))
            {
                soundPlayer = disableSoundPlayers.First(x => x.soundType == soundEnum);
                soundAudioPayer = soundPlayer.GetComponent<AudioSource>();
                soundPlayer.gameObject.SetActive(true);

                // Remove when get
                disableSoundPlayers.Remove(soundPlayer);
            }
            else
            {
                GameObject newObj = new GameObject("SoundPlayer" + soundEnum.ToString());
                newObj.transform.SetParent(soundManagerGlobal.transform);
                soundPlayer = newObj.AddComponent<SoundPlayer>();
                soundPlayer.soundType = soundEnum;
                soundAudioPayer = newObj.AddComponent<AudioSource>();
            }
            AudioClip audioClip = soundData.soundDic.Dictionary[soundEnum.ToString()];

            soundAudioPayer.playOnAwake = false;
            soundAudioPayer.volume = volume;
            soundAudioPayer.clip = audioClip;
            soundAudioPayer.Play();
            activeSoundPlayers.Add(soundPlayer);

            // Add to list when disable
            NoodyCustomCode.StartDelayFunction(() =>
            {
                soundAudioPayer.gameObject.SetActive(false);
                activeSoundPlayers.Remove(soundPlayer);
                disableSoundPlayers.Add(soundPlayer);
            }, audioClip.length);
        }
        /// <summary>
        /// Stop all soundPlayers has the same soundEnum
        /// </summary>
        /// <param name="soundEnum"></param>
        public static void StopSound(SoundEnum soundEnum)
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }
            SoundPlayer[] soundPlayerArray = GameObject.FindObjectsByType<SoundPlayer>(sortMode: FindObjectsSortMode.None).Where(x => x.soundType == soundEnum).ToArray();

            foreach(var soundPlayer in soundPlayerArray)
            {
                if(soundPlayer.isActiveAndEnabled)
                {
                    soundPlayer.gameObject.SetActive(false);
                    activeSoundPlayers.Remove(soundPlayer);
                    disableSoundPlayers.Add(soundPlayer);
                }
            }
        }
        /// <summary>
        /// Stop all soundPlayer found
        /// </summary>
        public static void StopAllSound()
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            foreach(var soundPlayer in GameObject.FindObjectsOfType<SoundPlayer>())
            {
                if(soundPlayer.isActiveAndEnabled)
                {
                    soundPlayer.gameObject.SetActive(false);

                    activeSoundPlayers.Remove(soundPlayer);
                    disableSoundPlayers.Add(soundPlayer);
                }
            }
        }
        /// <summary>
        /// Get the sound length base on soundEnum (data from soundData)
        /// </summary>
        /// <param name="soundEnum"></param>
        /// <returns></returns>
        public static float GetSoundLength(SoundEnum soundEnum)
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            return soundData.soundDic.Dictionary[soundEnum.ToString()].length;
        }
#endregion

#region MusicRegion
        /// <summary>
        /// Play music with new MusicPlayer gameObject if exist no play, else play
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMusic(MusicEnum musicEnum, float volume = 1)
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            if (activeMusicPlayers.Any(x => x.musicType == musicEnum)) return;

            MusicPlayer musicPlayer;
            if(disableMusicPlayers.Any(x => x.musicType == musicEnum))
            {
                musicPlayer = disableMusicPlayers.First(x => x.musicType == musicEnum);
                musicPlayer.gameObject.SetActive(true);
                disableMusicPlayers.Remove(musicPlayer);
            }
            else
            {
                GameObject newObj = new GameObject("MusicPlayer");
                newObj.transform.SetParent(soundManagerGlobal.transform);
                musicPlayer = newObj.AddComponent<MusicPlayer>();
            }

            musicPlayer.musicType = musicEnum;
            activeMusicPlayers.Add(musicPlayer);

            AudioSource musicAudioSource;
            musicAudioSource = musicPlayer.gameObject.AddComponent<AudioSource>();
            AudioClip audioClip = musicAudioSource.clip = soundData.musicDic.Dictionary[musicEnum.ToString()];

            musicAudioSource.volume = volume;
            musicAudioSource.clip = audioClip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }

        /// <summary>
        /// Change all music volumes that have the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        /// <param name="volume"></param>
        public static void ChangeMusicVolume(MusicEnum musicEnum, float volume)
        {
            InitIfNeed();
            foreach(var musicPlayer in activeMusicPlayers)
            {
                if(musicPlayer.musicType == musicEnum)
                {
                    AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
                    audioSource.volume = volume;
                }
            }
        }
        /// <summary>
        /// Change music clip of audio source from sourceMusicEnum clip to toMusicEnum clip
        /// </summary>
        /// <param name="sourceMusicEnum"></param>
        /// <param name="toMusicEnum"></param>
        public static void ChangeMusic(MusicEnum sourceMusicEnum, MusicEnum toMusicEnum)

        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            MusicPlayer musicPlayer;
            AudioSource musicAudioSource;
            if(activeMusicPlayers.Any(x => x.musicType == sourceMusicEnum))
            {
                musicPlayer = activeMusicPlayers.First(x => x.musicType == sourceMusicEnum);
                musicAudioSource = musicPlayer.GetComponent<AudioSource>();
                musicPlayer.musicType = toMusicEnum;
                musicAudioSource.clip = soundData.musicDic.Dictionary[toMusicEnum.ToString()];
            }
            else
            {
                Debug.Log("No source music enum, just play to music enum instead");
                PlayMusic(toMusicEnum);
            }
        }

        public static void ChangeMusicAndVolume(MusicEnum sourceMusicEnum, MusicEnum toMusicEnum, float volume)
        {
            ChangeMusic(sourceMusicEnum, toMusicEnum);
            ChangeMusicVolume(toMusicEnum, volume);
        }

        #region Stop Music
        /// <summary>
        /// Stop all MusicPlayer has the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void StopMusic(MusicEnum musicEnum)
        {
            InitIfNeed();
            if(activeMusicPlayers.Any(x => x.musicType == musicEnum))
            {
                MusicPlayer musicPlayer =  activeMusicPlayers.First(x => x.musicType == musicEnum);

                musicPlayer.GetComponent<AudioSource>().Stop();
                musicPlayer.gameObject.SetActive(false);
                activeMusicPlayers.Remove(musicPlayer);
                disableMusicPlayers.Add(musicPlayer);
            } 
        }
        /// <summary>
        /// Stop all MusicPlayer found
        /// </summary>
        public static void StopAllMusic()
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }
            MusicPlayer[] musicPlayerArray = GameObject.FindObjectsOfType<MusicPlayer>();
            foreach(var musicPlayer in musicPlayerArray)
            {
                musicPlayer.GetComponent<AudioSource>().Stop();
                musicPlayer.gameObject.SetActive(false);
                activeMusicPlayers.Remove(musicPlayer);
                disableMusicPlayers.Add(musicPlayer);
            }
        }
        #endregion
        /// <summary>
        /// Get music length (data from SoundData)
        /// </summary>
        /// <param name="musicEnum"></param>
        /// <returns></returns>
        public static float GetMusicLength(MusicEnum musicEnum)
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            return soundData.musicDic.Dictionary[musicEnum.ToString()].length;
        }
        #endregion
    }

    public class SoundPlayer : MonoBehaviour 
    {
        public SoundEnum soundType;
    }
    public class MusicPlayer : MonoBehaviour 
    {
        public MusicEnum musicType;
    }
}
