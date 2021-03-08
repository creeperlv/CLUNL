using System;
using System.Collections.Generic;

namespace CLUNL.Scripting
{
    public class ScriptEnvironment : IDisposable
    {
        public Dictionary<string, object> ExposedObjects = new Dictionary<string, object>();
        public Dictionary<string, Type> ExposedTypes = new Dictionary<string, Type>();
        public ScriptEnvironment()
        {
            Expose("null", (object)null);
            Expose("Null", (object)null);
            Expose("NULL", (object)null);
        }
        public void Expose(string name, object Target)
        {
            if (ExposedObjects.ContainsKey(name))
            {
                ExposedObjects[name] = Target;
            }
            else ExposedObjects.Add(name, Target);
        }
        public void Expose(string name, Type t)
        {
            if (ExposedTypes.ContainsKey(name))
            {
                ExposedTypes[name] = t;
            }
            else ExposedTypes.Add(name, t);
        }
        public void EnclosureObject(string name)
        {
            if (ExposedObjects.ContainsKey(name))
                ExposedObjects.Remove(name);
        }
        public void EnclosureType(string name)
        {
            if (ExposedTypes.ContainsKey(name))
                ExposedTypes.Remove(name);
        }
        public void Dispose()
        {
            ExposedObjects = null;
        }
        public ScriptEnvironment HardCopy()
        {
            ScriptEnvironment environment = new ScriptEnvironment();
            foreach (var item in ExposedObjects)
            {
                if (!environment.ExposedObjects.ContainsKey(item.Key)) //Ignore pre-exposed objects.
                    environment.ExposedObjects.Add(item.Key, item.Value);
            }
            foreach (var item in ExposedTypes)
            {
                if (!environment.ExposedTypes.ContainsKey(item.Key))//Ignore pre-exposed types.
                    environment.ExposedTypes.Add(item.Key, item.Value);
            }
            return environment;
        }
    }
}
