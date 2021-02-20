using System;
namespace EphIt.Db.Models
{
    public partial class VMVariable
    {
        public VMVariable(Variable variable) {
            VariableId = variable.VariableId;
            Name = variable.Name;
            Value = variable.Value;
            Created = variable.Created;
            CreatedByUserId = variable.CreatedByUserId;
            Modified = variable.Modified;
            ModifiedByUserId = variable.ModifiedByUserId;
        }
        public VMVariable(){}
        public int VariableId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime Created { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedByUserId { get; set; }
    }
}
