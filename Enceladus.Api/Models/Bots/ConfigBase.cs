using System;
using System.ComponentModel.DataAnnotations;

namespace Enceladus.Api.Models.Bots
{
    public abstract class ConfigBase
    {
        [Key]
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
        public abstract ConfigBaseControlType ControlType { get; }
        public int Order { get; set; }

        public bool IsDeleted { get; set; }
        public string HelpText { get; set; }
    }

    public enum ConfigBaseControlType
    {
        Dropdown = 1,
        Number = 2,
        Percentage = 3,
        Checkbox = 4
    }
}
