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
        private static List<MusicPlayer> enableMusicPlayers;
        private static List<SoundPlayer> enableSoundPlayers;
        private static List<MusicPlayer> disableMusicPlayers;
        private static List<SoundPlayer> disableSoundPlayers;
#endregion

#region Parameter
        private static float globalSoundVolume;
        private static float globalMusicVolume;
        public static float GlobalSoundVolume
        {
            get
            {
                return globalSoundVolume;
            }
            set
            {
                globalSoundVolume = value;
                AppliedVolume();
            }
        }
        public static float GlobalMusicVolume
        {
            get
            {
                return globalMusicVolume;
            }
            set
            {
                globalMusicVolume = value;
                AppliedVolume();
            }
        }
#endregion

#region Init
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
                enableMusicPlayers = new List<MusicPlayer>();
                enableSoundPlayers = new List<SoundPlayer>();
                GlobalMusicVolume = 1;
                GlobalSoundVolume = 1;
            }
        }
#endregion

#region SoundRegion
        /// <summary>
        /// Play sound with your volume
        /// </summary>
        /// <param name="soundEnum"></param>
        /// <param name="volume"></param>
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
            enableSoundPlayers.Add(soundPlayer);

            // Add to list when disable
            NoodyCustomCode.StartDelayFunction(() =>
            {
                soundAudioPayer.gameObject.SetActive(false);
                enableSoundPlayers.Remove(soundPlayer);
                disableSoundPlayers.Add(soundPlayer);
            }, audioClip.length);
        }
        /// <summary>
        /// Play sound with globalSoundVolume
        /// </summary>
        /// <param name="soundEnum"></param>
        public static void PlaySound(SoundEnum soundEnum)
        {
            PlaySound(soundEnum, GlobalSoundVolume);
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
                    enableSoundPlayers.Remove(soundPlayer);
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

                    enableSoundPlayers.Remove(soundPlayer);
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
        /// Play music with new MusicPlayer gameObject and with your volume
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMusic(MusicEnum musicEnum, float volume = 1, bool alwaysPlay = false)
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }

            if (enableMusicPlayers.Any(x => x.musicType == musicEnum)) return;

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
            musicPlayer.isAlwaysPlay = alwaysPlay;
            enableMusicPlayers.Add(musicPlayer);

            AudioSource musicAudioSource;
            musicAudioSource = musicPlayer.gameObject.AddComponent<AudioSource>();
            AudioClip audioClip = musicAudioSource.clip = soundData.musicDic.Dictionary[musicEnum.ToString()];

            musicAudioSource.volume = volume;

            musicAudioSource.clip = audioClip;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
        /// <summary>
        /// Play music with globalMusicVolume
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMusic(MusicEnum musicEnum)
        {
            PlayMusic(musicEnum, GlobalMusicVolume);
        }
        /// <summary>
        /// Change all music volumes that have the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        /// <param name="volume"></param>
        public static void ChangeMusicVolume(MusicEnum musicEnum, float volume)
        {
            InitIfNeed();
            foreach(var musicPlayer in enableMusicPlayers)
            {
                if(musicPlayer.musicType == musicEnum)
                {
                    AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
                    audioSource.volume = volume;
                }
            }
        }
        public static void ChangeMusicVolumeAll(float volume)
        {
            InitIfNeed();
            foreach(var musicPlayer in enableMusicPlayers)
            {
                AudioSource audioSource = musicPlayer.GetComponent<AudioSource>();
                audioSource.volume = volume;
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
            if(enableMusicPlayers.Any(x => x.musicType == sourceMusicEnum))
            {
                musicPlayer = enableMusicPlayers.First(x => x.musicType == sourceMusicEnum);
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
        /// Stop all MusicPlayer with the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void StopMusic(MusicEnum musicEnum)
        {
            InitIfNeed();
            if(enableMusicPlayers.Any(x => x.musicType == musicEnum))
            {
                MusicPlayer musicPlayer =  enableMusicPlayers.First(x => x.musicType == musicEnum);

                musicPlayer.GetComponent<AudioSource>().Stop();
                musicPlayer.gameObject.SetActive(false);
                enableMusicPlayers.Remove(musicPlayer);
                disableMusicPlayers.Add(musicPlayer);
            } 
        }
        /// <summary>
        /// Resume MusicPlayer with the same musicEnum
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void ResumeMusic(MusicEnum musicEnum)
        {
            InitIfNeed();
            if(enableMusicPlayers.Any(x => x.musicType == musicEnum))
            {
                MusicPlayer musicPlayer =  disableMusicPlayers.First(x => x.musicType == musicEnum);

                musicPlayer.gameObject.SetActive(true);
                musicPlayer.TryGetComponent<AudioSource>(out AudioSource audioSource);
                audioSource.Play();
                audioSource.volume = GlobalMusicVolume;

                enableMusicPlayers.Add(musicPlayer);
                disableMusicPlayers.Remove(musicPlayer);
            } 
        }
        /// <summary>
        /// Stop all MusicPlayers found
        /// </summary>
        public static void StopAllMusic()
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }
            foreach(var musicPlayer in enableMusicPlayers)
            {
                musicPlayer.GetComponent<AudioSource>().Stop();
                musicPlayer.gameObject.SetActive(false);
                enableMusicPlayers.Remove(musicPlayer);
                disableMusicPlayers.Add(musicPlayer);
            }
        }
        /// <summary>
        /// Resume all MusicPlayers found
        /// </summary>
        public static void ResumeAllMusic()
        {
            InitIfNeed();
            if(soundData == null)
            {
                FindSoundData();
            }
            foreach(var musicPlayer in disableMusicPlayers)
            {
                musicPlayer.TryGetComponent<AudioSource>(out AudioSource audioSource);

                audioSource.Play();
                audioSource.volume = GlobalMusicVolume;

                musicPlayer.gameObject.SetActive(true);
                enableMusicPlayers.Add(musicPlayer);
                disableMusicPlayers.Remove(musicPlayer);
            }
        }
        #endregion
#endregion

#region Support functions
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
        public static bool IsMusicPlaying(MusicEnum musicEnum)
        {
            InitIfNeed();
            return enableMusicPlayers.Any(x => x.musicType == musicEnum);
        }
        private static void AppliedVolume()
        {
            // To sound
            SoundPlayer[] soundPlayers = GameObject.FindObjectsByType<SoundPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach(var sound in soundPlayers)
            {
                if(sound.TryGetComponent<AudioSource>(out AudioSource audioSource))
                {
                    audioSource.volume = globalSoundVolume;
                }
            }
            // To music
            MusicPlayer[] musicPlayers = GameObject.FindObjectsByType<MusicPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach(var musicPlayer in musicPlayers)
            {
                if(globalMusicVolume == 0)
                {
                    if (musicPlayer.isAlwaysPlay == true) continue;
                    if(musicPlayer.TryGetComponent<AudioSource>(out AudioSource audioSource))
                    {
                        audioSource.volume = globalMusicVolume;
                    }
                    musicPlayer.gameObject.SetActive(false);
                    disableMusicPlayers.Add(musicPlayer);
                    enableMusicPlayers.Remove(musicPlayer);
                }
                else
                {
                    if(musicPlayer.TryGetComponent<AudioSource>(out AudioSource audioSource))
                    {
                        audioSource.volume = globalMusicVolume;
                    }
                }
            }
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
        public bool isAlwaysPlay;
    }
}
