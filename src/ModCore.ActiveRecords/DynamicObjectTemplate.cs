using ModCore.Abstraction.DataAccess;
using ModCore.Models.Objects;
using ModCore.Specifications.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.DynamicObjects

{
    public class DynamicObjectTemplate
    {

        private List<ObjectDefinition> _definitions;
        private IDataRepositoryAsync<ObjectDefinition> _objTemprepos;
        private HashSet<ObjectProperty> _properties;


        public List<ObjectProperty> Properties { get { return _properties.ToList(); } }

        public DynamicObjectTemplate(ObjectDefinition definition, IDataRepositoryAsync<ObjectDefinition> objTemprepos)
        {
            if (definition == null)
                throw new NullReferenceException("object definition");

            _objTemprepos = objTemprepos;
            _definitions = new List<ObjectDefinition>();
            _properties = new HashSet<ObjectProperty>();

            _definitions.Add(definition);

            LoadBaseTypes(definition).Wait(); //async constructors are not possible? nor a good idea?

        }

        private async Task LoadBaseTypes(ObjectDefinition objDef)
        {
            if (string.IsNullOrEmpty(objDef.InheritedName))
                return;

            var baseType = await _objTemprepos.FindAsync(new ObjectByName(objDef.InheritedName));

            foreach (var prop in baseType.Properties)
            {
                var exists = _properties.Any(a => a.Name == prop.Name);
                if (!exists)
                    _properties.Add(prop);
            }

            await LoadBaseTypes(baseType);
        }
        
    }
}
