using XFrame.Core;
using XFrame.Core.Binder;
using XFrame.Modules.Local;
using XFrame.Modules.Archives;

namespace Game.Test
{
    [XModule]
    public class SettingData : SingletonModule<SettingData>
    {
        private JsonArchive m_Archive;

        protected override void OnStart()
        {
            base.OnStart();
            m_Archive = ArchiveModule.Inst.GetOrNew<JsonArchive>("setting");
            Lang = new ValueBinder<Language>(
                () => m_Archive.Get(nameof(Lang), Language.English),
                (v) => m_Archive.Set(nameof(Lang), v));
            Lang.AddHandler((oldLang, newLang) => LocalizeModule.Inst.Lang = newLang, true);
        }

        public ValueBinder<Language> Lang { get; private set; }
    }
}
