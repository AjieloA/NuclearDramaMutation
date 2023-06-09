public static class TypeName
{
    public class ResourcesTypeName
    {
        public const string RMaterials = "Materials/";
        public const string RPrefabs = "Prefabs/";
        public const string RUIPrefabes = "Prefabs/UI/";
        public const string RUIScripts = "Scripts/UI/";
        public const string RSceneVFX = "Prefabs/VFX/";
        public const string RMonster = "Prefabs/Monster/Monsters/";
        public const string RTurrent = "Prefabs/Turrent/";
    }

    public class SceneTypeName
    {
        public const string SceneInit = "Init";
        public const string SceneGrid = "Fieldrunners_Grid";
    }

    public class UITypeName
    {
        public class InitTypeName
        {
            public const string BGPanel = "BGPanel";
            public const string RawImage = "RawImage";
            public const string ProgressBar = "ProgressBar";
            public const string ProgressBarText = "ProgressBarText";
            public const string Buttons = "Buttons";
            public const string PlayBtn = "PlayBtn";
            public const string PlayBtnText = "PlayBtnText";
            public const string ModeBtn = "ModeBtn";
            public const string ModeBtnText = "ModeBtnText";
            public const string QuitBtn = "QuitBtn";
            public const string QuitBtnBtnText = "QuitBtnBtnText";
            public const string SettingBtn = "SettingBtn";
            public const string SettingText = "SettingText";
            public const string ProgressScrollbar = "ProgressScrollbar";
            public const string ProgressScrollbarIma = "ProgressScrollbarIma";
            public const string ProgressScrollbarArea = "ProgressScrollbarArea";
            public const string ProgressScrollbarHandle = "ProgressScrollbarHandle";
        }
    }

    public class EventTypeName
    {
        public const string CreatMonter="CREATMONSTER";
    }
    public enum NodeTypeName
    {
        Empty=0,
        Turret=1,
        Path=2,
        Point=3

    }
}