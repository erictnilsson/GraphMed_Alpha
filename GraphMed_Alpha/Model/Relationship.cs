using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Model
{
    class Relationship : Node
    {
        public string Id { get; set; }
        public string EffectiveTime { get; set; }
        public string Active { get; set; }
        public string ModuleId { get; set; }
        public string SourceId { get; set; }
        public string DestinationId { get; set; }
        public string RelationshipGroup { get; set; }
        public string TypeId { get; set; }
        public string CharacteristicTypeId { get; set; }
        public string ModifierId { get; set; }

        public Relationship(string id, string effectiveTime, string active, string moduleId, string sourceId, string destinationId, string relationshipGroup, string typeId, string characteristicTypeId, string modiefierId)
        {
            this.Id = id;
            this.EffectiveTime = effectiveTime;
            this.Active = active;
            this.ModuleId = moduleId;
            this.SourceId = sourceId;
            this.DestinationId = destinationId;
            this.RelationshipGroup = relationshipGroup;
            this.TypeId = typeId;
            this.CharacteristicTypeId = characteristicTypeId;
            this.ModifierId = ModifierId;
        }

    }
}
