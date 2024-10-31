using VRage;
using VRage.Analytics;
using VRage.Audio;
using VRage.Http;
using VRage.Input;
using VRage.Scripting;
using VRage.Serialization;

namespace SpaceEngineersBlueprintEditor.SpaceEngineersCore.InnerModel;

public class CorePlatform : IVRagePlatform
{
    public bool SessionReady { get; set; }

    public IVRageWindows Windows
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IVRageHttp Http
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IVRageSystem System { get; } = new VRageSystemImpl();

    public IVRageRender Render { get; } = new VRageRenderImpl();

    public IAnsel Ansel
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IAfterMath AfterMath
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IVRageInput Input
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IVRageInput2 Input2
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IMyAudio Audio
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IMyImeProcessor ImeProcessor
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IMyCrashReporting CrashReporting
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public IVRageScripting Scripting
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public void Init()
    {
        this.typeModel = new DynamicTypeModel();
    }

    public bool CreateInput2()
    {
        throw new NotImplementedException();
    }

    public IVideoPlayer CreateVideoPlayer()
    {
        throw new NotImplementedException();
    }

    public void Done()
    {
        throw new NotImplementedException();
    }

    public IProtoTypeModel GetTypeModel()
    {
        return this.typeModel;
    }

    public IMyAnalytics InitAnalytics(string projectId, string version)
    {
        throw new NotImplementedException();
    }

    public IMyAnalytics InitAnalytics(string projectId, string version, bool idInited)
    {
        throw new NotImplementedException();
    }

    public void InitScripting(IVRageScripting scripting)
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }

    private IProtoTypeModel typeModel;
}
