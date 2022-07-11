using LitJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARPGCommon.Tools
{
    public class ParameterTool
    {
        public static T GetParameter<T>(Dictionary<byte, object> parameters, ParameterCode parameterCode, bool isObject = true) 
        {
            object o = null;
            parameters.TryGetValue((byte)parameterCode, out o);
            if(isObject == false)
            {
                return (T)o;
            }
            return JsonMapper.ToObject<T>(o.ToString());
        }

        public static void AddParameter<T>(Dictionary<Byte, object> parameters, ParameterCode key, T value, bool isObject = true)
        {
            if (isObject)
            {
                string json = JsonMapper.ToJson(value);
                parameters.Add((byte)key, json);
            }
            else
            {
                parameters.Add((byte)key, value);
            }
        }

        public static SubCode GetSubCode(Dictionary<byte, object> parameters)
        {
            return GetParameter<SubCode>(parameters, ParameterCode.SubCode, false);
        }

        public static void AddSubCode(Dictionary<byte, object> parameters, SubCode subCode)
        {
            AddParameter<SubCode>(parameters, ParameterCode.SubCode, subCode, false);
        }

        public static void AddOperationcodeSubcodeRoleID(Dictionary<byte, object> parameters, OperationCode opCode, int roleID)
        {
            if (parameters.ContainsKey((byte)ParameterCode.OperationCode))
            {
                parameters.Remove((byte)ParameterCode.OperationCode);
            }
            if (parameters.ContainsKey((byte)ParameterCode.RoleID))
            {
                parameters.Remove((byte)ParameterCode.RoleID);
            }
            parameters.Add((byte)ParameterCode.OperationCode, opCode);
            parameters.Add((byte)ParameterCode.RoleID, roleID);
        }
    }
}
