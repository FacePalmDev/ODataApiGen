using System;
using System.Collections.Generic;
using System.Linq;
using ODataApiGen.Models;

namespace ODataApiGen.Angular
{
    public class ServiceEntity : Service
    {
        public Models.EntitySet EdmEntitySet { get; private set; }
        public ServiceEntity(EntitySet type) {
            EdmEntitySet = type;
        }
        public override IEnumerable<string> ImportTypes
        {
            get
            {
                var parameters = new List<Models.Parameter>();
                foreach (var cal in this.EdmEntitySet.Actions)
                    parameters.AddRange(cal.Parameters);
                foreach (var cal in this.EdmEntitySet.Functions)
                    parameters.AddRange(cal.Parameters);

                var list = new List<string> {
                    this.EdmEntitySet.EntityType
                };
                list.AddRange(parameters.Select(p => p.Type));
                list.AddRange(this.EdmEntitySet.Actions.SelectMany(a => this.CallableNamespaces(a)));
                list.AddRange(this.EdmEntitySet.Functions.SelectMany(a => this.CallableNamespaces(a)));
                list.AddRange(this.Interface.EdmStructuredType.Actions.SelectMany(a => this.CallableNamespaces(a)));
                list.AddRange(this.Interface.EdmStructuredType.Functions.SelectMany(a => this.CallableNamespaces(a)));
                list.AddRange(this.Interface.EdmStructuredType.Properties.Select(a => a.Type));
                if (this.Interface.EdmStructuredType is EntityType)
                    list.AddRange((this.Interface.EdmStructuredType as EntityType).NavigationProperties.Select(a => a.Type));
                return list;
            }
        }

        public override IEnumerable<Import> Imports => GetImportRecords();
        public override string EntitySetName => this.EdmEntitySet.Name;
        public override string EntityName => EdmEntitySet.EntityType.Split('.').Last();
        public override string Name => this.EdmEntitySet.Name[0].ToString().ToUpper() + this.EdmEntitySet.Name.Substring(1) + "Service";
        public override string NameSpace => this.EdmEntitySet.Namespace;
        public override string FileName => this.EdmEntitySet.Name.ToLower() + ".service";
        public override string EntityType => this.EdmEntitySet.EntityType;
        public IEnumerable<string> Actions =>  this.RenderCallables(this.EdmEntitySet.Actions.Union(this.Interface.EdmStructuredType.Actions));
        public IEnumerable<string> Functions => this.RenderCallables(this.EdmEntitySet.Functions.Union(this.Interface.EdmStructuredType.Functions));
        public IEnumerable<string> Navigations => (this.Interface.EdmStructuredType is EntityType) ? this.RenderReferences((this.Interface.EdmStructuredType as EntityType).NavigationProperties) : Enumerable.Empty<string>();
    }
}