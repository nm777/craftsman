namespace Craftsman.Tests.FileTextTests
{
    using Craftsman.Builders;
    using Craftsman.Models;
    using Craftsman.Tests.Fakes;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using System.Linq;
    using AutoBogus;
    using Craftsman.Builders.Mediator;
    using System.ComponentModel.DataAnnotations;

    public class CreateHandlerFileTextTests
    {
        [Fact]
        public void GetContextFileText_passed_normal_entity_creates_expected_text()
        {
            var classNamespace = "Foundation.Api.Mediator.Handlers";
            var entity = new FakeEntity().Generate();
            entity.Name = "ValueToReplace";
            

            var fileText = CreateHandlerBuilder.GetHandlerText(classNamespace, entity);

            var expectedText = @$"namespace Foundation.Api.Mediator.Handlers
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

    public class CreateValueToReplaceHandler : IRequestHandler<CreateValueToReplaceCommand, ActionResult<ValueToReplaceDto>>
    {{
        private readonly IValueToReplaceRepository _valueToReplaceRepository;
        private readonly IMapper _mapper;

        public CreateValueToReplaceHandler(IValueToReplaceRepository valueToReplaceRepository
            , IMapper mapper)
        {{
            _valueToReplaceRepository = valueToReplaceRepository ??
                throw new ArgumentNullException(nameof(valueToReplaceRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }}

        public async Task<ActionResult<ValueToReplaceDto>> Handle(CreateValueToReplaceCommand createValueToReplaceCommand, CancellationToken cancellationToken)
        {{
            var valueToReplace = _mapper.Map<ValueToReplace>(createValueToReplaceCommand.ValueToReplaceForCreationDto);
            _valueToReplaceRepository.AddValueToReplace(valueToReplace);
            _valueToReplaceRepository.Save();

            var valueToReplaceDto = _mapper.Map<ValueToReplaceDto>(valueToReplace);
            return createValueToReplaceCommand.Controller.CreatedAtRoute(""GetValueToReplace"",
                new {{ {entity.Name.LowercaseFirstLetter()}Dto.{entity.PrimaryKeyProperty.Name} }},
                valueToReplaceDto);
        }}
    }}
}}";

            fileText.Should().Be(expectedText);
        }
    }
}
