﻿using System;
using System.Reflection;

namespace SaberTailor.Utilities
{
    public class ReflectionUtil
    {
        public static void SetPrivateField(object obj, string fieldName, object value)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(obj, value);
        }

        public static T GetPrivateField<T>(object obj, string fieldName)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            object value = field.GetValue(obj);
            return (T)value;
        }

        public static object GetPrivateField(Type type, object obj, string fieldName)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            object value = field.GetValue(obj);
            return value;
        }

        public static void InvokePrivateMethod(object obj, string methodName, object[] methodParams)
        {
            MethodInfo method = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(obj, methodParams);
        }
    }
}
