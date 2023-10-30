using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NOOD.SerializableDictionary;
using NOOD.Extension;
using NOOD.NoodCustomEditor;
using UnityEditor;

namespace NOOD.Sound
{
    public class SoundManager : MonoBehaviorInstance<SoundManager>
    {
        private static SoundDataSO soundData;

        void FindSoundData()
        {
            soundData = Resources.FindObjectsOfTypeAll<SoundDataSO>()[0];
            if(soundData == null)
                Debug.LogError("Can't find SoundData, please create one in Resources folder using Create -> SoundData");
        }

#region SoundRegion
        public static void PlaySound(SoundEnum soundEnum)
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }

            GameObject newObj = new GameObject("SoundPlayer" + soundEnum.ToString());
            newObj.AddComponent<SoundPlayer>();
            AudioSource soundAudioPayer = newObj.AddComponent<AudioSource>();
            AudioClip audioClip = soundAudioPayer.clip = soundData.soundDic.Dictionary[soundEnum.ToString()];
            soundAudioPayer.clip = audioClip;
            soundAudioPayer.Play();
            Destroy(soundAudioPayer.gameObject, audioClip.length);
        }
        public static void StopSound(SoundEnum soundEnum)
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            GameObject soundPlayerObj = GameObject.Find("SoundPlayer" + soundEnum.ToString());
            Destroy(soundPlayerObj);
        }
        public static void StopAllSound()
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            foreach(var soundPlayer in GameObject.FindObjectsOfType<SoundPlayer>())
            {
                Destroy(soundPlayer);
            }
        }
        public static float GetSoundLength(SoundEnum soundEnum)
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            return soundData.soundDic.Dictionary[soundEnum.ToString()].length;
        }
#endregion

#region MusicRegion
        /// <summary>
        /// Play sound with the MusicPlayer gameObject if exists else create one then play music
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMusic(MusicEnum musicEnum)
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            AudioSource musicPlayer = GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>();
            if(musicPlayer == null)
            {
                GameObject newObj = new GameObject("MusicPlayer" + musicEnum.ToString());
                newObj.AddComponent<MusicPlayer>();
                musicPlayer = newObj.AddComponent<AudioSource>();
            }
            AudioClip audioClip = musicPlayer.clip = soundData.musicDic.Dictionary[musicEnum.ToString()];
            musicPlayer.clip = audioClip;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
        /// <summary>
        /// Play one more music in background
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMoreMusic(MusicEnum musicEnum)
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            GameObject newObj = new GameObject("MusicPlayer" + musicEnum.ToString());
            newObj.AddComponent<MusicPlayer>();
            AudioSource musicPlayer = newObj.AddComponent<AudioSource>();
            AudioClip audioClip = musicPlayer.clip = soundData.musicDic.Dictionary[musicEnum.ToString()];
            musicPlayer.clip = audioClip;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }
        public static void ChangeMusic(MusicEnum musicEnum)
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            AudioSource musicPlayer = GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>();
            musicPlayer.gameObject.name = "MusicPlayer" + musicEnum.ToString();
            AudioClip clip = soundData.musicDic.Dictionary[musicEnum.ToString()];
            musicPlayer.clip = clip;
        }
        public static void StopAllMusic()
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            AudioSource musicPlayer = GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>();
            musicPlayer.Stop();
        }
        public static float GetMusicLength(MusicEnum musicEnum)
        {
            if(soundData == null)
            {
                Instance.FindSoundData();
            }
            return soundData.musicDic.Dictionary[musicEnum.ToString()].length;
        }
#endregion
    }

    public class SoundPlayer : MonoBehaviour { }
    public class MusicPlayer : MonoBehaviour { }
}
