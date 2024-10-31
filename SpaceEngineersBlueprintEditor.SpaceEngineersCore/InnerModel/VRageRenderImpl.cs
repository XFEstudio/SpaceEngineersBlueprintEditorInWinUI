using VRage;
using VRageRender;

namespace SpaceEngineersBlueprintEditor.SpaceEngineersCore.InnerModel;

internal class VRageRenderImpl : IVRageRender
{
    public bool UseParallelRenderInit
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool IsRenderOutputDebugSupported
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool ForceClearGBuffer
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public event Action OnResuming;

    public event Action OnSuspending;

    public void ApplyRenderSettings(MyRenderDeviceSettings? settings)
    {
        throw new NotImplementedException();
    }

    public object CreateRenderAnnotation(object deviceContext)
    {
        throw new NotImplementedException();
    }

    public void CreateRenderDevice(ref MyRenderDeviceSettings? settings, out object deviceInstance, out object swapChain)
    {
        throw new NotImplementedException();
    }

    public void DisposeRenderDevice()
    {
        throw new NotImplementedException();
    }

    public void FlushIndirectArgsFromComputeShader(object deviceContext)
    {
        throw new NotImplementedException();
    }

    public ulong GetMemoryBudgetForStreamedResources()
    {
        return 0UL;
    }

    public ulong GetMemoryBudgetForGeneratedTextures()
    {
        return 0UL;
    }

    public ulong GetMemoryBudgetForVoxelTextureArrays()
    {
        return 0UL;
    }

    public MyAdapterInfo[] GetRenderAdapterList()
    {
        throw new NotImplementedException();
    }

    public MyRenderPresetEnum GetRenderQualityHint()
    {
        throw new NotImplementedException();
    }

    public void ResumeRenderContext()
    {
        throw new NotImplementedException();
    }

    public void SetMemoryUsedForImprovedGFX(long bytes)
    {
        throw new NotImplementedException();
    }

    public void SuspendRenderContext()
    {
        throw new NotImplementedException();
    }

    public void RequestSuspendWait()
    {
        throw new NotImplementedException();
    }

    public void CustomUpdateForDeferredBuffer(object deviceContext, object buffer)
    {
        throw new NotImplementedException();
    }

    public void SubmitEmptyCustomContext(object deviceContext)
    {
        throw new NotImplementedException();
    }

    public void FastVSSetConstantBuffer(object deviceContext, int slot, object buffer)
    {
        throw new NotImplementedException();
    }

    public void FastGSSetConstantBuffer(object deviceContext, int slot, object buffer)
    {
        throw new NotImplementedException();
    }

    public void FastPSSetConstantBuffer(object deviceContext, int slot, object buffer)
    {
        throw new NotImplementedException();
    }

    public void FastCSSetConstantBuffer(object deviceContext, int slot, object buffer)
    {
        throw new NotImplementedException();
    }

    public void FastVSSetConstantBuffers1(object deviceContext, int slot, object buffer, int offset, int size, ref object constantBindingsCache)
    {
        throw new NotImplementedException();
    }

    public void FastPSSetConstantBuffers1(object deviceContext, int slot, object buffer, int offset, int size, ref object constantBindingsCache)
    {
        throw new NotImplementedException();
    }

    public void SetDepthTextureHint(VRageRender_DepthTextureHintType hint, object deviceContext = null, object texture = null)
    {
        throw new NotImplementedException();
    }

    public bool IsExclusiveTextureLoadRequired()
    {
        throw new NotImplementedException();
    }
}