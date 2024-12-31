using AutoMapper;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.API.Helpers.Mapper.CustomTypeConverters;
using PALM.InterfaceLayouts.Unofficial.Entities.PurchaseOrders.InboundEncumbranceLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALM.BatchInterfaceTools.API.Helpers.Mappers
{
    internal static class MapperConfig
    {
        internal static void MapperConfiguration(this IMapperConfigurationExpression config)
        {
            #region Purchase Orders
            // Purchase Order domain object --> DTO
            config.CreateMap<List<POHeaderDetails>, List<FlattenedPurchaseOrder>>().ConvertUsing(new PurchaseOrderToFlattenedCustomTypeConverter());

            // Purchase Order DTO --> domain object
            config.CreateMap<List<FlattenedPurchaseOrder>, List<POHeaderDetails>>().ConvertUsing(new FlattenedToPurchaseOrderCustomTypeConverter());
            #endregion            
        }
    }
}
