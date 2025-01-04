using AutoMapper;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.Library.Entities.AccountsPayables.InboundVoucherLoad;

namespace PALM.BatchInterfaceTools.API.Helpers.Mapper.CustomTypeConverters
{
    public class FlattenedToAccountsPayableCustomTypeConverter : ITypeConverter<List<FlattenedVoucher>, List<VoucherHeader>>
    {
        public List<VoucherHeader> Convert(List<FlattenedVoucher> source, List<VoucherHeader> destination, ResolutionContext context)
        {
            // handle potential null
            if (context == null || source is null)
                return null;

            List<VoucherHeader> vouchers = new();

            // group by header fields
            var groupedByHeaders = source
                .GroupBy(flatAP => new
                {
                    flatAP.APBusinessUnit,
                    flatAP.VoucherStyle,
                    flatAP.VoucherOrigin,
                    flatAP.SupplierID,
                    flatAP.SupplierLocation,
                    flatAP.SupplierAddressSequenceNumber,
                    flatAP.RemitAddressSequenceNumber,
                    flatAP.InvoiceID,
                    flatAP.InvoiceDate,
                    flatAP.InvoiceReceiptDate,
                    flatAP.GoodsServicesReceivedDate,
                    flatAP.GoodsServicesApprovedDate,
                    flatAP.MatchAction,
                    flatAP.AccountingDate,
                    flatAP.GrossAmount,
                    flatAP.PaymentTerms,
                    flatAP.ConfidentialTransactionFlag,
                    flatAP.ServiceDateFrom,
                    flatAP.ServiceDateTo,
                    flatAP.SourceSystemUserId,
                    flatAP.Audit,
                    flatAP.EmergencyVoucher,
                    flatAP.FinalPayment,
                });

            foreach (var groupedAPByHeader in groupedByHeaders)
            {
                // setup header
                VoucherHeader voucherHeader = new();
                MapVoucherHeader(groupedAPByHeader.First(), voucherHeader); // using the first instance since all records in the group have the same header info
                vouchers.Add(voucherHeader);

                // skipping supplier - sensitive data; not implementing

                // setup payment
                MapVoucherPayment(groupedAPByHeader.First(), voucherHeader.VoucherPayment); // using the first instance since all records in the group have the same payment info

                // group by lines
                var groupedByLines = groupedAPByHeader.GroupBy(flatAP => new
                {
                    flatAP.VoucherLineNumber,
                    flatAP.LineDescription,
                    flatAP.Quantity,
                    flatAP.UnitofMeasure,
                    flatAP.PriceFormatted,
                    flatAP.MerchandiseAmount,
                    flatAP.RelatedVoucher,
                    flatAP.DistributionMethod,
                    flatAP.ShipToLocation,
                    flatAP.Merchant,
                    flatAP.Traveler,
                    flatAP.POBusinessUnit,
                    flatAP.POID,
                    flatAP.POLineNumber,
                    flatAP.ReceiptBusinessUnit,
                    flatAP.ReceiptID,
                    flatAP.ReceiptLineNumber,
                    flatAP.DocumentNumber,
                });

                foreach (var groupedByLine in groupedByLines)
                {
                    // Setup line
                    VoucherLine voucherLine = new();
                    MapVoucherLine(groupedByLine.First(), voucherLine); // using the first instance since all records in the group have the same line info
                    voucherHeader.VoucherLines.Add(voucherLine);

                    // Group by distribution
                    var groupedByDistributions = groupedByLine.GroupBy(flatPo => new
                    {
                        flatPo.VoucherDistributionLineNumber,
                        flatPo.DistributionLineMerchandiseAmount,
                        flatPo.DistributionLineQuantity,
                        flatPo.GLBusinessUnit,
                        flatPo.Organization,
                        flatPo.Account,
                        flatPo.Fund,
                        flatPo.BudgetEntity,
                        flatPo.Category,
                        flatPo.StateProgram,
                        flatPo.Grant,
                        flatPo.Contract,
                        flatPo.OA1,
                        flatPo.OA2,
                        flatPo.PCBusinessUnit,
                        flatPo.Project,
                        flatPo.Activity,
                        flatPo.PCSourceType,
                        flatPo.PCCategory,
                        flatPo.Subcategory,
                        flatPo.BudgetDate,
                        flatPo.PODistributionLineNumber,
                        flatPo.ReceiptDistributionLineNumber,
                        flatPo.AssetIDReference,
                        flatPo.AssetFlag,
                        flatPo.AMBusinessUnit,
                        flatPo.AssetID,
                        flatPo.ProfileID
                    });

                    foreach (var groupedByDistribution in groupedByDistributions)
                    {
                        // Should be single results by this point, at the lowest level
                        VoucherDistribution voucherDistribution = new();
                        MapVoucherDistribution(groupedByDistribution.Single(), voucherDistribution);
                        voucherLine.VoucherDistributions.Add(voucherDistribution);
                    }
                }

            }

            return vouchers;
        }
        private void MapVoucherHeader(FlattenedVoucher source, VoucherHeader destination)
        {
            destination.APBusinessUnit = source.APBusinessUnit;
            destination.VoucherStyle = source.VoucherStyle;
            destination.VoucherOrigin = source.VoucherOrigin;
            destination.SupplierID = source.SupplierID;
            destination.SupplierLocation = source.SupplierLocation;
            destination.SupplierAddressSequenceNumber = source.SupplierAddressSequenceNumber;
            destination.RemitAddressSequenceNumber = source.RemitAddressSequenceNumber;
            destination.InvoiceID = source.InvoiceID;
            destination.InvoiceDate = source.InvoiceDate;
            destination.InvoiceReceiptDate = source.InvoiceReceiptDate;
            destination.GoodsServicesReceivedDate = source.GoodsServicesReceivedDate;
            destination.GoodsServicesApprovedDate = source.GoodsServicesApprovedDate;
            destination.MatchAction = source.MatchAction;
            destination.AccountingDate = source.AccountingDate;
            destination.GrossAmount = source.GrossAmount;
            destination.PaymentTerms = source.PaymentTerms;
            destination.ConfidentialTransactionFlag = source.ConfidentialTransactionFlag;
            destination.ServiceDateFrom = source.ServiceDateFrom;
            destination.ServiceDateTo = source.ServiceDateTo;
            destination.SourceSystemUserId = source.SourceSystemUserId;
            destination.Audit = source.Audit;
            destination.FinalPayment = source.FinalPayment;
        }
        private void MapVoucherPayment(FlattenedVoucher source, VoucherPayment destination)
        {
            destination.PaymentMethod = source.PaymentMethod;
            destination.PaymentMessageCode = source.PaymentMessageCode;
            destination.PaymentHandlingCode = source.PaymentHandlingCode;
            destination.PaymentHoldFlag = source.PaymentHoldFlag;
            destination.PaymentHoldReason = source.PaymentHoldReason;
            destination.ScheduledPaymentDate = source.ScheduledPaymentDate;
            destination.SeparatePaymentFlag = source.SeparatePaymentFlag;
            destination.PayGroupCode = source.PayGroupCode;
            destination.PaymentAction = source.PaymentAction;
            destination.PaymentReferenceNumber = source.PaymentReferenceNumber;
        }
        private void MapVoucherLine(FlattenedVoucher source, VoucherLine destination)
        {
            destination.VoucherLineNumber = source.VoucherLineNumber;
            destination.LineDescription = source.LineDescription;
            destination.Quantity = source.Quantity;
            destination.UnitofMeasure = source.UnitofMeasure;
            destination.Price = source.Price;
            destination.MerchandiseAmount = source.MerchandiseAmount;
            destination.RelatedVoucher = source.RelatedVoucher;
            destination.DistributionMethod = source.DistributionMethod;
            destination.ShipToLocation = source.ShipToLocation;
            destination.Merchant = source.Merchant;
            destination.Traveler = source.Traveler;
            destination.POBusinessUnit = source.POBusinessUnit;
            destination.POID = source.POID;
            destination.POLineNumber = source.POLineNumber;
            destination.ReceiptBusinessUnit = source.ReceiptBusinessUnit;
            destination.ReceiptID = source.ReceiptID;
            destination.ReceiptLineNumber = source.ReceiptLineNumber;
            destination.DocumentNumber = source.DocumentNumber;
        }
        private void MapVoucherDistribution(FlattenedVoucher source, VoucherDistribution destination)
        {
            destination.VoucherLineNumber = source.VoucherLineNumber;
            destination.VoucherDistributionLineNumber = source.VoucherDistributionLineNumber;
            destination.DistributionLineMerchandiseAmount = source.DistributionLineMerchandiseAmount;
            destination.DistributionLineQuantity = source.DistributionLineQuantity;
            destination.GLBusinessUnit = source.GLBusinessUnit;
            destination.Organization = source.Organization;
            destination.Account = source.Account;
            destination.Fund = source.Fund;
            destination.BudgetEntity = source.BudgetEntity;
            destination.Category = source.Category;
            destination.StateProgram = source.StateProgram;
            destination.Grant = source.Grant;
            destination.Contract = source.Contract;
            destination.OA1 = source.OA1;
            destination.OA2 = source.OA2;
            destination.PCBusinessUnit = source.PCBusinessUnit;
            destination.Project = source.Project;
            destination.Activity = source.Activity;
            destination.PCSourceType = source.PCSourceType;
            destination.PCCategory = source.PCCategory;
            destination.Subcategory = source.Subcategory;
            destination.BudgetDate = source.BudgetDate;
            destination.POLineNumber = source.POLineNumber;
            destination.PODistributionLineNumber = source.PODistributionLineNumber;
            destination.ReceiptLineNumber = source.ReceiptLineNumber;
            destination.ReceiptDistributionLineNumber = source.ReceiptDistributionLineNumber;
            destination.AssetIDReference = source.AssetIDReference;
            destination.AssetFlag = source.AssetFlag;
            destination.AMBusinessUnit = source.AMBusinessUnit;
            destination.AssetID = source.AssetID;
            destination.ProfileID = source.ProfileID;
        }
    }
}
