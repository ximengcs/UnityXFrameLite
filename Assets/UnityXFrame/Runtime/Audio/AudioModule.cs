using System;
using XFrame.Core;
using UnityEngine;
using UnityEngine.Audio;
using XFrame.Modules.Pools;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using System.Collections.Generic;

namespace UnityXFrame.Core.Audios
{
    /// <summary>
    /// 声音模块
    /// </summary>
    [XModule]
    public partial class AudioModule : SingletonModule<AudioModule>
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
            m_AudioPool = PoolModule.Inst.GetOrNew<Audio>();
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
            return Play(name, MAIN_GROUP, callback);
        }

        public IAudio Play(string name, string groupName, Action callback = null)
        {
            IAudio audio = InnerReadyAudio(name, groupName);
            audio.Play(callback);
            return audio;
        }

        public IAudio PlayLoop(string name)
        {
            return PlayLoop(name, MAIN_GROUP);
        }

        public IAudio PlayLoop(string name, string groupName)
        {
            IAudio audio = InnerReadyAudio(name, groupName);
            audio.PlayLoop();
            return audio;
        }

        public XTask<IAudio> PlayAsync(string name, Action callback = null)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, MAIN_GROUP);
            task.OnComplete(() => task.Data.Play(callback));
            return task;
        }

        public XTask<IAudio> PlayAsync(string name, string groupName, Action callback = null)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, groupName);
            task.OnComplete(() => task.Data.Play(callback));
            return task;
        }

        public XTask<IAudio> PlayLoopAsync(string name)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, MAIN_GROUP);
            task.OnComplete(() => task.Data.PlayLoop());
            return task;
        }

        public XTask<IAudio> PlayLoopAsync(string name, string groupName)
        {
            XTask<IAudio> task = InnerReadyAudioAsync(name, groupName);
            task.OnComplete(() => task.Data.PlayLoop());
            return task;
        }
        #endregion

        #region Inner Imeplement
        private Audio InnerReadyAudio(string name, string groupName)
        {
            Group group = InnerGetOrNewGroup(groupName);
            Audio audio = InnerCreateAudio(name);
            group.Add(audio);
            return audio;
        }

        private XTask<IAudio> InnerReadyAudioAsync(string name, string groupName)
        {
            Group group = InnerGetOrNewGroup(groupName);
            XTask<IAudio> audio = InnerCreateAudioAsync(name);
            audio.OnComplete(() => group.Add(audio.Data));
            return audio;
        }

        private Audio InnerCreateAudio(string name)
        {
            AudioClip clip = ResModule.Inst.Load<AudioClip>($"{Constant.AUDIO_PATH}/{name}");
            Audio audio = m_AudioPool.Require();
            audio.OnInit(m_Root, m_MainGroup, clip);
            return audio;
        }

        private XTask<IAudio> InnerCreateAudioAsync(string name)
        {
            XTask<IAudio> task = TaskModule.Inst.GetOrNew<XTask<IAudio>>();
            ResLoadTask<AudioClip> loadTask = ResModule.Inst.LoadAsync<AudioClip>($"{Constant.AUDIO_PATH}/{name}");
            loadTask.OnComplete((clip) =>
            {
                if (clip == null)
                    return;

                Audio audio = m_AudioPool.Require();
                audio.OnInit(m_Root, m_MainGroup, clip);
                task.Data = audio;
            });
            task.Add(loadTask).Start();
            return task;
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
