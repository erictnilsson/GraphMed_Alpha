using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Model
{
    public class Concept : Node
    {
        public string Id { get; set; }
        public string EffectiveTime { get; set; }
        public string Active { get; set; }
        public string ModuleId { get; set; }
        public string DefinitionStatusId { get; set; }
        public List<Description> Descriptions { get; set; }

        public Concept() : base()
        {
            this.LinkProp = "Id";
            Descriptions = new List<Description>(); 
        }

        public Concept(string id, string effectiveTime, string active, string moduleId, string definitionStatusId)
        {
            this.Id = id;
            this.EffectiveTime = effectiveTime;
            this.Active = active;
            this.ModuleId = moduleId;
            this.DefinitionStatusId = definitionStatusId;
        }
    }
}
