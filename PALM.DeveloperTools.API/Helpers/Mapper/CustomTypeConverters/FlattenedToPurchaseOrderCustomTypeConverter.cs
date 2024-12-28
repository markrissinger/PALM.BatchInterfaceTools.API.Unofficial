using AutoMapper;
using PALM.DeveloperTools.API.DTO;
using PALM.InterfaceLayouts.Unofficial.Entities.PurchaseOrders.InboundEncumbranceLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALM.DeveloperTools.API.Helpers.Mapper.CustomTypeConverters
{
    internal class FlattenedToPurchaseOrderCustomTypeConverter : ITypeConverter<List<FlattenedPurchaseOrder>, List<POHeaderDetails>>
    {
        public List<POHeaderDetails> Convert(List<FlattenedPurchaseOrder> source, List<POHeaderDetails> destination, ResolutionContext context)
        {
            // Handle potential null
            if (context == null || source is null)
                return null;

            List<POHeaderDetails> purchaseOrders = new();

            // Group by header fields
            var groupedPOsByHeaders = source
                .GroupBy(flatPo => new
                {
                    flatPo.POHeaderAction,
                    flatPo.BusinessUnit,
                    flatPo.POID,
                    flatPo.HoldStatus,
                    flatPo.PODate,
                    flatPo.POReference,
                    flatPo.ConfidentialPO,
                    flatPo.SupplierID,
                    flatPo.SupplierLocation,
                    flatPo.AddressSequenceNumber,
                    flatPo.POOrigin,
                    flatPo.ApprovalRequired,
                    flatPo.SourceSystemUserID
                });

            foreach (var groupedPOByHeader in groupedPOsByHeaders)
            {
                // Setup header
                POHeaderDetails poHeader = new POHeaderDetails();
                MapPOHeader(groupedPOByHeader.First(), poHeader); // using the first instance since all records in the group have the same header info
                purchaseOrders.Add(poHeader);

                // Group by lines
                var groupedPOsByLines = groupedPOByHeader.GroupBy(flatPo => new
                {
                    flatPo.POLineAction,
                    flatPo.LineNumber,
                    flatPo.CategoryCode,
                    flatPo.UnitOfMeasure,
                    flatPo.AmountOnlyFlag,
                    flatPo.PhysicalNature,
                    flatPo.ItemDescription,

                    flatPo.POTotalLineAmount,
                    flatPo.POQuantity,
                });

                foreach (var groupedPOByLine in groupedPOsByLines)
                {
                    // Setup line
                    POLineDetails poLineDetails = new POLineDetails();
                    MapPOLine(groupedPOByLine.First(), poLineDetails); // using the first instance since all records in the group have the same line info
                    poHeader.POLines.Add(poLineDetails);

                    // Ship line (if needed)
                    if (groupedPOByLine.Key.POQuantity.HasValue || groupedPOByLine.Key.POTotalLineAmount.HasValue)
                    {
                        POLineShipDetails poShipDetails = new POLineShipDetails();
                        MapPOShipLine(groupedPOByLine.First(), poShipDetails);
                        poLineDetails.POLineShipDetails = poShipDetails;
                    }

                    // Group by distribution line
                    var groupedPOsByDistLines = groupedPOByLine.GroupBy(flatPo => new
                    {
                        flatPo.PODistributionAction,
                        flatPo.DistributionLineNumber,
                        flatPo.DistributionPOQuantity,
                        flatPo.DistributionPercentage,
                        flatPo.DistributionLineMerchandiseAmount,
                        flatPo.Organization,
                        flatPo.Account,
                        flatPo.Fund,
                        flatPo.BudgetEntity,
                        flatPo.Category,
                        flatPo.StateProgram,
                        flatPo.Grant,
                        flatPo.OA1,
                        flatPo.OA2,
                        flatPo.PCBusinessUnit,
                        flatPo.Project,
                        flatPo.Activity,
                        flatPo.PCSourceType,
                        flatPo.PCCategory,
                        flatPo.PCSubcategory,
                        flatPo.BudgetDate,
                        flatPo.AssetProfileID
                    });

                    foreach (var groupedPOByDistLine in groupedPOsByDistLines)
                    {
                        // Should be single results by this point, at the lowest level
                        PODistributionDetails poDistributionDetails = new();
                        MapPODistributionLine(groupedPOByDistLine.Single(), poDistributionDetails);
                        poLineDetails.PODistributionDetails.Add(poDistributionDetails);
                    }
                }

            }

            return purchaseOrders;
        }

        private void MapPOHeader(FlattenedPurchaseOrder source, POHeaderDetails destination)
        {
            destination.POHeaderAction = source.POHeaderAction;
            destination.BusinessUnit = source.BusinessUnit;
            destination.POID = source.POID;
            destination.HoldStatus = source.HoldStatus;
            destination.PODate = source.PODate;
            destination.POReference = source.POReference;
            destination.ConfidentialPO = source.ConfidentialPO;
            destination.SupplierID = source.SupplierID;
            destination.SupplierLocation = source.SupplierLocation;
            destination.AddressSequenceNumber = source.AddressSequenceNumber;
            destination.POOrigin = source.POOrigin;
            destination.ApprovalRequired = source.ApprovalRequired;
            destination.SourceSystemUserID = source.SourceSystemUserID;
        }
        private void MapPOLine(FlattenedPurchaseOrder source, POLineDetails destination)
        {
            destination.POLineAction = source.POLineAction;
            destination.LineNumber = source.LineNumber;
            destination.CategoryCode = source.CategoryCode;
            destination.UnitOfMeasure = source.UnitOfMeasure;
            destination.AmountOnlyFlag = source.AmountOnlyFlag;
            destination.PhysicalNature = source.PhysicalNature;
            destination.ItemDescription = source.ItemDescription;
        }
        private void MapPOShipLine(FlattenedPurchaseOrder source, POLineShipDetails destination)
        {
            destination.POTotalLineAmount = source.POTotalLineAmount;
            destination.POQuantity = source.POQuantity;
        }
        private void MapPODistributionLine(FlattenedPurchaseOrder source, PODistributionDetails destination)
        {
            destination.PODistributionAction = source.PODistributionAction;
            destination.DistributionLineNumber = source.DistributionLineNumber;
            destination.DistributionPOQuantity = source.DistributionPOQuantity;
            destination.DistributionPercentage = source.DistributionPercentage;
            destination.DistributionLineMerchandiseAmount = source.DistributionLineMerchandiseAmount;
            destination.Organization = source.Organization;
            destination.Account = source.Account;
            destination.Fund = source.Fund;
            destination.BudgetEntity = source.BudgetEntity;
            destination.Category = source.Category;
            destination.StateProgram = source.StateProgram;
            destination.Grant = source.Grant;
            destination.OA1 = source.OA1;
            destination.OA2 = source.OA2;
            destination.PCBusinessUnit = source.PCBusinessUnit;
            destination.Project = source.Project;
            destination.Activity = source.Activity;
            destination.PCSourceType = source.PCSourceType;
            destination.PCCategory = source.PCCategory;
            destination.PCSubcategory = source.PCSubcategory;
            destination.BudgetDate = source.BudgetDate;
            destination.AssetProfileID = source.AssetProfileID;
        }
    }
}