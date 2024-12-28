using PALM.InterfaceLayouts.Unofficial.Interfaces.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PALM.InterfaceLayouts.Unofficial.Constants.PurchaseOrdersConstants;

namespace PALM.DeveloperTools.API.DTO
{
    public class FlattenedPurchaseOrder : IPOHeaderDetails, IPOLineDetails, IPOLineShipDetails, IPODistributionDetails
    {
        public FlattenedPurchaseOrder() { }

        #region Header
        [Required]
        public POHeaderActions POHeaderAction { get; set; }

        [Required]
        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? BusinessUnit { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 4)]
        public string? POID { get; set; }

        [AllowedValues(["Y", "N", null])]
        public string? HoldStatus { get; set; }

        public DateOnly? PODate { get; set; }

        [StringLength(maximumLength: 30)]
        public string? POReference { get; set; }

        [AllowedValues(["Y", "N", null])]
        public string? ConfidentialPO { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string? SupplierID { get; set; }

        [StringLength(maximumLength: 10)]
        public string? SupplierLocation { get; set; }

        [Range(0,99999)]
        public int? AddressSequenceNumber { get; set; }

        [Required]
        [StringLength(maximumLength: 3)]
        public string? POOrigin { get; set; }

        [AllowedValues(["Y", "N", null])]
        public string? ApprovalRequired { get; set; }

        [StringLength(maximumLength: 30)]
        public string? SourceSystemUserID { get; set; }
        #endregion

        #region Line
        [Required]
        public POLineActions POLineAction { get; set; }

        [Required]
        [Range(0, 99999)]
        public int LineNumber { get; set; }

        [StringLength(maximumLength: 18, MinimumLength = 18)]
        public string? CategoryCode { get; set; }

        public string? UnitOfMeasure { get; set; }

        [AllowedValues(["Y", "N", null])]
        public string? AmountOnlyFlag { get; set; }

        [AllowedValues(["G", "S", null])]
        public string? PhysicalNature { get; set; }

        [StringLength(maximumLength: 254, MinimumLength = 1)]
        public string? ItemDescription { get; set; }
        #endregion

        #region LineShip
        public decimal? POTotalLineAmount { get; set; }

        public decimal? POQuantity { get; set; }
        #endregion

        #region DistributionLine
        [Required]
        public PODistributionActions PODistributionAction { get; set; }

        [Required]
        [Range(0, 99999)]
        public int DistributionLineNumber { get; set; }

        public decimal? DistributionPOQuantity { get; set; }

        public decimal? DistributionPercentage { get; set; }

        public decimal? DistributionLineMerchandiseAmount { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string? Organization { get; set; }

        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string? Account { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? Fund { get; set; }

        [StringLength(maximumLength: 8, MinimumLength = 8)]
        public string? BudgetEntity { get; set; }

        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string? Category { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string? StateProgram { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 5)]
        public string? Grant { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 5)]
        public string? OA1 { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 10)]
        public string? OA2 { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? PCBusinessUnit { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 15)]
        public string? Project { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 15)]
        public string? Activity { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 5)]
        public string? PCSourceType { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 5)]
        public string? PCCategory { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 5)]
        public string? PCSubcategory { get; set; }

        public DateOnly? BudgetDate { get; set; }

        [StringLength(maximumLength: 1, MinimumLength = 10)]
        public string? AssetProfileID { get; set; }
        #endregion
    }
}