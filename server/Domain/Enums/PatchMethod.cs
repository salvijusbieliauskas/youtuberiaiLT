using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum PatchMethod
    {
        [Display(Name = "Add")]
        Add = 1,
        [Display(Name = "Remove")]
        Remove
    }
}
