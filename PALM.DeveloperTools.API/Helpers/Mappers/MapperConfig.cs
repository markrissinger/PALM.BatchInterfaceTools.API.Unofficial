using AutoMapper;
using PALM.DeveloperTools.API.DTO;
using PALM.InterfaceLayouts.Unofficial.Entities.InterfaceLayouts.PurchaseOrders.InboundEncumbranceLoad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALM.DeveloperTools.API.Helpers.Mappers
{
    internal static class MapperConfig
    {
        internal static MapperConfiguration MapperConfiguration()
        {
            return new MapperConfiguration(config =>
            {
                // Purchase Order domain object --> DTO
                config.CreateMap<List<POHeaderDetails>, List<FlattenedPurchaseOrder>>().ConvertUsing(new PurchaseOrderToFlattenedCustomTypeConverter());

                // Purchase Order DTO --> domain object
                config.CreateMap<List<FlattenedPurchaseOrder>, List<POHeaderDetails>>().ConvertUsing(new FlattenedToPurchaseOrderCustomTypeConverter());
            });
        } 
    }
}
