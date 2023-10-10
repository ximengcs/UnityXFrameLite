using XFrame.Core;
using XFrame.Core.Binder;
using XFrame.Modules.Local;
using XFrame.Modules.Archives;
using UnityXFrame.Core;

namespace Game.Test
{
    [CommonModule]
    public class SettingData : SingletonModule<SettingData>
    {
        private JsonArchive m_Archive;

        protected override void OnStart()
        {
            base.OnStart();
            m_Archive = Global.Archive.GetOrNew<JsonArchive>("setting");
            Lang = new ValueBinder<Language>(
                () => m_Archive.Get(nameof(Lang), Language.English),
                (v) => m_Archive.Set(nameof(Lang), v));
            Lang.AddHandler((oldLang, newLang) => Global.I18N.Lang = newLang, true);
        }

        public ValueBinder<Language> Lang { get; private set; }
    }
}
