namespace Craftsman.Helpers
{
    using Craftsman.Exceptions;
    using Craftsman.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using YamlDotNet.Serialization;

    public class FileParsingHelper
    {
        public static ApiTemplate GetApiTemplateFromFile(string filePath)
        {
            var ext = Path.GetExtension(filePath);
            if (ext == ".yml" || ext == ".yaml")
                return ReadYaml(filePath);
            else
                return ReadJson(filePath);
        }

        public static ApiTemplate ReadYaml(string yamlFile)
        {
            var deserializer = new Deserializer();
            ApiTemplate templatefromYaml = deserializer.Deserialize<ApiTemplate>(File.ReadAllText(yamlFile));

            return templatefromYaml;
        }

        public static ApiTemplate ReadJson(string jsonFile)
        {
            return JsonConvert.DeserializeObject<ApiTemplate>(File.ReadAllText(jsonFile));
        }

        public static bool IsJsonOrYaml(string filePath)
        {
            var validExtensions = new string[] { ".json", ".yaml", ".yml" };
            return validExtensions.Contains(Path.GetExtension(filePath));
        }

        public static bool RunInitialTemplateParsingGuards(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            if (!IsJsonOrYaml(filePath))
            {
                //TODO: add link to docs for examples
                throw new InvalidFileTypeException();
            }

            return true;
        }

        public static void RunKeyGuard(ApiTemplate template)
        {
            if (template.Entities.Where(e => e.PrimaryKeyProperty == null).ToList().Count == 0 && template.Entities.Where(e => e.CompositeKeyProperties.Count <= 0).ToList().Count > 0)
                throw new MissingKeyException();
        }

        public static void RunSolutionNameAssignedGuard(ApiTemplate template)
        {
            if (template.SolutionName == null || template.SolutionName.Length <= 0)
                throw new InvalidSolutionNameException();
        }
    }
}
