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

    public class CreateHandlerBuilder
    {
        public static void GenerateCreateHandler(string solutionDirectory, Entity entity)
        {
            try
            {
                var classPath = ClassPathHelper.MediatorHandlerClassPath(solutionDirectory, $"{entity.Name}.cs", entity.Name);

                if (!Directory.Exists(classPath.ClassDirectory))
                    Directory.CreateDirectory(classPath.ClassDirectory);

                if (File.Exists(classPath.FullClassPath))
                    throw new FileAlreadyExistsException(classPath.FullClassPath);

                using (FileStream fs = File.Create(classPath.FullClassPath))
                {
                    var data = GetHandlerText(classPath.ClassNamespace, entity);
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

        public static string GetHandlerText(string classNamespace, Entity entity)
        {
            var readDto = Utilities.GetDtoName(entity.Name, Dto.Read);
            var creationDto = Utilities.GetDtoName(entity.Name, Dto.Creation);
            return @$"namespace {classNamespace}
{{
    using AutoMapper;
    using Foundation.Api.Data.Entities;
    using Foundation.Api.Mediator.Commands;
    using Foundation.Api.Models;
    using Foundation.Api.Services;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Create{entity.Name}Handler : IRequestHandler<Create{entity.Name}Command, ActionResult<{entity.Name}Dto>>
    {{
        private readonly {Utilities.GetRepositoryName(entity.Name, true)} _{entity.Name.LowercaseFirstLetter()}Repository;
        private readonly IMapper _mapper;

        public Create{entity.Name}Handler({Utilities.GetRepositoryName(entity.Name, true)} {entity.Name.LowercaseFirstLetter()}Repository
            , IMapper mapper)
        {{
            _{entity.Name.LowercaseFirstLetter()}Repository = {entity.Name.LowercaseFirstLetter()}Repository ??
                throw new ArgumentNullException(nameof({entity.Name.LowercaseFirstLetter()}Repository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }}

        public async Task<ActionResult<{readDto}>> Handle(Create{entity.Name}Command create{entity.Name}Command, CancellationToken cancellationToken)
        {{
            var {entity.Name.LowercaseFirstLetter()} = _mapper.Map<{entity.Name}>(create{entity.Name}Command.{creationDto});
            _{entity.Name.LowercaseFirstLetter()}Repository.Add{entity.Name}({entity.Name.LowercaseFirstLetter()});
            _{entity.Name.LowercaseFirstLetter()}Repository.Save();

            var {entity.Name.LowercaseFirstLetter()}Dto = _mapper.Map<{readDto}>({entity.Name.LowercaseFirstLetter()});
            return create{entity.Name}Command.Controller.CreatedAtRoute(""Get{entity.Name}"",
                new {{ {entity.Name.LowercaseFirstLetter()}Dto.{entity.PrimaryKeyProperty.Name} }},
                {entity.Name.LowercaseFirstLetter()}Dto);
        }}
    }}
}}";
        }
    }
}
