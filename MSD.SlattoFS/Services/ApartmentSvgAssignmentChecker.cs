using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Models.Pocos;
using MSD.SlattoFS.Repositories;
using MSD.SlattoFS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace MSD.SlattoFS.Services
{
    public class ApartmentSvgAssignmentChecker
    {
        private readonly IPocoRepository<Apartment> _apartmentRepo;
        private readonly IPocoRepository<SvgData> _svgDataRepo;
        private readonly string _apartmentIdAttributeName;
        private readonly string _apartmentElementGroupName;

        public ApartmentSvgAssignmentChecker(string apartmentIdAttributeName = "apt-id", string apartmentElementGroupName = "g")
        {
            _apartmentRepo = new ApartmentRepository();
            _svgDataRepo = new SVGDataRepository();
            _apartmentIdAttributeName = apartmentIdAttributeName;
            _apartmentElementGroupName = apartmentElementGroupName;
        }

        #region Public Methods
        public ApartmentAssignmentData GetApartmentAssignmentData(int buildingId)
        {
            ApartmentAssignmentData data = new ApartmentAssignmentData();
            data.TotalApartments = CountTotalApartments(buildingId);
            data.AssignedApartments = GetApartmentAssignments(buildingId);
            if (data.AssignedApartments.Count > 0)
            {
                data.UnassignedApartments = ((ApartmentRepository)_apartmentRepo).GetAllExcept(data.AssignedApartments.Select(a => a.Apartment.Id).ToList());
            }
            else
            {
                data.UnassignedApartments = ((ApartmentRepository)_apartmentRepo).GetAllByBuildingId(buildingId);
            }
            return data;
        }
        #endregion

        #region Private Methods
        private int CountTotalApartments(int buildingId)
        {
            return ((ApartmentRepository)_apartmentRepo).CountApartments(buildingId);
        }

        private List<Apartment> GetAssignedApartments(string svg)
        {
            List<int> apartmentIDs = GetAttributeValue(_apartmentIdAttributeName, svg).ToList();
            List<Apartment> apartments = ((ApartmentRepository)_apartmentRepo).GetByIds(apartmentIDs);
            return apartments;
        }

        private List<int> GetAttributeValue(string attribute, string tag)
        {
            string data = "<svg>" + tag + "</svg>";
            XDocument document = XDocument.Parse(data);
            var elements = document.Elements("svg").Elements(_apartmentElementGroupName);

            List<int> ID = new List<int>();
            foreach (var element in elements.Attributes(attribute))
                ID.Add(int.Parse(element.Value));
            return ID;
        }

        /// <summary>
        /// Retrieves the list of apartments with its associated building asset IDs based on the provided building ID.
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns>List<ApartmentAssignment></returns>
        private List<ApartmentAssignment> GetApartmentAssignments(int buildingId)
        {
            IList<SvgData> SVGs = _svgDataRepo.GetAllById(buildingId);

            if (SVGs.Count > 0)
            {
                List<ApartmentAssignment> apartmentAssignments = new List<ApartmentAssignment>();

                foreach (var SVG in SVGs)
                {
                    List<Apartment> apartments = GetAssignedApartments(SVG.Svg);
                    foreach (var apartment in apartments)
                    {
                        ApartmentAssignment apartmentAssignment = new ApartmentAssignment();

                        apartmentAssignment = apartmentAssignments.Where(x => x.Apartment.Id == apartment.Id).FirstOrDefault();
                        if (apartmentAssignment == null)
                        {
                            apartmentAssignment = new ApartmentAssignment();
                            apartmentAssignment.Apartment = apartment;
                            apartmentAssignment.BuildingAssetIds = new List<int>();
                            apartmentAssignment.BuildingAssetIds.Add(SVG.AssetId);
                            apartmentAssignments.Add(apartmentAssignment);
                        }
                        else
                        {
                            apartmentAssignments[apartmentAssignments.IndexOf(apartmentAssignment)].BuildingAssetIds.Add(SVG.AssetId);
                        }
                    }
                }

                return apartmentAssignments;
            }
            else {
                return new List<ApartmentAssignment>();
            }
        }
        #endregion
    }
}