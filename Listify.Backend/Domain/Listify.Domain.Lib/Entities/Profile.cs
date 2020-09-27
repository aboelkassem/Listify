using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Listify.Domain.Lib.Entities
{
    [Table("Profiles", Schema = "Listify")]
    public class Profile : BaseEntity
    {
        public string ProfileTitle { get; set; }
        public string ProfileDescription { get; set; }
        public string AvatarUrl { get; set; }

        public Guid ApplicationUserId { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
