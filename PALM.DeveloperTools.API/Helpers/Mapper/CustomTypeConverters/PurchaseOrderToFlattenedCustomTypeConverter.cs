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
    internal class PurchaseOrderToFlattenedCustomTypeConverter : ITypeConverter<List<POHeaderDetails>, List<FlattenedPurchaseOrder>>
    {
        public List<FlattenedPurchaseOrder> Convert(List<POHeaderDetails> source, List<FlattenedPurchaseOrder> destination, ResolutionContext context)
        {
            // Handle potential null
            if (context == null || source is null)
                return null;

            List<FlattenedPurchaseOrder> flattenedPurchaseOrders = new();

            foreach (var poHeader in source)
            {
                // Check if the PO has lines on it
                if (poHeader.POLines.Any())
                {
                    foreach (var poLine in poHeader.POLines)
                    {
                        // Check if the PO Line has Ship details on it
                        bool hasShipDetails = poLine.POLineShipDetails is not null;

                        // Check if the PO Line has distributions on it
                        if (poLine.PODistributionDetails.Any())
                        {
                            foreach (var distLine in poLine.PODistributionDetails)
                            {
                                // All the things
                                FlattenedPurchaseOrder flattenedPurchaseOrder = new();
                                MapPOHeader(poHeader, flattenedPurchaseOrder);
                                MapPOLine(poLine, flattenedPurchaseOrder);
                                if (hasShipDetails)
#pragma warning disable CS8604 // bool is confirming this won't be null
                                    MapPOShipLine(poLine.POLineShipDetails, flattenedPurchaseOrder);
#pragma warning restore CS8604 // bool is confirming this won't be null
                                MapPODistributionLine(distLine, flattenedPurchaseOrder);
                                flattenedPurchaseOrders.Add(flattenedPurchaseOrder);
                            }
                        }
                        else
                        {
                            // No distributions; just create lines and ship (if included)
                            FlattenedPurchaseOrder flattenedPurchaseOrder = new();
                            MapPOHeader(poHeader, flattenedPurchaseOrder);
                            MapPOLine(poLine, flattenedPurchaseOrder);
                            if (hasShipDetails)
#pragma warning disable CS8604 // bool is confirming this won't be null
                                MapPOShipLine(poLine.POLineShipDetails, flattenedPurchaseOrder);
#pragma warning restore CS8604 // bool is confirming this won't be null
                            flattenedPurchaseOrders.Add(flattenedPurchaseOrder);
                        }
                    }
                }
                else
                {
                    // No lines; just create header
                    FlattenedPurchaseOrder flattenedPurchaseOrder = new();
                    MapPOHeader(poHeader, flattenedPurchaseOrder);
                    flattenedPurchaseOrders.Add(flattenedPurchaseOrder);
                }
            }


            return flattenedPurchaseOrders;
        }

        private void MapPOHeader(POHeaderDetails source, FlattenedPurchaseOrder destination)
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
        private void MapPOLine(POLineDetails source, FlattenedPurchaseOrder destination)
        {
            destination.POLineAction = source.POLineAction;
            destination.LineNumber = source.LineNumber;
            destination.CategoryCode = source.CategoryCode;
            destination.UnitOfMeasure = source.UnitOfMeasure;
            destination.AmountOnlyFlag = source.AmountOnlyFlag;
            destination.PhysicalNature = source.PhysicalNature;
            destination.ItemDescription = source.ItemDescription;
        }
        private void MapPOShipLine(POLineShipDetails source, FlattenedPurchaseOrder destination)
        {
            destination.POTotalLineAmount = source.POTotalLineAmount;
            destination.POQuantity = source.POQuantity;
        }
        private void MapPODistributionLine(PODistributionDetails source, FlattenedPurchaseOrder destination)
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
            destination.Contract = source.Contract;
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