using System;
using XFrame.Core;
using UnityEngine;
using UnityEngine.Audio;
using XFrame.Modules.Pools;
using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Tasks;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Audios
{
    /// <summary>
    /// 声音模块
    /// </summary>
    [CommonModule]
    [XType(typeof(IAudioModule))]
    public partial class AudioModule : ModuleBase, IAudioModule
    {
        #region Inner Fields
        private float m_Volume;
        private Transform m_Root;
        private AudioMixer m_Mixer;
        private AudioMixerGroup m_MainGroup;
        private IPool<Audio> m_AudioPool;
        private Dictionary<string, Group> m_Groups;
        private const string MAIN_GROUP = "Main";
        #endregion

        #region IModule Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_Volume = 1.0f;
            m_Mixer = Init.Inst.Data.AudioMixer;
            m_Root = new GameObject("Audios").transform;
            m_AudioPool = Domain.GetModule<IPoolModule>().GetOrNew<Audio>();
            m_Groups = new Dictionary<string, Group>();
            m_MainGroup = m_Mixer.FindMatchingGroups("Master")[0];
        }
        #endregion

        #region Intreface
        /// <summary>
        /// 总音量
        /// </summary>
        public float Volume
        {
            get => m_Volume;
            set
            {
                m_Volume = value;
                foreach (Group group in m_Groups.Values)
                    group.Volume = value;
            }
        }

        /// <summary>
        /// 主声音组
        /// </summary>
        public IAudioGroup MainGroup => GetOrNewGroup(MAIN_GROUP);

        /// <summary>
        /// 获取或创建一个音乐组
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <returns>音乐组</returns>
        public IAudioGroup GetOrNewGroup(string groupName)
        {
            return InnerGetOrNewGroup(groupName);
        }

        /// <summary>
        /// 移除音乐组
        /// </summary>
        /// <param name="groupName">组名称</param>
        public void RemoveGroup(string groupName)
        {
            if (m_Groups.TryGetValue(groupName, out Group group))
            {
                group.OnDestroy();
                m_Groups.Remove(groupName);
            }
        }

        public IAudio Play(string name, Action callback = null)
        {
            return Play(name, MAIN_GROUP, true, callback);
        }

        public IAudio Play(string name, bool autoRelease, Action callback = null)
        {
            return Play(name, MAIN_GROUP, autoRelease, callback);
        }

        public IAudio Play(string name, string groupName, Action callback = null)
        {
            return Play(name, groupName, true, callback);
        }

        public IAudio Play(string name, string groupName, bool autoRelease, Action callback = null)
        {
            IAudio audio = InnerReadyAudio(name, groupName, autoRelease);
            audio.Play(callback);
            return audio;
        }

        public IAudio PlayLoop(string name, bool autoRelease = false)
        {
            return PlayLoop(name, MAIN_GROUP, autoRelease);
        }

        public IAudio PlayLoop(string name, string groupName, bool autoRelease = false)
        {
            IAudio audio = InnerReadyAudio(name, groupName, autoRelease);
            audio.PlayLoop();
            return audio;
        }

        public XTask<IAudio> PlayAsync(string name, Action callback = null)
        {
            return PlayAsync(name, true, callback);
        }

        public XTask<IAudio> PlayAsync(string name, bool autoRelease, Action callback = null)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, MAIN_GROUP, autoRelease);
            task.OnCompleted(() => task.GetResult().Play(callback));
            return task;
        }

        public XTask<IAudio> PlayAsync(string name, string groupName, Action callback = null)
        {
            return PlayAsync(name, groupName, true, callback);
        }

        public XTask<IAudio> PlayAsync(string name, string groupName, bool autoRelease, Action callback = null)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, groupName, autoRelease);
            task.OnCompleted(() => task.GetResult().Play(callback));
            return task;
        }

        public XTask<IAudio> PlayLoopAsync(string name, bool autoRelease = false)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, MAIN_GROUP, autoRelease);
            task.OnCompleted(() => task.GetResult().PlayLoop());
            return task;
        }

        public XTask<IAudio> PlayLoopAsync(string name, string groupName, bool autoRelease = false)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, groupName, autoRelease);
            task.OnCompleted(() => task.GetResult().PlayLoop());
            return task;
        }
        #endregion

        #region Inner Imeplement
        private Audio InnerReadyAudio(string name, string groupName, bool autoRelease)
        {
            Group group = InnerGetOrNewGroup(groupName);
            Audio audio = InnerCreateAudio(name, autoRelease);
            group.Add(audio);
            return audio;
        }

        private XTask<IAudio> InnerReadyAudioAsync(string name, string groupName, bool autoRelease)
        {
            Group group = InnerGetOrNewGroup(groupName);
            XTask<IAudio> audio = InnerCreateAudioAsync(name, autoRelease);
            audio.OnCompleted(() => group.Add(audio.GetResult()));
            return audio;
        }

        private Audio InnerCreateAudio(string name, bool autoRelease)
        {
            AudioClip clip = Domain.GetModule<IResModule>().Load<AudioClip>($"{Constant.AUDIO_PATH}/{name}");
            Audio audio = m_AudioPool.Require();
            audio.OnInit(this, m_Root, m_MainGroup, clip, autoRelease);
            return audio;
        }

        private async XTask<IAudio> InnerCreateAudioAsync(string name, bool autoRelease)
        {
            AudioClip clip = await Domain.GetModule<IResModule>().LoadAsync<AudioClip>($"{Constant.AUDIO_PATH}/{name}");
            if (clip != null)
            {
                Audio audio = m_AudioPool.Require();
                audio.OnInit(this, m_Root, m_MainGroup, clip, autoRelease);
                return audio;
            }
            else
            {
                return null;
            }
        }

        private Group InnerGetOrNewGroup(string groupName)
        {
            if (!m_Groups.TryGetValue(groupName, out Group group))
            {
                group = new Group();
                group.OnInit();
                m_Groups.Add(groupName, group);
            }
            return group;
        }
        #endregion
    }
}
