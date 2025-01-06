using AutoMapper;
using PALM.BatchInterfaceTools.API.DTO;
using PALM.BatchInterfaceTools.Library.Entities.CommitmentControl.InboundBudgetJournal;

namespace PALM.BatchInterfaceTools.API.Helpers.Mapper.CustomTypeConverters
{
    public class BudgetHeaderToFlattenedBudgetJournal : ITypeConverter<List<KKBudgetHeader>, List<FlattenedBudgetJournal>>
    {
        public List<FlattenedBudgetJournal> Convert(List<KKBudgetHeader> source, List<FlattenedBudgetJournal> destination, ResolutionContext context)
        {
            // handle potential null
            if (context == null || source is null)
                return null;

            var flattenedBudgetJournals = new List<FlattenedBudgetJournal>();

            foreach (var budgetHeader in source)
            {
                // loop over budget lines while creating flattened objects from them
                foreach (var budgetJournalLine in budgetHeader.KKBudgetLines)
                {
                    FlattenedBudgetJournal flattenedBudgetJournal = new();
                    MapBudgetHeader(budgetHeader, flattenedBudgetJournal);
                    MapBudgetLine(budgetJournalLine, flattenedBudgetJournal);
                    flattenedBudgetJournals.Add(flattenedBudgetJournal);
                }
            }

            return flattenedBudgetJournals;
        }
        private void MapBudgetHeader(KKBudgetHeader source, FlattenedBudgetJournal destination)
        {
            destination.BusinessUnit = source.BusinessUnit;
            destination.JournalDate = source.JournalDate;
            destination.LongDescription = source.LongDescription;
            destination.LedgerGroup = source.LedgerGroup;
            destination.BudgetEntryType = source.BudgetEntryType;
            destination.AlternateDescription = source.AlternateDescription;
        }

        private void MapBudgetLine(KKBudgetLine source, FlattenedBudgetJournal destination)
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
