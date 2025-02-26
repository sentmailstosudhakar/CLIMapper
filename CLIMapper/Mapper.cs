using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLIMapper
{
    /// <summary>
    /// Mapper.
    /// Parse and Maps command line arguments into C# Type.
    /// </summary>
    public sealed class Mapper
    {
        #region Public Methods
        /// <summary>
        /// Map and Map arguments and sets values into repective property which has command attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="MapperException"></exception>
        public static T Map<T>(string[] args) where T : new()
        {
            T parsedObject = Activator.CreateInstance<T>();
            if (args == null || args.Length == 0)
                return parsedObject;
            var properties = GetCommandProperties<T>();
            var commandMap = GetMappedCommand(properties);
            Logger.Log("Mapping started.");
            for (int i = 0; i < args.Length; i++)
            {
                Logger.Log($"Processing argument: {args[i]}", Logger.Severity.Debug);
                if (commandMap.TryGetValue(args[i], out PropertyInfo property))
                    ProcessCommand(parsedObject, property, args, ref i);
                else
                    Logger.Log($"Command not found: {args[i]}", Logger.Severity.Warning);
            }
            Logger.Log("Mapping completed.");
            return parsedObject;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Get Properties which has Command Attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="MapperException"></exception>
        private static IEnumerable<PropertyInfo> GetCommandProperties<T>()
        {
            var type = typeof(T);
            if (type.GetProperties().Any(propperty => propperty.GetCustomAttribute<CommandAttribute>() != null))
                return type.GetProperties().Where(property => property.GetCustomAttributes<CommandAttribute>() != null);
            throw new MapperException($"Command Attribute not found in {type} type.");
        }

        /// <summary>
        /// Creates a map for key and property.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Dictionary<string, PropertyInfo> GetMappedCommand(IEnumerable<PropertyInfo> properties)
        {
            Dictionary<string, PropertyInfo> mappedProperties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<CommandAttribute>();
                if (attribute != null)
                {
                    var commandKeys = attribute.Command.Split(MapperConstant.KeyDelimiter);
                    foreach (var commandKey in commandKeys)
                        mappedProperties.Add(commandKey, property);
                }
            }
            return mappedProperties;
        }

        /// <summary>
        /// Process Arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parsedObject"></param>
        /// <param name="property"></param>
        /// <param name="args"></param>
        /// <param name="i"></param>
        /// <exception cref="MapperException"></exception>
        private static void ProcessCommand<T>(T parsedObject, PropertyInfo property, string[] args, ref int i)
        {
            var commandAttribute = property.GetCustomAttribute<CommandAttribute>();
            if (commandAttribute.IsStandAlone)
                if (property.PropertyType == typeof(string) || property.PropertyType == typeof(bool))
                    SetValue(parsedObject, property, args[i]);
                else
                    throw new MapperException($"Stand alone property cannot be :{property.PropertyType}. Use string or bool");
            else
            {
                if (i + 1 < args.Length)
                    SetValue(parsedObject, property, args[++i]);
                else
                    Logger.Log($"No value found for {commandAttribute.Command}", Logger.Severity.Warning);
            }
            Logger.Log($"Process completed for property:{property.Name}", Logger.Severity.Debug);
        }

        /// <summary>
        /// Sets value to the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="initializdObject"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private static void SetValue<T>(T initializdObject, PropertyInfo property, string value)
        {
            Logger.Log($"Assignment started {property.PropertyType} {property.Name} = {value}", Logger.Severity.Debug);
            if (property.PropertyType == typeof(bool))
                property.SetValue(initializdObject, true);
            else
            {
                var propertyValue = property.GetValue(initializdObject);
                if (propertyValue is ICollection)
                {
                    if (propertyValue.GetType().IsArray)
                    {
                        Array oldArray = (Array)propertyValue;
                        Type elementType = oldArray.GetType().GetElementType();
                        Array newArray = Array.CreateInstance(elementType, oldArray.Length + 1);
                        Array.Copy(oldArray, newArray, oldArray.Length);
                        newArray.SetValue(Convert.ChangeType(value, elementType, MapperConstant.formatProvider), oldArray.Length);
                        property.SetValue(initializdObject, newArray);
                    }
                    else if (propertyValue is IList list)
                        list.Add(value);
                    else
                        throw new MapperException($"Unsupported collection :{property.PropertyType}");
                }
                else
                    property.SetValue(initializdObject, Convert.ChangeType(value, property.PropertyType, MapperConstant.formatProvider));
            }
            Logger.Log("Assignment completed.", Logger.Severity.Debug);
        }

        #endregion
    }
}