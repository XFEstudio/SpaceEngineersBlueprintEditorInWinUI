using ProtoBuf.Meta;
using System.Security.Cryptography;
using System.Text;
using VRage.Serialization;

namespace SpaceEngineersBlueprintEditor.SpaceEngineersCore.InnerModel;

public class DynamicTypeModel : IProtoTypeModel
{
    private RuntimeTypeModel m_typeModel;

    public TypeModel Model => m_typeModel;

    public DynamicTypeModel()
    {
        CreateTypeModel();
    }

    private void CreateTypeModel()
    {
        m_typeModel = RuntimeTypeModel.Create(setDefault: true);
        m_typeModel.AutoAddMissingTypes = true;
        m_typeModel.UseImplicitZeroDefaults = false;
    }

    private static ushort Get16BitHash(string s)
    {
        using MD5 mD = MD5.Create();
        return BitConverter.ToUInt16(mD.ComputeHash(Encoding.UTF8.GetBytes(s)), 0);
    }

    public void RegisterTypes(IEnumerable<Type> types)
    {
        HashSet<Type> registered = new HashSet<Type>();
        foreach (Type type in types)
        {
            RegisterType(type);
        }

        void RegisterType(Type protoType)
        {
            if (!protoType.IsGenericType)
            {
                if (protoType.BaseType == typeof(object) || protoType.IsValueType)
                {
                    m_typeModel.Add(protoType, applyDefaultBehaviour: true);
                }
                else
                {
                    RegisterType(protoType.BaseType);
                    if (registered.Add(protoType))
                    {
                        int fieldNumber = Get16BitHash(protoType.Name) + 65535;
                        m_typeModel.Add(protoType, applyDefaultBehaviour: true);
                        m_typeModel[protoType.BaseType].AddSubType(fieldNumber, protoType);
                    }
                }
            }
        }
    }

    public void FlushCaches()
    {
        CreateTypeModel();
    }
}