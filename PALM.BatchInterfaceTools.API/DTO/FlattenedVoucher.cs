using static PALM.BatchInterfaceTools.Library.Constants.AccountsPayableConstants;
using System.ComponentModel.DataAnnotations;
using PALM.BatchInterfaceTools.Library.Interfaces.AccountsPayable;

namespace PALM.BatchInterfaceTools.API.DTO
{
    public class FlattenedVoucher : IVoucherHeader, IVoucherPayment, IVoucherLine, IVoucherDistribution //IVoucherSupplier
    {
        public FlattenedVoucher() { }

        #region VoucherHeader
        [Required]
        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? APBusinessUnit { get; set; }

        [Required]
        public VoucherStyleValues VoucherStyle { get; set; }

        [Required]
        public string? VoucherOrigin { get; set; }

        [Required]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string? SupplierID { get; set; }

        [StringLength(maximumLength: 10)]
        public string? SupplierLocation { get; set; }

        [Range(0, 99999)]
        public int? SupplierAddressSequenceNumber { get; set; }

        [Range(0, 99999)]
        public int? RemitAddressSequenceNumber { get; set; }

        [Required]
        [StringLength(maximumLength: 30)]
        public string? InvoiceID { get; set; }

        [Required]
        public DateOnly? InvoiceDate { get; set; }

        [Required]
        public DateOnly? InvoiceReceiptDate { get; set; }

        [Required]
        public DateOnly? GoodsServicesReceivedDate { get; set; }

        [Required]
        public DateOnly? GoodsServicesApprovedDate { get; set; }

        [Required]
        public MatchActionValues MatchAction { get; set; }

        [Required]
        public DateOnly? AccountingDate { get; set; }

        public decimal? GrossAmount { get; set; }

        [Required]
        [StringLength(maximumLength: 5)]
        public string? PaymentTerms { get; set; }

        [Required]
        public ConfidentialTransactionFlagValues ConfidentialTransactionFlag { get; set; }

        public DateOnly? ServiceDateFrom { get; set; }

        public DateOnly? ServiceDateTo { get; set; }

        [StringLength(maximumLength: 100)]
        public string? SourceSystemUserId { get; set; }

        public AuditValues? Audit { get; set; }

        public EmergencyVoucherValues? EmergencyVoucher { get; set; }

        public FinalPaymentValues? FinalPayment { get; set; }
        #endregion

        #region VoucherPayment
        public PaymentMethodValues? PaymentMethod { get; set; }

        [StringLength(maximumLength: 6)]
        public string? PaymentMessageCode { get; set; }

        [Required]
        [StringLength(maximumLength: 2)]
        public string? PaymentHandlingCode { get; set; }

        public PaymentHoldFlagValues? PaymentHoldFlag { get; set; }
        internal string? PaymentHoldFlagFormatted { get { return PaymentHoldFlag.HasValue ? Enum.GetName(typeof(PaymentHoldFlagValues), PaymentHoldFlag) : string.Empty; } }

        [StringLength(maximumLength: 3)]
        public string? PaymentHoldReason { get; set; }

        public DateOnly? ScheduledPaymentDate { get; set; }

        public SeparatePaymentFlagValues? SeparatePaymentFlag { get; set; }

        [StringLength(maximumLength: 2)]
        public string? PayGroupCode { get; set; }

        [Required]
        public PaymentActionValues PaymentAction { get; set; }

        [StringLength(maximumLength: 20)]
        public string? PaymentReferenceNumber { get; set; }
        #endregion

        #region VoucherLine        
        [Required]
        [Range(0, 99999)]
        public int VoucherLineNumber { get; set; }

        [StringLength(maximumLength: 254)]
        public string? LineDescription { get; set; }

        public decimal? Quantity { get; set; }

        [StringLength(maximumLength: 3)]
        public string? UnitofMeasure { get; set; }

        public decimal? Price { get; set; }
        internal string? PriceFormatted { get { return Price?.ToString("0.00"); } }

        public decimal? MerchandiseAmount { get; set; }

        [StringLength(maximumLength: 8)]
        public string? RelatedVoucher { get; set; }

        [Required]
        public DistributionMethodValues DistributionMethod { get; set; }

        [StringLength(maximumLength: 10)]
        public string? ShipToLocation { get; set; }

        [StringLength(maximumLength: 100)]
        public string? Merchant { get; set; }

        [StringLength(maximumLength: 100)]
        public string? Traveler { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? POBusinessUnit { get; set; }

        [StringLength(maximumLength: 10)]
        public string? POID { get; set; }

        [Range(0, 99999)]
        public int? POLineNumber { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? ReceiptBusinessUnit { get; set; }

        [StringLength(maximumLength: 10)]
        public string? ReceiptID { get; set; }

        [Range(0, 99999)]
        public int? ReceiptLineNumber { get; set; }

        [StringLength(maximumLength: 22)]
        public string? DocumentNumber { get; set; }

        #endregion

        #region VoucherDistribution
        [Required]
        [Range(0, 99999)]
        public int VoucherDistributionLineNumber { get; set; }

        [Required]
        public decimal? DistributionLineMerchandiseAmount { get; set; }

        public decimal? DistributionLineQuantity { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? GLBusinessUnit { get; set; }

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

        [StringLength(maximumLength: 5, MinimumLength = 1)]
        public string? Grant { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 1)]
        public string? Contract { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 1)]
        public string? OA1 { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 1)]
        public string? OA2 { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? PCBusinessUnit { get; set; }

        [StringLength(maximumLength: 15, MinimumLength = 1)]
        public string? Project { get; set; }

        [StringLength(maximumLength: 15, MinimumLength = 1)]
        public string? Activity { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 1)]
        public string? PCSourceType { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 1)]
        public string? PCCategory { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 1)]
        public string? Subcategory { get; set; }

        [Required]
        public DateOnly? BudgetDate { get; set; }

        [Range(0, 99999)]
        public int? PODistributionLineNumber { get; set; }

        [Range(0, 99999)]
        public int? ReceiptDistributionLineNumber { get; set; }

        [StringLength(maximumLength: 100)]
        public string? AssetIDReference { get; set; }

        public AssetFlagValues? AssetFlag { get; set; }

        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string? AMBusinessUnit { get; set; }

        [StringLength(maximumLength: 12)]
        public string? AssetID { get; set; }

        [StringLength(maximumLength: 10)]
        public string? ProfileID { get; set; }
        #endregion
    }
}
