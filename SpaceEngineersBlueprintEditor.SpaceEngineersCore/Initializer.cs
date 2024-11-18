using log4net.Config;
using ParallelTasks;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using SpaceEngineers.Game;
using SpaceEngineersBlueprintEditor.SpaceEngineersCore.InnerModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineersBlueprintEditor.SpaceEngineersCore;

public class Initializer
{
    public static bool IsDefinitionsLoadComplete { get; private set; }
    private static object GetUninitializedObject(Type type)
    {
        return FormatterServices.GetUninitializedObject(type);
    }
    public static void Initialize(string gameContentPath, string userDataPath)
    {
        XmlConfigurator.Configure();
        MyFileSystem.ExePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(FastResourceLock))!.Location);
        MyLog.Default = MySandboxGame.Log;
        SpaceEngineersGame.SetupBasicGameInfo();
        _ = new MyCommonProgramStartup([]);
        MyFileSystem.Reset();
        MyFileSystem.Init(gameContentPath, userDataPath);
        MyVRage.Init(new CorePlatform());
        MyVRage.Platform.Init();
        MySandboxGame.Config = new MyConfig("SpaceEngineers.cfg");
        MySandboxGame.Config.Load();
        SpaceEngineersGame.SetupPerGameSettings();
        MyPerGameSettings.UpdateOrchestratorType = null;
        InitMultithreading();
        MyRenderProxy.Initialize(new MyNullRender());
        InitSandboxGame();
        MySession obj = (MySession)GetUninitializedObject(typeof(MySession));
        ConstructField(obj, "CreativeTools");
        ConstructField(obj, "m_sessionComponents");
        ConstructField(obj, "m_sessionComponentsForUpdate");
        obj.Settings = new MyObjectBuilder_SessionSettings
        {
            EnableVoxelDestruction = true
        };
        MySession.Static = obj;
        var myHeightMapLoadingSystem = new MyHeightMapLoadingSystem();
        obj.RegisterComponent(myHeightMapLoadingSystem, myHeightMapLoadingSystem.UpdateOrder, myHeightMapLoadingSystem.Priority);
        myHeightMapLoadingSystem.LoadData();
        var myPlanets = new MyPlanets();
        obj.RegisterComponent(myPlanets, myHeightMapLoadingSystem.UpdateOrder, myHeightMapLoadingSystem.Priority);
        myPlanets.LoadData();
        MyDefinitionManager.Static.PreloadDefinitions();
        MyDefinitionManager.Static.LoadData([]);
        MyTexts.Clear();
        var culture = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag.Split(['-'], StringSplitOptions.RemoveEmptyEntries);
        var language = culture.Length > 0 ? culture[0] : null;
        var country = culture.Length > 1 ? culture[1] : null;
        MyTexts.LoadTexts(@$"{gameContentPath}\Data\Localization", language, country);
        IsDefinitionsLoadComplete = true;
    }

    private static void Preallocate()
    {
        ForceStaticCtor(
        [
            typeof(MyEntities),
            typeof(MyObjectBuilder_Base),
            typeof(MyMath)
        ]);
        static void ForceStaticCtor(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            }
        }
    }
    private static void InitMultithreading()
    {
        ParallelTasks.Parallel.Scheduler = new PrioritizedScheduler(Math.Max(Environment.ProcessorCount / 2, 1), amd: true, null);
    }

    private static void InitSandboxGame()
    {
        MySandboxGame.Static = (MySandboxGame)GetUninitializedObject(typeof(MySandboxGame));
        MySandboxGame @static = MySandboxGame.Static;
        object value = Activator.CreateInstance(typeof(MyConcurrentQueue<>).MakeGenericType(typeof(MySandboxGame).GetNestedType("MyInvokeData", BindingFlags.NonPublic)!), 32)!;
        object value2 = Activator.CreateInstance(typeof(MyConcurrentQueue<>).MakeGenericType(typeof(MySandboxGame).GetNestedType("MyInvokeData", BindingFlags.NonPublic)!), 32)!;
        typeof(MySandboxGame).GetField("m_invokeQueue", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(@static, value);
        typeof(MySandboxGame).GetField("m_invokeQueueExecuting", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(@static, value2);
        RegisterAssemblies();
        MyGlobalTypeMetadata.Static.Init();
        Preallocate();
    }

    private static void RegisterAssemblies()
    {
        MyPlugins.RegisterGameAssemblyFile(MyPerGameSettings.GameModAssembly);
        if (MyPerGameSettings.GameModBaseObjBuildersAssembly != null)
        {
            MyPlugins.RegisterBaseGameObjectBuildersAssemblyFile(MyPerGameSettings.GameModBaseObjBuildersAssembly);
        }

        MyPlugins.RegisterGameObjectBuildersAssemblyFile(MyPerGameSettings.GameModObjBuildersAssembly);
        MyPlugins.RegisterSandboxAssemblyFile(MyPerGameSettings.SandboxAssembly);
        MyPlugins.RegisterSandboxGameAssemblyFile(MyPerGameSettings.SandboxGameAssembly);
        MyPlugins.Load();
    }

    private static string GetBackingFieldName(string propertyName)
    {
        return $"<{propertyName}>k__BackingField";
    }
    private static FieldInfo GetFieldInfo(Type type, string fieldName)
    {
        FieldInfo field;
        do
        {
            field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!;
            if (field == null)
            {
                field = type.GetField(GetBackingFieldName(fieldName), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!;
            }
            type = type.BaseType!;
        }
        while (field == null && type != null);
        return field!;
    }
    public static void SetFieldValue(Type type, string fieldName, object val)
    {
        FieldInfo fieldInfo = GetFieldInfo(type, fieldName) ?? throw new ArgumentOutOfRangeException(nameof(fieldName), "Couldn't find field " + fieldName + " in type " + type.FullName);
        fieldInfo.SetValue(type, val);
    }
    public static object CreateInstance(Type type)
    {
        IEnumerable<object> source = from parameter in type.GetConstructors().First().GetParameters()
                                     select CreateInstance(parameter.ParameterType);
        return Activator.CreateInstance(type, source.ToArray<object>())!;
    }
    public static void ConstructField(object obj, string fieldName)
    {
        if (obj == null)
        {
            return;
        }
        FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)!;
        if (field == null)
        {
            return;
        }
        object value = CreateInstance(field.FieldType);
        field.SetValue(obj, value);
    }
}
