using System;
using UnityEngine;
namespace TestFarm
{
    public interface IInventoriable
    {
        GameObject GameObject { get; }
        int GetCount();
        string GetName();
        void SetCount(int count);
        void SetContent(string name);
        void SetAction(Action<string> action, string name);
    }
}