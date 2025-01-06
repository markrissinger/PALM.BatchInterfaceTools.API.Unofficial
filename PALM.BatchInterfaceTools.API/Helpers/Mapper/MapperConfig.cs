using AutoMapper;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.API.Helpers.Mapper.CustomTypeConverters;
using PALM.BatchInterfaceTools.Library.Entities.AccountsPayables.InboundVoucherLoad;
using PALM.BatchInterfaceTools.Library.Entities.CommitmentControl.InboundBudgetJournal;
using PALM.BatchInterfaceTools.Library.Entities.PurchaseOrders.InboundEncumbranceLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PALM.BatchInterfaceTools.API.Helpers.Mappers
{
    internal static class MapperConfig
    {
        //internal static void MapperConfiguration(this IMapperConfigurationExpression config)
        internal static void MapperConfiguration(this IMapperConfigurationExpression config)
        {
            #region Purchase Orders
            // Purchase Order domain object --> DTO
            config.CreateMap<List<POHeaderDetails>, List<FlattenedPurchaseOrder>>().ConvertUsing(new PurchaseOrderToFlattenedCustomTypeConverter());

            // Purchase Order DTO --> domain object
            config.CreateMap<List<FlattenedPurchaseOrder>, List<POHeaderDetails>>().ConvertUsing(new FlattenedToPurchaseOrderCustomTypeConverter());
            #endregion

            #region Accounts Payable
            // Accounts Payable DTO --> domain object
            config.CreateMap<List<FlattenedVoucher>, List<VoucherHeader>>().ConvertUsing(new FlattenedToAccountsPayableCustomTypeConverter());
            #endregion

            #region Commitment Control
            // Budget Journal DTO --> domain object
            //config.CreateMap<FlattenedBudgetJournal, KKBudgetHeader>();
            //config.CreateMap<FlattenedBudgetJournal, KKBudgetLine>();
            config.CreateMap<List<FlattenedBudgetJournal>, List<KKBudgetHeader>>().ConvertUsing(new FlattenedToBudgetHeader());

            // Budget Journal domain object --> DTO
            //config.CreateMap<KKBudgetHeader, FlattenedBudgetJournal>();
            //config.CreateMap<KKBudgetLine, FlattenedBudgetJournal>();
            config.CreateMap<List<KKBudgetHeader>, List<FlattenedBudgetJournal>>().ConvertUsing(new BudgetHeaderToFlattenedBudgetJournal());
            #endregion

            // Check Mappings
            var mapConfig = new MapperConfiguration((MapperConfigurationExpression)config);
            mapConfig.AssertConfigurationIsValid();
        }
    }
}
