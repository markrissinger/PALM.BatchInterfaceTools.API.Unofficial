using AutoMapper;
using AutoMapper.Execution;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.Library.Entities.CommitmentControl.InboundBudgetJournal;

namespace PALM.BatchInterfaceTools.API.Helpers.Mapper.CustomTypeConverters
{
    public class FlattenedToBudgetHeader : ITypeConverter<List<FlattenedBudgetJournal>, List<KKBudgetHeader>>
    {
        public List<KKBudgetHeader> Convert(List<FlattenedBudgetJournal> source, List<KKBudgetHeader> destination, ResolutionContext context)
        {
            // handle potential null
            if (context == null || source is null)
                return null;

            List<KKBudgetHeader> budgetHeaders = new();

            // group by header fields
            var groupedByHeaders = source
                .GroupBy(flatBudgetJournal => new
                {
                    flatBudgetJournal.BusinessUnit,
                    flatBudgetJournal.JournalDate,
                    flatBudgetJournal.LongDescription,
                    flatBudgetJournal.LedgerGroup,
                    flatBudgetJournal.BudgetEntryType,
                    flatBudgetJournal.AlternateDescription
                });

            foreach ( var groupedByHeader in groupedByHeaders )
            {
                KKBudgetHeader budgetHeader = new();
                MapBudgetHeader(groupedByHeader.First(), budgetHeader);

                foreach ( var flattenedBudgetJournal in groupedByHeader)
                {
                    KKBudgetLine budgetLine = new();
                    MapBudgetLine(flattenedBudgetJournal, budgetLine);
                    budgetHeader.KKBudgetLines.Add(budgetLine);
                }

                budgetHeaders.Add(budgetHeader);
            }

            return budgetHeaders;
        }

        private void MapBudgetHeader(FlattenedBudgetJournal source, KKBudgetHeader destination)
        {
            destination.BusinessUnit = source.BusinessUnit;
            destination.JournalDate = source.JournalDate;
            destination.LongDescription = source.LongDescription;
            destination.LedgerGroup = source.LedgerGroup;
            destination.BudgetEntryType = source.BudgetEntryType;
            destination.AlternateDescription = source.AlternateDescription;
        }

        private void MapBudgetLine(FlattenedBudgetJournal source, KKBudgetLine destination)
        {
            destination.JournalLineNumber = source.JournalLineNumber;
            destination.BudgetPeriod = source.BudgetPeriod;
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
            destination.MonetaryAmount = source.MonetaryAmount;
            destination.JournalClass = source.JournalClass;
            destination.JournalLineReference = source.JournalLineReference;
            destination.JournalLineReferenceDescription = source.JournalLineReferenceDescription;
        }
    }
}
