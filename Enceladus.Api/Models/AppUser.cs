using System;

namespace Enceladus.Api.Models
{
    public class AppUser: IModificationHistory
    {
        public Guid Id { get; set; }
        public string AuthId { get; set; }
        public string Email { get; set; }

        #region IModificationHistory
        public string CreatedByUser { get; set; }
        public DateTime DateCreated { get; set; }
        public string ModifiedByUser { get; set; }
        public DateTime DateModified { get; set; }
        #endregion IModificationHistory
    }
}