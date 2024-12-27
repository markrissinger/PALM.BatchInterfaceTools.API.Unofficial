using PALM.InterfaceLayouts.Unofficial.Interfaces.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PALM.InterfaceLayouts.Unofficial.Constants.PurchaseOrdersConstants;

namespace PALM.DeveloperTools.API.DTO
{
    public class FlattenedPurchaseOrder : IPOHeaderDetails, IPOLineDetails, IPOLineShipDetails, IPODistributionDetails
    {
        #region Header
        public POHeaderActions POHeaderAction { get; set; }
        public string? BusinessUnit { get; set; }
        public string? POID { get; set; }
        public string? HoldStatus { get; set; }
        public DateOnly? PODate { get; set; }
        public string? POReference { get; set; }
        public string? ConfidentialPO { get; set; }
        public string? SupplierID { get; set; }
        public string? SupplierLocation { get; set; }
        public int? AddressSequenceNumber { get; set; }
        public string? POOrigin { get; set; }
        public string? ApprovalRequired { get; set; }
        public string? SourceSystemUserID { get; set; }
        #endregion

        #region Line
        public POLineActions POLineAction { get; set; }
        public int LineNumber { get; set; }
        public string? CategoryCode { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? AmountOnlyFlag { get; set; }
        public string? PhysicalNature { get; set; }
        public string? ItemDescription { get; set; }
        #endregion

        #region LineShip
        public decimal? POTotalLineAmount { get; set; }
        public decimal? POQuantity { get; set; }
        #endregion

        #region DistributionLine
        public PODistributionActions PODistributionAction { get; set; }
        public int DistributionLineNumber { get; set; }
        public decimal? DistributionPOQuantity { get; set; }
        public decimal? DistributionPercentage { get; set; }
        public decimal? DistributionLineMerchandiseAmount { get; set; }
        public string? Organization { get; set; }
        public string? Account { get; set; }
        public string? Fund { get; set; }
        public string? BudgetEntity { get; set; }
        public string? Category { get; set; }
        public string? StateProgram { get; set; }
        public string? Grant { get; set; }
        public string? OA1 { get; set; }
        public string? OA2 { get; set; }
        public string? PCBusinessUnit { get; set; }
        public string? Project { get; set; }
        public string? Activity { get; set; }
        public string? PCSourceType { get; set; }
        public string? PCCategory { get; set; }
        public string? PCSubcategory { get; set; }
        public DateOnly? BudgetDate { get; set; }
        public string? AssetProfileID { get; set; }
        #endregion
    }
}
