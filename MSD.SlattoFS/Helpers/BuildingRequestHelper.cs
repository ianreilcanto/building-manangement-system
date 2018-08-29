using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSD.SlattoFS.Helpers
{
    public class BuildingRequestResult
    {
        public BuildingRequestResult()
        {
            AccountId = -1;
            BuildingId = -1;
            IsValid = false;
        }
        public int AccountId { get; set; }
        public int BuildingId { get; set; }
        public bool IsValid { get; set; }
    }

    public class ApartmentRequestResult
    {
        public ApartmentRequestResult()
        {
            AccountId = -1;
            BuildingId = -1;
            ApartmentId = -1;
            IsValid = false;
        }
        public int AccountId { get; set; }
        public int BuildingId { get; set; }
        public int ApartmentId { get; set; }
        public bool IsValid { get; set; }
    }

    /*
        TODO NOTE: THIS NEEDS to be optimized!!
        using the Request object is a mess. ViewModels are the way to go but 
        in so far as the vm binding goes, still have not found a way to do it in angularjs via $http
         */

    public static class BuildingRequestHelper
    {
        private const string ACCOUNTID_KEY = "accountId";
        private const string BUILDINGID_KEY = "buildingId";
        private const string APARTMENTID_KEY = "apartmentId";

        public static BuildingRequestResult GetRequestResult(this HttpRequestBase request)
        {

            var result = new BuildingRequestResult();
            
            //guard clause
            if (request.Form[ACCOUNTID_KEY] == null || request.Form[BUILDINGID_KEY] == null)
            {
                result.IsValid = false;
            }
            else
            {
                int accountId = -1;
                int.TryParse(request.Form[ACCOUNTID_KEY].ToString(), out accountId);

                int buildingId = -1;
                int.TryParse(request.Form[BUILDINGID_KEY].ToString(), out buildingId);

                result.AccountId = accountId;
                result.BuildingId = buildingId;

                result.IsValid = accountId > 0 && buildingId > 0;
            }
         
            return result;
        }

        public static ApartmentRequestResult GetAptRequestResult(this HttpRequestBase request)
        {
            var result = new ApartmentRequestResult();
            if (request.Form[ACCOUNTID_KEY] == null || request.Form[BUILDINGID_KEY] == null || request.Form[APARTMENTID_KEY] == null)
            {
                result.IsValid = false;
            }else
            {
                var bldgReqResult = GetRequestResult(request);
                if (bldgReqResult.IsValid)
                {
                    int apartmentId = -1;
                    int.TryParse(request.Form[APARTMENTID_KEY].ToString(), out apartmentId);

                    result.AccountId = bldgReqResult.AccountId;
                    result.BuildingId = bldgReqResult.BuildingId;
                    result.ApartmentId = apartmentId;
                    result.IsValid = bldgReqResult.IsValid && apartmentId > 0;
                }
            }

            return result;
        }

    }
}