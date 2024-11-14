using ParallelTasks;
using System.Management;
using VRage;
using VRage.Library.Threading;
using VRage.Utils;

namespace SpaceEngineersBlueprintEditor.SpaceEngineersCore.InnerModel;

internal class VRageSystemImpl : IVRageSystem
{
    public float CPUCounter
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float RAMCounter
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float GCMemory
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public long RemainingMemoryForGame
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public long ProcessPrivateMemory
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool IsUsingGeforceNow
    {
        get
        {
            return false;
        }
    }

    public bool IsScriptCompilationSupported
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string Clipboard
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public bool IsAllocationProfilingReady
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool IsSingleInstance
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool IsRemoteDebuggingSupported
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public SimulationQuality SimulationQuality
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool IsDeprecatedOS
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool IsMemoryLimited
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public bool HasSwappedMouseButtons
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string ThreeLetterISORegionName
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string TwoLetterISORegionName
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string RegionLatitude
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string RegionLongitude
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public string TempPath
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public int? OptimalHavokThreadCount
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public PrioritizedScheduler.ExplicitWorkerSetup? ExplicitWorkerSetup
    {
        get
        {
            return null;
        }
    }

    public bool AreEnterBackButtonsSwapped
    {
        get
        {
            return false;
        }
    }

    public float? ForcedUiRatio
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public event Action<string> OnSystemProtocolActivated;

    public event Action OnResuming;

    public event Action LeaveSession;

    public event Action OnSuspending;

    public string GetRootPath()
    {
        throw new NotImplementedException();
    }

    public string GetAppDataPath()
    {
        throw new NotImplementedException();
    }

    public ulong GetGlobalAllocationsStamp()
    {
        throw new NotImplementedException();
    }

    public string GetInfoCPU(out uint frequency, out uint physicalCores)
    {
        if (this.m_cpuInfo.Item1 == null)
        {
            try
            {
                using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select Name, MaxClockSpeed, NumberOfCores from Win32_Processor"))
                {
                    foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
                    {
                        ManagementObject managementObject = (ManagementObject)managementBaseObject;
                        this.m_cpuInfo.Item1 = managementObject["Name"].ToString();
                        this.m_cpuInfo.Item3 = (uint)managementObject["NumberOfCores"];
                        this.m_cpuInfo.Item2 = (uint)managementObject["MaxClockSpeed"];
                    }
                }
            }
            catch (Exception)
            {
                this.m_cpuInfo.Item1 = "UnknownCPU";
                this.m_cpuInfo.Item3 = 0U;
                this.m_cpuInfo.Item2 = 0U;
            }
        }
        frequency = this.m_cpuInfo.Item2;
        physicalCores = this.m_cpuInfo.Item3;
        return this.m_cpuInfo.Item1;
    }

    public string GetOsName()
    {
        throw new NotImplementedException();
    }

    public List<string> GetProcessesLockingFile(string path)
    {
        throw new NotImplementedException();
    }

    public ulong GetThreadAllocationStamp()
    {
        throw new NotImplementedException();
    }

    public ulong GetTotalPhysicalMemory()
    {
        throw new NotImplementedException();
    }

    public void LogEnvironmentInformation()
    {
        throw new NotImplementedException();
    }

    public void LogToExternalDebugger(string message)
    {
        throw new NotImplementedException();
    }

    public bool OpenUrl(string url)
    {
        throw new NotImplementedException();
    }

    public void ResetColdStartRegister()
    {
        throw new NotImplementedException();
    }

    public void WriteLineToConsole(string msg)
    {
        Console.WriteLine(msg);
    }

    public void GetGCMemory(out float allocated, out float used)
    {
        throw new NotImplementedException();
    }

    public void OnThreadpoolInitialized()
    {
        throw new NotImplementedException();
    }

    public void LogRuntimeInfo(Action<string> log)
    {
        throw new NotImplementedException();
    }

    public void OnSessionStarted(SessionType sessionType)
    {
        throw new NotImplementedException();
    }

    public void OnSessionUnloaded()
    {
        throw new NotImplementedException();
    }

    public int? GetExperimentalPCULimit(int safePCULimit)
    {
        throw new NotImplementedException();
    }

    public void DebuggerBreak()
    {
        throw new NotImplementedException();
    }

    public void CollectGC(int generation, GCCollectionMode mode)
    {
        generation = Math.Min(generation, GC.MaxGeneration);
        GC.Collect(generation, mode);
    }

    public void CollectGC(int generation, GCCollectionMode mode, bool blocking, bool compacting)
    {
        generation = Math.Min(generation, GC.MaxGeneration);
        GC.Collect(generation, mode, blocking, compacting);
    }

    public bool OpenUrl(string url, bool predetermined = true)
    {
        throw new NotImplementedException();
    }

    public ISharedCriticalSection CreateSharedCriticalSection(bool spinLock)
    {
        if (spinLock)
        {
            return new MyCriticalSection_SpinLock();
        }
        return new MyCriticalSection_Mutex();
    }

    public DateTime GetNetworkTimeUTC()
    {
        throw new NotImplementedException();
    }

    public string GetPlatformSpecificCrashReport()
    {
        return null;
    }

    public string GetModsCachePath()
    {
        return null;
    }


    private ValueTuple<string, uint, uint> m_cpuInfo;
}