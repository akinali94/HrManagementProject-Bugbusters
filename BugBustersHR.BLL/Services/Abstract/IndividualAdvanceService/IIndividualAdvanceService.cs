﻿using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.ENTITY.Concrete;

namespace BugBustersHR.BLL.Services.Abstract.IndividualAdvanceService
{
    public interface IIndividualAdvanceService:IService<IndividualAdvance>
    {
        IndividualAdvance GetByIdIndividualAdvanceRequest(int id);
        IEnumerable<IndividualAdvance> GetAllIndividualAdvanceReq();


        void RemainCalculation(IndividualAdvance ındividual);
        Task TChangeToTrueforAdvance(int id);
        Task TChangeToFalseforAdvance(int id);
        Task GetAdvanceApprovelName(IEnumerable<IndividualAdvanceRequestVM> request);

    }
}
