namespace Craftsman.Builders.Repositories
{
    using Craftsman.Models;
    using System;
    using System.Collections.Generic;

    public static class RepositoryHelpers
    {
        public static string GetPrimaryRepoMethods(Entity entity, string fkIncludes)
        {
            var paramString = CreateRepoParameterString(entity);

            return @$"public async Task<{entity.Name}> Get{entity.Name}Async({paramString})
        {{
            // include marker -- requires return _context.{entity.Plural} as it's own line with no extra text -- do not delete this comment
            return await _context.{entity.Plural}{fkIncludes}
                .FirstOrDefaultAsync({entity.Lambda} => {entity.Lambda}.{entity.PrimaryKeyProperty.Name} == {entity.PrimaryKeyProperty.Name.LowercaseFirstLetter()});
        }}

        public {entity.Name} Get{entity.Name}({paramString})
        {{
            // include marker -- requires return _context.{entity.Plural} as it's own line with no extra text -- do not delete this comment
            return _context.{entity.Plural}{fkIncludes}
                .FirstOrDefault({entity.Lambda} => {entity.Lambda}.{entity.PrimaryKeyProperty.Name} == {entity.PrimaryKeyProperty.Name.LowercaseFirstLetter()});
        }}";
        }

        public static string GetCompositeRepoMethods(Entity entity, string fkIncludes)
        {
            var paramString = CreateRepoParameterString(entity);

            var paramList = new List<string>();
            foreach (var prop in entity.CompositeKeyProperties)
            {
                paramList.Add($"{entity.Lambda} => {entity.Lambda}.{prop.Name} == {prop.Name.LowercaseFirstLetter()}");
            }
            var whereString = string.Join(@$"
                    && ", paramList);

            return @$"public async Task<{entity.Name}> Get{entity.Name}Async({paramString})
        {{
            // include marker -- requires return _context.{entity.Plural} as it's own line with no extra text -- do not delete this comment
            return await _context.{entity.Plural}{fkIncludes}
                .FirstOrDefaultAsync({whereString});
        }}

        public {entity.Name} Get{entity.Name}({paramString})
        {{
            // include marker -- requires return _context.{entity.Plural} as it's own line with no extra text -- do not delete this comment
            return _context.{entity.Plural}{fkIncludes}
                .FirstOrDefault({whereString});
        }}";
        }

        public static string CreateRepoParameterString(Entity entity)
        {
            if (entity.CompositeKeyProperties.Count > 0)
            {
                var paramList = new List<string>();
                foreach (var prop in entity.CompositeKeyProperties)
                {
                    paramList.Add($"{prop.Type} {prop.Name}");
                }
                return string.Join(", ", paramList);
            }
            else
            {
                return $"{entity.PrimaryKeyProperty.Type} {entity.PrimaryKeyProperty.Name.LowercaseFirstLetter()}";
            }
        }
    }
}
