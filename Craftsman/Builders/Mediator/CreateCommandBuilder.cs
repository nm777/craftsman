namespace Craftsman.Builders.Mediator
{
    using Craftsman.Enums;
    using Craftsman.Exceptions;
    using Craftsman.Helpers;
    using Craftsman.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using static Helpers.ConsoleWriter;

    public class CreateCommandBuilder
    {
        public static void GenerateCreateCommand(string solutionDirectory, Entity entity)
        {
            try
            {
                var classPath = ClassPathHelper.MediatorCommandClassPath(solutionDirectory, $"Create{entity.Name}Command.cs", entity.Name);

                if (!Directory.Exists(classPath.ClassDirectory))
                    Directory.CreateDirectory(classPath.ClassDirectory);

                if (File.Exists(classPath.FullClassPath))
                    throw new FileAlreadyExistsException(classPath.FullClassPath);

                using (FileStream fs = File.Create(classPath.FullClassPath))
                {
                    var data = GetCommandText(classPath.ClassNamespace, entity);
                    fs.Write(Encoding.UTF8.GetBytes(data));
                }

                GlobalSingleton.AddCreatedFile(classPath.FullClassPath.Replace($"{solutionDirectory}{Path.DirectorySeparatorChar}", ""));
            }
            catch (FileAlreadyExistsException e)
            {
                WriteError(e.Message);
                throw;
            }
            catch (Exception e)
            {
                WriteError($"An unhandled exception occured when running the API command.\nThe error details are: \n{e.Message}");
                throw;
            }
        }

        public static string GetCommandText(string classNamespace, Entity entity)
        {
            var readDto = Utilities.GetDtoName(entity.Name, Dto.Read);
            var creationDto = Utilities.GetDtoName(entity.Name, Dto.Creation);
            return @$"namespace {classNamespace}
{{
    using MediatR;
    using {ClassPathHelper.DtoClassPath("", "", entity.Name).ClassNamespace};

    public class Create{entity.Name}Command : IRequest<{readDto}>
    {{
        public {creationDto} {creationDto} {{ get; set; }}

        public Create{entity.Name}Command({creationDto} {creationDto.LowercaseFirstLetter()})
        {{
            {creationDto} = {creationDto.LowercaseFirstLetter()};
        }}
    }}
}}";
        }
    }
}
