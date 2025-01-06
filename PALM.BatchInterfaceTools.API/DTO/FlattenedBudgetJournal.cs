using PALM.BatchInterfaceTools.Library.Constants;
using PALM.BatchInterfaceTools.Library.Interfaces.CommitmentControl;
using System.ComponentModel.DataAnnotations;

namespace PALM.BatchInterfaceTools.API.DTO
{
    public class FlattenedBudgetJournal : IKKBudgetHeader, IKKBudgetLine
    {
        #region Budget Header
        [Required]
        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? BusinessUnit { get; set; }

        [Required]
        public DateOnly JournalDate { get; set; }

        [StringLength(maximumLength: 254)]
        public string? LongDescription { get; set; }

        [Required]
        public CommitmentControlConstants.LedgerGroupValues LedgerGroup { get; set; }

        [Required]
        [StringLength(maximumLength: 1)]
        public string? BudgetEntryType { get; set; }

        [StringLength(maximumLength: 150)]
        public string? AlternateDescription { get; set; }
        #endregion

        #region Budget Line
        [Required]
        [Range(0, 99999)]
        public int JournalLineNumber { get; set; }

        [Required]
        public DateOnly BudgetPeriod { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string? Organization { get; set; }

        [Required]
        [StringLength(maximumLength: 6, MinimumLength = 1)]
        public string? Account { get; set; }

        [Required]
        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? Fund { get; set; }

        [Required]
        [StringLength(maximumLength: 8, MinimumLength = 8)]
        public string? BudgetEntity { get; set; }

        [Required]
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string? Category { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string? StateProgram { get; set; }

        [StringLength(maximumLength: 5)]
        public string? Grant { get; set; }

        [StringLength(maximumLength: 10)]
        public string? Contract { get; set; }

        [StringLength(maximumLength: 5)]
        public string? OA1 { get; set; }

        [StringLength(maximumLength: 10)]
        public string? OA2 { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? PCBusinessUnit { get; set; }

        [StringLength(maximumLength: 15)]
        public string? Project { get; set; }

        [StringLength(maximumLength: 15)]
        public string? Activity { get; set; }

        [StringLength(maximumLength: 5)]
        public string? PCSourceType { get; set; }

        [Required]
        public decimal MonetaryAmount { get; set; }

        public CommitmentControlConstants.JournalClassValues? JournalClass { get; set; }

        [StringLength(maximumLength: 10)]
        public string? JournalLineReference { get; set; }

        [StringLength(maximumLength: 30)]
        public string? JournalLineReferenceDescription { get; set; }
        #endregion
    }
}
