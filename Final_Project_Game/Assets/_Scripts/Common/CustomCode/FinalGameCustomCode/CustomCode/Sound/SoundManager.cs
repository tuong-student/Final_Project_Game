using UnityEngine;

namespace NOOD.Sound
{
    public class SoundManager
    {
        private static SoundDataSO soundData;

        public static void FindSoundData()
        {
            soundData = Resources.FindObjectsOfTypeAll<SoundDataSO>()[0];
            if(soundData == null)
                Debug.LogError("Can't find SoundData, please create one in Resources folder using Create -> SoundData");
        }

#region SoundRegion
        public static void PlaySound(SoundEnum soundEnum, bool isMute)
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            GameObject newObj = new GameObject("SoundPlayer" + soundEnum.ToString());
            newObj.AddComponent<SoundPlayer>();
            AudioSource soundAudioPayer = newObj.AddComponent<AudioSource>();
            AudioClip audioClip = soundAudioPayer.clip = soundData.soundDic.Dictionary[soundEnum.ToString()];
            if (isMute)
                soundAudioPayer.volume = 0;
            else
                soundAudioPayer.volume = 1;
            soundAudioPayer.clip = audioClip;
            soundAudioPayer.Play();
            UnityEngine.Object.Destroy(soundAudioPayer.gameObject, audioClip.length);
        }
        public static void StopSound(SoundEnum soundEnum)
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            GameObject soundPlayerObj = GameObject.Find("SoundPlayer" + soundEnum.ToString());
            UnityEngine.Object.Destroy(soundPlayerObj);
        }
        public static void StopAllSound()
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            foreach(var soundPlayer in GameObject.FindObjectsOfType<SoundPlayer>())
            {
                UnityEngine.Object.Destroy(soundPlayer);
            }
        }
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
        /// Play sound with the MusicPlayer gameObject if exists else create one then play music
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMusic(MusicEnum musicEnum, bool isMute)
        {
            if(soundData == null)
            {
                FindSoundData();
            }
            AudioSource musicPlayer;
            MusicPlayer musicPlayerObject = GameObject.FindObjectOfType<MusicPlayer>();
            if(musicPlayerObject == null)
            {
                GameObject newObj = new GameObject("MusicPlayer" + musicEnum.ToString());
                newObj.AddComponent<MusicPlayer>();
                musicPlayer = newObj.AddComponent<AudioSource>();
            }
            else
                musicPlayer = musicPlayerObject.GetComponent<AudioSource>();
            AudioClip audioClip = musicPlayer.clip = soundData.musicDic.Dictionary[musicEnum.ToString()];
            if (isMute)
                musicPlayer.volume = 0;
            else
                musicPlayer.volume = 1;
            musicPlayer.clip = audioClip;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }


        public static void AdjustMusicTemporary(bool isMute)
        {
            AudioSource musicPlayer;
            MusicPlayer musicPlayerObject = GameObject.FindObjectOfType<MusicPlayer>();
            if (musicPlayerObject == null)
            {
                Debug.Log("Music Player Object is Null");
                return;
            }
            else
                musicPlayer = musicPlayerObject.GetComponent<AudioSource>();
            if (isMute)
                musicPlayer.volume = 0;
            else
                musicPlayer.volume = 1;
        }
        /// <summary>
        /// Play one more music in background
        /// </summary>
        /// <param name="musicEnum"></param>
        public static void PlayMoreMusic(MusicEnum musicEnum)
        {
            if(soundData == null)
            {
                FindSoundData();
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
                FindSoundData();
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
                FindSoundData();
            }
            AudioSource musicPlayer = GameObject.FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>();
            musicPlayer.Stop();
        }
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

    public class SoundPlayer : MonoBehaviour { }
    public class MusicPlayer : MonoBehaviour { }
}
