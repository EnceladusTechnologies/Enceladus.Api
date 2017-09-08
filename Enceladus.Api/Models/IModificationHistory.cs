using System;

namespace Enceladus.Api.Models
{
    public interface IModificationHistory
    {
        string CreatedByUser { get; set; }
        DateTime DateCreated { get; set; }
        string ModifiedByUser { get; set; }
        DateTime DateModified { get; set; }
    }
}
