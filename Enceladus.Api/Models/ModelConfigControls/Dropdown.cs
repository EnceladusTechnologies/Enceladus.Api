using Enceladus.Api.Models.Bots;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Enceladus.Api.Models.ModelConfigControls
{
    public class Dropdown : ConfigBase
    {
        public ICollection<ControlOption> Options { get; set; }
        public override ConfigBaseControlType ControlType => ConfigBaseControlType.Dropdown;
    }


    public class ControlOption
    {
        [Key]
        public Guid Key { get; set; }
        public string Value { get; set; }
        public bool IsDeleted { get; set; }
    }
}
