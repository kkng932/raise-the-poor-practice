using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class InjectContainer
{
    static InjectContainer _Instance = new InjectContainer();
    public static InjectContainer Instance { get => _Instance; }

    Dictionary<Type, object> dic = new Dictionary<Type, object>();

    public void Regist<T>(T obj)
    {
        dic[typeof(T)] = obj;
    }

    public object Get(Type t)
    {
        return dic[t];
    }

    public void Inject<T>(T obj)
    {
        var type = typeof(T);

        //obj �ʵ��߿� Inject �� �ִ� �ʵ带 ã�Ƽ�
        //��ϵ� ���� obj�� �ʵ忡 �־��ִ��Լ�
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);

        foreach (var f in fields)
        {

            if (f.GetCustomAttribute<Inject>(false) != null)
            {
                f.SetValue(obj, Get(f.FieldType));
            }

        }
    }

    internal void Reset()
    {
        dic = new Dictionary<Type, object>();
    }
}

public class Inject : Attribute
{

}


public class InjectObj
{
    InjectContainer _InjectContainer;

    bool isInjected = false;

    public void Inject<T>(T t)
    {
        if (isInjected)
        {
            return;
        }
        isInjected = true;

        if (_InjectContainer == null)
        {
            _InjectContainer = InjectContainer.Instance;
        }

        _InjectContainer.Inject(t);

    }
}
